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
        private void SettingsUsernameInput_Leave(object sender, EventArgs e)
        {
            if (SettingsUsernameInput.Text == "")
            {
                SettingsUsernameInput.Text = username;
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

        private void SettingsMinPasswordInput_Enter(object sender, EventArgs e)
        {
            if (SettingsMinPasswordInput.ForeColor == Color.Gray)
            {
                SettingsMinPasswordInput.Clear();
            }

            SettingsMinPasswordInput.ForeColor = Color.White;
        }

        private void SettingsMinPasswordInput_Leave(object sender, EventArgs e)
        {
            SettingsFeedback.Text = "";

            if (SettingsMinPasswordInput.Text == "")
            {
                if (int.Parse(SettingsMaxPasswordInput.Text) < 16)
                {
                    SettingsMinPasswordInput.Text = SettingsMaxPasswordInput.Text;
                    SettingsMinPasswordInput.ForeColor = Color.White;
                }
                else
                {
                    SettingsMinPasswordInput.Text = "16";
                    SettingsMinPasswordInput.ForeColor = Color.Gray;
                }
            }
            else if (int.Parse(SettingsMinPasswordInput.Text) < 8)
            {
                SettingsMinPasswordInput.Text = "8";
                SettingsFeedback.Text = "Password min lenght can not be lower than 8!";
            }

            if (int.Parse(SettingsMinPasswordInput.Text) > int.Parse(SettingsMaxPasswordInput.Text))
            {
                SettingsMinPasswordInput.Text = SettingsMaxPasswordInput.Text;
                SettingsFeedback.Text = "Password min lenght can not be higher than password max lenght!";
            }
        }

        private void SettingsMaxPasswordInput_Enter(object sender, EventArgs e)
        {
            if (SettingsMaxPasswordInput.ForeColor == Color.Gray)
            {
                SettingsMaxPasswordInput.Clear();
            }

            SettingsMaxPasswordInput.ForeColor = Color.White;
        }

        private void SettingsMaxPasswordInput_Leave(object sender, EventArgs e)
        {
            SettingsFeedback.Text = "";

            if (SettingsMaxPasswordInput.Text == "")
            {
                if (int.Parse(SettingsMinPasswordInput.Text) > 24)
                {
                    SettingsMaxPasswordInput.Text = SettingsMinPasswordInput.Text;
                    SettingsMaxPasswordInput.ForeColor = Color.White;
                }
                else
                {
                    SettingsMaxPasswordInput.Text = "24";
                    SettingsMaxPasswordInput.ForeColor = Color.Gray;
                }
            }
            else if (int.Parse(SettingsMaxPasswordInput.Text) > 50)
            {
                SettingsMaxPasswordInput.Text = "50";
                SettingsFeedback.Text = "Password max lenght can not be higher than 50!";
            }

            if (int.Parse(SettingsMaxPasswordInput.Text) < int.Parse(SettingsMinPasswordInput.Text))
            {
                SettingsMaxPasswordInput.Text = SettingsMinPasswordInput.Text;
                SettingsFeedback.Text = "Password max lenght can not be lower than password min lenght!";
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
            AccountSearchPanel.Visible = false;
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

            // Change username and password
            if ((SettingsNewPasswordInput.Text != "" && SettingsNewPasswordInput.ForeColor == Color.White) || (SettingsConfirmPasswordInput.Text != "" && SettingsConfirmPasswordInput.ForeColor == Color.White))
            {
                if (Encrypter.SHA256(SettingsCurrentPasswordInput.Text) != password)
                {
                    SettingsFeedback.Text = "Current password was wrong, new password was not saved!";
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
            }
            else // Change only username
            {
                if (SettingsUsernameInput.Text.ToLower() != username)
                {
                    if (File.Exists(@"" + filePath + username + ".txt"))
                    {
                        deleteFile(username);
                    }

                    username = SettingsUsernameInput.Text.ToLower();
                }
            }

            SaveData();

            SettingsPanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            AccountsPanel.Visible = true;
            AccountSearchPanel.Visible = true;

            if (accountData.Count >= accountLimit)
            {
                AddAccountButton.Enabled = false;
            }
        }

        private void CancelSettingsButton_Click(object sender, EventArgs e)
        {
            SettingsPanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            AccountsPanel.Visible = true;
            AccountSearchPanel.Visible = true;

            if (accountData.Count >= accountLimit)
            {
                AddAccountButton.Enabled = false;
            }
        }
    }
}