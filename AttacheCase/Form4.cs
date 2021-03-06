﻿//---------------------------------------------------------------------- 
// "アタッシェケース#3 ( AttachéCase#3 )" -- File encryption software.
// Copyright (C) 2016  Mitsuhiro Hibara
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------- 
using System;
using System.Drawing;
using System.Windows.Forms;
using AttacheCase.Properties;

namespace AttacheCase
{
  public partial class Form4 : Form
	{

		private bool fLoading = false;

		// Show dialog type ( string )
		private string _FormType;
		public string FormType
		{
			get { return _FormType; }
			set { _FormType = value; }
		}

		// Overwrite opiton
		private int _OverWriteOption = -1;
		public int OverWriteOption
		{
			get { return _OverWriteOption; }
		}

		// Overwrite option for new date file or folder only.
		private bool _OverWriteForNewDate;
		public bool OverWriteForNewDate
		{
			get { return _OverWriteForNewDate; }
		}

		// File or Directory path to overwrite.
		private string _OverWriteFilePath;
		public string OverWriteFilePath
		{
			set { _OverWriteFilePath = value; }
		}

		// Ask to encrypt or decrypt regardless of file content.
		private int _AskEncryptOrDecrypt;
		public int AskEncryptOrDecrypt
		{
			get { return _AskEncryptOrDecrypt; }
		}
		

		public Form4(string InputType, string MessageText)
		{

			InitializeComponent();

			fLoading = true;

			tabControl1.Visible = false;
			panelInputPassword.Parent = panelOuter;
			panelOverwriteConfirm.Parent = panelOuter;
			panelAskEncryptOrDecrypt.Parent = panelOuter;

			_FormType = InputType;

			//-----------------------------------
			// パスワード入力ウィンドウ
			// Input password window 
			if (_FormType == "EncryptPassword" || _FormType == "DecryptPassword")
			{
				panelInputPassword.Visible = true;
				this.Text = Resources.DialogTitleQuestion;
				checkBoxNotMaskEncryptedPassword.Checked = AppSettings.Instance.fNotMaskPassword;

				if (_FormType == "EncryptPassword")
				{
					textBoxPassword.Text = AppSettings.Instance.MyEncryptPasswordString;
					textBoxRePassword.Text = AppSettings.Instance.MyEncryptPasswordString;
				}
				else if (_FormType == "DecryptPassword")
				{
					textBoxPassword.Text = AppSettings.Instance.MyDecryptPasswordString;
					textBoxRePassword.Text = AppSettings.Instance.MyDecryptPasswordString;
				}
				
			}
			//-----------------------------------
			// 上書き確認ダイアログ
			// Dialog of confirming to overwrite 
			else if (_FormType == "ComfirmToOverwriteFile" || _FormType == "ComfirmToOverwriteDir")
			{
				panelOverwriteConfirm.Visible = true;
				this.Text = Resources.DialogTitleQuestion;
				labelMessageText.Text = MessageText;
			}
			//-----------------------------------
			//暗号化、復号の選択ダイアログ
			else if (_FormType == "AskEncryptOrDecrypt")
			{
				panelOverwriteConfirm.Visible = true;
				this.Text = Resources.DialogTitleQuestion;
			}
			//-----------------------------------
			//無指定？
			else
			{
				return;
			}

		}

		private void Form4_Shown(object sender, EventArgs e)
		{
			//-----------------------------------
			// パスワード入力ウィンドウ
			// Input password window 
			if ( panelInputPassword.Visible == true )
			{
				textBoxPassword.Focus();
				textBoxPassword.SelectAll();
			}
			//-----------------------------------
			// 上書きウィンドウ
			// Confirm to overwrite window 
			else if (panelOverwriteConfirm.Visible == true)
			{
				buttonOverwriteNo.Focus();
			}

			fLoading = false;

		}

		private void checkBoxNotMaskEncryptedPassword_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxNotMaskEncryptedPassword.Checked == true)
			{
				textBoxPassword.PasswordChar = (char)0;
				textBoxRePassword.PasswordChar = (char)0;
				textBoxPassword.UseSystemPasswordChar = false;
				textBoxRePassword.UseSystemPasswordChar = false;
			}
			else
			{
				textBoxPassword.UseSystemPasswordChar = true;
				textBoxRePassword.UseSystemPasswordChar = true;
				textBoxPassword.PasswordChar = '*';
				textBoxRePassword.PasswordChar = '*';
			}
		}
		
		//======================================================================
		// パスワード入力ウィンドウ
		// Input password window 
		//======================================================================
		#region
		private void textBoxPassword_TextChanged(object sender, EventArgs e)
		{
			if (fLoading == true)
			{
				return;
			}
			
			if (textBoxPassword.Text.Length == 0)
			{
				textBoxRePassword.Enabled = false;
				textBoxRePassword.BackColor = SystemColors.ButtonFace;
			}
			else
			{
				textBoxRePassword.Enabled = true;
				textBoxRePassword.BackColor = SystemColors.Window;

				if (textBoxPassword.Text == textBoxRePassword.Text)
				{
					buttonPasswordOK.Enabled = true;
					pictureBoxPasswordValid.Visible = true;
					labelPasswordValid.Visible = true;
				}
				else
				{
					buttonPasswordOK.Enabled = false;
					pictureBoxPasswordValid.Visible = false;
					labelPasswordValid.Visible = false;
				}
			}
		}

		private void textBoxRePassword_TextChanged(object sender, EventArgs e)
		{
			if (fLoading == true)
			{
				return;
			}

			if (textBoxRePassword.Text.Length > 0)
			{
				if (textBoxPassword.Text == textBoxRePassword.Text)
				{
					// Light green
					textBoxRePassword.BackColor = Color.Honeydew;
					buttonPasswordOK.Enabled = true;
					pictureBoxPasswordValid.Visible = true;
					labelPasswordValid.Visible = true;

				}
				else
				{
					// Light pink
					textBoxRePassword.BackColor = Color.MistyRose;
					buttonPasswordOK.Enabled = false;
					pictureBoxPasswordValid.Visible = false;
					labelPasswordValid.Visible = false;
				}
			}
		}

		private void Form4_KeyDown(object sender, KeyEventArgs e)
		{
			if (panelInputPassword.Visible == true)
			{
				if (e.KeyCode == Keys.Enter)	// Enter key
				{
					if (textBoxPassword.Focused == true)
					{
						textBoxRePassword.Focus();
						textBoxRePassword.SelectAll();
					}
					else if (textBoxRePassword.Focused == true)
					{
						//OK button
						buttonPasswordOK_Click(sender, e);
					}
				}
			}
		}

		private void buttonPasswordOK_Click(object sender, EventArgs e)
		{
			AppSettings.Instance.fNotMaskPassword = checkBoxNotMaskEncryptedPassword.Checked;

			if (_FormType == "EncryptPassword")
			{
				if (textBoxPassword.Text == textBoxRePassword.Text)
				{
					AppSettings.Instance.MyEncryptPasswordString = textBoxRePassword.Text;
					this.Close();
				}
			}
			else if (_FormType == "DecryptPassword")
			{
				if (textBoxPassword.Text == textBoxRePassword.Text)
				{
					AppSettings.Instance.MyDecryptPasswordString = textBoxRePassword.Text;
					this.Close();
				}
			}
		}

		private void buttonPasswordCancel_Click(object sender, EventArgs e)
		{
			// Not mask password character
			AppSettings.Instance.fNotMaskPassword = checkBoxNotMaskEncryptedPassword.Checked == true ? true : false;
			this.Close();
		}

		#endregion


		//======================================================================
		// 上書き確認ダイアログ
		// Dialog of confirming to overwrite 
		//======================================================================
		#region

		// Temporary option for overwrite ( 0: none, 1: Yes, 2: Overwrite all )
		// private int _OverWriteOption
		private void buttonOverwriteAll_Click(object sender, EventArgs e)
		{
			_OverWriteOption = 2;
			this.Close();

		}
		private void checkBoxOverwriteForNewDate_CheckedChanged(object sender, EventArgs e)
		{
			if (fLoading == true)
			{
				return;
			}

			// Overwrite for new file date only.
			_OverWriteForNewDate = true;
			this.Close();

		}

		private void buttonOverwriteYes_Click(object sender, EventArgs e)
		{
			_OverWriteOption = 1;
			this.Close();

		}

		private void buttonOverwriteNo_Click(object sender, EventArgs e)
		{
			_OverWriteOption = 0;
			this.Close();

		}

		private void buttonOverwriteCancel_Click(object sender, EventArgs e)
		{
			_OverWriteOption = -1;
			this.Close();
		}

		#endregion

		//======================================================================
		// 暗号化か復号処理かを問い合わせる
		// Ask to encrypt or decrypt regardless of file content 
		//======================================================================
		#region
		private void buttonEncrypt_Click(object sender, EventArgs e)
		{
			_AskEncryptOrDecrypt = 1;
			this.Close();
		}

		private void buttonDecrypt_Click(object sender, EventArgs e)
		{
			_AskEncryptOrDecrypt = 2;
			this.Close();
		}

		private void buttonAskEncryptOrDecryptCancel_Click(object sender, EventArgs e)
		{
			_AskEncryptOrDecrypt = -1;
			this.Close();
		}

		#endregion


	}

}
