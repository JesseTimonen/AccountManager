using Encryption;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AccountManager
{
    public partial class AccountManagerForm
    {
        /*=====================================================*\
        |                      Settings UI                      |
        \*=====================================================*/
        private void SettingsUsernameInput_Enter(object sender, EventArgs e)
        {
            if (SettingsUsernameInput.ForeColor == Color.Gray)
            {
                SettingsUsernameInput.Clear();
            }

            SettingsUsernameInput.ForeColor = Color.White;
        }

        private void SettingsUsernameInput_Leave(object sender, EventArgs e)
        {
            if (SettingsUsernameInput.Text == "")
            {
                SettingsUsernameInput.Text = "New Username";
                SettingsUsernameInput.ForeColor = Color.Gray;
            }
        }

        private void SettingsCurrentPasswordInput_Enter(object sender, EventArgs e)
        {
            if (SettingsCurrentPasswordInput.ForeColor == Color.Gray)
            {
                SettingsCurrentPasswordInput.Clear();
            }

            SettingsCurrentPasswordInput.PasswordChar = '*';
            SettingsCurrentPasswordInput.ForeColor = Color.White;
        }

        private void SettingsCurrentPasswordInput_Leave(object sender, EventArgs e)
        {
            if (SettingsCurrentPasswordInput.Text == "")
            {
                SettingsCurrentPasswordInput.PasswordChar = '\0';
                SettingsCurrentPasswordInput.Text = "Current password";
                SettingsCurrentPasswordInput.ForeColor = Color.Gray;
            }
        }

        private void SettingsNewPasswordInput_Enter(object sender, EventArgs e)
        {
            if (SettingsNewPasswordInput.ForeColor == Color.Gray)
            {
                SettingsNewPasswordInput.Clear();
            }

            SettingsNewPasswordInput.PasswordChar = '*';
            SettingsNewPasswordInput.ForeColor = Color.White;
        }

        private void SettingsNewPasswordInput_Leave(object sender, EventArgs e)
        {
            if (SettingsNewPasswordInput.Text == "")
            {
                SettingsNewPasswordInput.PasswordChar = '\0';
                SettingsNewPasswordInput.Text = "New password";
                SettingsNewPasswordInput.ForeColor = Color.Gray;
            }
        }

        private void SettingsConfirmPasswordInput_Enter(object sender, EventArgs e)
        {
            if (SettingsConfirmPasswordInput.ForeColor == Color.Gray)
            {
                SettingsConfirmPasswordInput.Clear();
            }

            SettingsConfirmPasswordInput.PasswordChar = '*';
            SettingsConfirmPasswordInput.ForeColor = Color.White;
        }

        private void SettingsConfirmPasswordInput_Leave(object sender, EventArgs e)
        {
            if (SettingsConfirmPasswordInput.Text == "")
            {
                SettingsConfirmPasswordInput.PasswordChar = '\0';
                SettingsConfirmPasswordInput.Text = "Confirm new password";
                SettingsConfirmPasswordInput.ForeColor = Color.Gray;
            }
        }

        private void resetSettingsUI()
        {
            SettingsUsernameInput.Text = username;
            SettingsUsernameInput.ForeColor = Color.White;

            SettingsCurrentPasswordInput.Text = "Current password";
            SettingsCurrentPasswordInput.ForeColor = Color.Gray;
            SettingsCurrentPasswordInput.PasswordChar = '\0';

            SettingsNewPasswordInput.Text = "New password";
            SettingsNewPasswordInput.ForeColor = Color.Gray;
            SettingsNewPasswordInput.PasswordChar = '\0';

            SettingsConfirmPasswordInput.Text = "Confirm new password";
            SettingsConfirmPasswordInput.ForeColor = Color.Gray;
            SettingsConfirmPasswordInput.PasswordChar = '\0';

            SettingsFeedback.Text = "";
        }

        private void SettingsShowCurrentPassword_Click(object sender, EventArgs e)
        {
            SettingsCurrentPasswordInput.PasswordChar = '\0';
        }

        private void SettingsShowNewPassword_Click(object sender, EventArgs e)
        {
            SettingsNewPasswordInput.PasswordChar = '\0';
        }

        private void SettingsShowConfirmNewPassword_Click(object sender, EventArgs e)
        {
            SettingsConfirmPasswordInput.PasswordChar = '\0';
        }


        /*=====================================================*\
        |                       Settings                        |
        \*=====================================================*/
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            AccountsPanel.Visible = false;
            AddAccountButton.Enabled = false;
            SettingsButton.Enabled = false;
            LogoutButton.Enabled = false;
            resetSettingsUI();
            SettingsPanel.Visible = true;
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            if (!validateUsername(SettingsUsernameInput.Text))
            {
                SettingsFeedback.Text = "Invalid username, please change the name before saving!";
                return;
            }

            if (File.Exists(@"" + filePath + SettingsUsernameInput.Text.ToLower() + ".txt") && SettingsUsernameInput.Text.ToLower() != username)
            {
                SettingsFeedback.Text = "This username is already in use!";
                return;
            }

            if ((SettingsNewPasswordInput.Text != "" && SettingsNewPasswordInput.ForeColor == Color.White) || (SettingsConfirmPasswordInput.Text != "" && SettingsConfirmPasswordInput.ForeColor == Color.White))
            {
                // Change username and password
                if (Encrypter.SHA256(SettingsCurrentPasswordInput.Text) != password)
                {
                    SettingsFeedback.Text = "Access denied!";
                    return;
                }

                if (!validatePassword(SettingsNewPasswordInput.Text, SettingsConfirmPasswordInput.Text))
                {
                    SettingsFeedback.Text = "Invalid password, please make sure password and confirm password match and they are both atleast 8 characters long and contains both capital and lower letter!";
                    return;
                }

                if (SettingsUsernameInput.Text.ToLower() != username)
                {
                    if (File.Exists(@"" + filePath + username + ".txt"))
                    {
                        deleteFile(username);
                    }

                    username = SettingsUsernameInput.Text.ToLower();
                }

                if (accountData.Any())
                {
                    List<KeyValuePair<string, string[]>> tempData = Encrypter.DecryptData(accountData, encryptionKey);
                    encryptionKey = SettingsNewPasswordInput.Text;
                    password = Encrypter.SHA256(SettingsNewPasswordInput.Text);
                    accountData = Encrypter.EncryptData(tempData, encryptionKey);
                }
                else
                {
                    encryptionKey = SettingsNewPasswordInput.Text;
                    password = Encrypter.SHA256(SettingsNewPasswordInput.Text);
                }

                SaveData();
            }
            else
            {
                // Change only username
                if (SettingsUsernameInput.Text.ToLower() != username)
                {
                    if (File.Exists(@"" + filePath + username + ".txt"))
                    {
                        deleteFile(username);
                    }

                    username = SettingsUsernameInput.Text.ToLower();
                    SaveData();
                }
            }

            SettingsPanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            AccountsPanel.Visible = true;
        }

        private void CancelSettingsButton_Click(object sender, EventArgs e)
        {
            SettingsPanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            AccountsPanel.Visible = true;
        }
    }
}