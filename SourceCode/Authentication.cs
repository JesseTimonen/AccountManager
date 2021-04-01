using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Encryption;

namespace AccountManager
{
    public partial class AccountManagerForm
    {
        /*=====================================================*\
        |                   Login UI visuals                    |
        \*=====================================================*/
        private void LoginUsernameInput_Enter(object sender, EventArgs e)
        {
            if (LoginUsernameInput.ForeColor == Color.Gray)
            {
                LoginUsernameInput.Clear();
            }

            LoginUsernameInput.ForeColor = Color.White;
            LoginUnderline1.BackColor = Color.FromArgb(78, 184, 206);
        }

        private void LoginUsernameInput_Leave(object sender, EventArgs e)
        {
            if (LoginUsernameInput.Text == "")
            {
                LoginUsernameInput.Text = "Username";
                LoginUsernameInput.ForeColor = Color.Gray;
            }

            LoginUnderline1.BackColor = Color.White;
        }

        private void LoginPasswordInput_Enter(object sender, EventArgs e)
        {
            if (LoginPasswordInput.ForeColor == Color.Gray)
            {
                LoginPasswordInput.Clear();
            }

            LoginPasswordInput.PasswordChar = '*';
            LoginPasswordInput.ForeColor = Color.White;
            LoginUnderline2.BackColor = Color.FromArgb(78, 184, 206);
        }

        private void LoginPasswordInput_Leave(object sender, EventArgs e)
        {
            if (LoginPasswordInput.Text == "")
            {
                LoginPasswordInput.Text = "Password";
                LoginPasswordInput.PasswordChar = '\0';
                LoginPasswordInput.ForeColor = Color.Gray;
            }

            LoginUnderline2.BackColor = Color.White;
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible = false;
            ResetLoginUI();
            ResetRegistrationUI();
            RegistrationPanel.Visible = true;
        }

        private void ResetLoginUI()
        {
            LoginUsernameInput.Text = "Username";
            LoginUsernameInput.ForeColor = Color.Gray;

            LoginPasswordInput.Text = "Password";
            LoginPasswordInput.PasswordChar = '\0';
            LoginPasswordInput.ForeColor = Color.Gray;

            LoginFeedback.Text = "";

            LoginUnderline1.BackColor = Color.White;
            LoginUnderline2.BackColor = Color.White;
        }


        /*=====================================================*\
        |                Registration UI visuals                |
        \*=====================================================*/
        private void RegisterUsernameInput_Enter(object sender, EventArgs e)
        {
            if (RegistrationUsernameInput.ForeColor == Color.Gray)
            {
                RegistrationUsernameInput.Clear();
            }

            RegistrationUsernameInput.ForeColor = Color.White;
            RegistrationUnderline1.BackColor = Color.FromArgb(78, 184, 206);
        }

        private void RegistrationUsernameInput_Leave(object sender, EventArgs e)
        {
            if (RegistrationUsernameInput.Text == "")
            {
                RegistrationUsernameInput.Text = "Username";
                RegistrationUsernameInput.ForeColor = Color.Gray;
            }

            RegistrationUnderline1.BackColor = Color.White;
        }

        private void RegisterPasswordInput_Enter(object sender, EventArgs e)
        {
            if (RegistrationPasswordInput.ForeColor == Color.Gray)
            {
                RegistrationPasswordInput.Clear();
            }

            RegistrationPasswordInput.PasswordChar = '*';
            RegistrationPasswordInput.ForeColor = Color.White;
            RegistrationUnderline2.BackColor = Color.FromArgb(78, 184, 206);
        }

        private void RegistrationPasswordInput_Leave(object sender, EventArgs e)
        {
            if (RegistrationPasswordInput.Text == "")
            {
                RegistrationPasswordInput.Text = "Password";
                RegistrationPasswordInput.PasswordChar = '\0';
                RegistrationPasswordInput.ForeColor = Color.Gray;
            }

            RegistrationUnderline2.BackColor = Color.White;
        }

        private void RegisterConfirmPasswordInput_Enter(object sender, EventArgs e)
        {
            if (RegistrationConfirmPasswordInput.ForeColor == Color.Gray)
            {
                RegistrationConfirmPasswordInput.Clear();
            }

            RegistrationConfirmPasswordInput.PasswordChar = '*';
            RegistrationConfirmPasswordInput.ForeColor = Color.White;
            RegistrationUnderline3.BackColor = Color.FromArgb(78, 184, 206);
        }

        private void RegistrationConfirmPasswordInput_Leave(object sender, EventArgs e)
        {
            if (RegistrationConfirmPasswordInput.Text == "")
            {
                RegistrationConfirmPasswordInput.Text = "Confirm password";
                RegistrationConfirmPasswordInput.PasswordChar = '\0';
                RegistrationConfirmPasswordInput.ForeColor = Color.Gray;
            }

            RegistrationUnderline3.BackColor = Color.White;
        }

        private void RegistrationUsernameInput_TextChanged(object sender, EventArgs e)
        {
            OverwriteAccountButton.Visible = false;
        }

        private void ReturnLoginButton_Click(object sender, EventArgs e)
        {
            RegistrationPanel.Visible = false;
            ResetRegistrationUI();
            ResetLoginUI();
            LoginPanel.Visible = true;
        }

        private void ResetRegistrationUI()
        {
            RegistrationUsernameInput.Text = "Username";
            RegistrationUsernameInput.ForeColor = Color.Gray;

            RegistrationPasswordInput.Text = "Password";
            RegistrationPasswordInput.PasswordChar = '\0';
            RegistrationPasswordInput.ForeColor = Color.Gray;

            RegistrationConfirmPasswordInput.Text = "Confirm password";
            RegistrationConfirmPasswordInput.PasswordChar = '\0';
            RegistrationConfirmPasswordInput.ForeColor = Color.Gray;

            RegistrationFeedback.Text = "";

            RegistrationUnderline1.BackColor = Color.White;
            RegistrationUnderline2.BackColor = Color.White;
            RegistrationUnderline3.BackColor = Color.White;
        }


        /*=====================================================*\
        |                         Login                         |
        \*=====================================================*/
        private void LoginUsernameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (LoginUsernameInput.ForeColor != Color.Gray && LoginPasswordInput.ForeColor != Color.Gray)
                {
                    if (LoginUsernameInput.Text != "")
                    {
                        login();
                    }
                }
                else if (LoginPasswordInput.ForeColor == Color.Gray)
                {
                    LoginPasswordInput.Focus();
                }
            }
        }

        private void LoginPasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (LoginUsernameInput.ForeColor != Color.Gray && LoginPasswordInput.ForeColor != Color.Gray)
                {
                    if (LoginPasswordInput.Text != "")
                    {
                        login();
                    }
                }
                else if (LoginUsernameInput.ForeColor == Color.Gray)
                {
                    LoginUsernameInput.Focus();
                }
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            login();
        }

        private void login()
        {
            username = LoginUsernameInput.Text.ToLower();

            // Return if user doesn't have a data file
            if (!File.Exists(@"" + filePath + username + ".txt"))
            {
                LoginFeedback.Text = "Account doesn't exist!";
                return;
            }

            if (!GetData(username))
            {
                LoginFeedback.Text = "Failed to load account data!";
                return;
            }

            // Check if password was correct
            if (Encrypter.SHA256(LoginPasswordInput.Text) == password)
            {
                encryptionKey = LoginPasswordInput.Text;
                LoginPanel.Visible = false;
                MainPanel.Visible = true;
                ResetLoginUI();
                DisplayAccounts();
            }
            else
            {
                LoginFeedback.Text = "Access denied!";
                ClearData();
            }
        }


        /*=====================================================*\
        |                    Create Account                     |
        \*=====================================================*/
        private void RegistrationUsernameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (RegistrationUsernameInput.ForeColor != Color.Gray && RegistrationPasswordInput.ForeColor != Color.Gray && RegistrationConfirmPasswordInput.ForeColor != Color.Gray)
                {
                    if (RegistrationUsernameInput.Text != "")
                    {
                        CreateAccount();
                    }
                }
                else if (RegistrationPasswordInput.ForeColor == Color.Gray)
                {
                    RegistrationPasswordInput.Focus();
                }
                else if (RegistrationConfirmPasswordInput.ForeColor == Color.Gray)
                {
                    RegistrationConfirmPasswordInput.Focus();
                }
            }
        }

        private void RegistrationPasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (RegistrationUsernameInput.ForeColor != Color.Gray && RegistrationPasswordInput.ForeColor != Color.Gray && RegistrationConfirmPasswordInput.ForeColor != Color.Gray)
                {
                    if (RegistrationPasswordInput.Text != "")
                    {
                        CreateAccount();
                    }
                }
                else if (RegistrationConfirmPasswordInput.ForeColor == Color.Gray)
                {
                    RegistrationConfirmPasswordInput.Focus();
                }
                else if (RegistrationUsernameInput.ForeColor == Color.Gray)
                {
                    RegistrationUsernameInput.Focus();
                }
            }
        }

        private void RegistrationConfirmPasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (RegistrationUsernameInput.ForeColor != Color.Gray && RegistrationPasswordInput.ForeColor != Color.Gray && RegistrationConfirmPasswordInput.ForeColor != Color.Gray)
                {
                    if (RegistrationConfirmPasswordInput.Text != "")
                    {
                        CreateAccount();
                    }
                }
                else if (RegistrationPasswordInput.ForeColor == Color.Gray)
                {
                    RegistrationPasswordInput.Focus();
                }
                else if (RegistrationUsernameInput.ForeColor == Color.Gray)
                {
                    RegistrationUsernameInput.Focus();
                }
            }
        }

        private void CreateAccountButton_Click(object sender, EventArgs e)
        {
            CreateAccount();
        }

        private void OverwriteAccountButton_Click(object sender, EventArgs e)
        {
            CreateAccount(true);
        }

        private bool ValidateRegistration(bool overwrite = false)
        {
            // Check does account already exist
            if (File.Exists(@"" + filePath + RegistrationUsernameInput.Text.ToLower() + ".txt"))
            {
                if (!overwrite)
                {
                    RegistrationFeedback.Text = "Account already exists!";
                    OverwriteAccountButton.Visible = true;
                    return false;
                }
            }
            else
            {
                OverwriteAccountButton.Visible = false;
            }

            if (!validateUsername(RegistrationUsernameInput.Text))
            {
                RegistrationFeedback.Text = "Invalid username!";
                return false;
            }

            if (!validatePassword(RegistrationPasswordInput.Text, RegistrationConfirmPasswordInput.Text))
            {
                RegistrationFeedback.Text = "Invalid password, please make sure password and confirm password match and they are both atleast 8 characters long and contains both capital and lower letter!";
                return false;
            }

            return true;
        }

        private void CreateAccount(bool overwrite = false)
        {
            if (!ValidateRegistration(overwrite)) return;

            if (overwrite)
            {
                // Delete previous account
                if (File.Exists(@"" + filePath + RegistrationUsernameInput.Text.ToLower() + ".txt"))
                {
                    deleteFile(RegistrationUsernameInput.Text.ToLower());
                }
            }

            username = RegistrationUsernameInput.Text.ToLower();

            try
            {
                password = Encrypter.SHA256(RegistrationPasswordInput.Text);
                encryptionKey = RegistrationPasswordInput.Text;
                SaveData();
                ResetRegistrationUI();
                OverwriteAccountButton.Visible = false;
                RegistrationPanel.Visible = false;
                MainPanel.Visible = true;
            }
            catch (Exception)
            {
                RegistrationFeedback.Text = "Failed to create new account!";
            }
        }


        /*=====================================================*\
        |                        Logout                         |
        \*=====================================================*/
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            ClearData();
            MainPanel.Visible = false;
            AddAccountPanel.Visible = false;
            EditAccountPanel.Visible = false;
            LoginPanel.Visible = true;
        }
    }
}