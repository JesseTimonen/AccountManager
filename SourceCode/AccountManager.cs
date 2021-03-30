using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Encryption;

namespace AccountManager
{
    public partial class AccountManagerForm : Form
    {
        private string username;
        private string password;
        private string filePath;
        private string encryptionKey;
        private List<KeyValuePair<string, string[]>> accountData = new List<KeyValuePair<string, string[]>>();
        private Random random = new Random();
        private StringBuilder stringBuilder = new StringBuilder();

        private const int accountLimit = 100;
        private List<Panel> accountPanels = new List<Panel>();
        private List<Label> titleLabels = new List<Label>();
        private List<Label> accountLabels = new List<Label>();
        private List<Button> copyURLButtons = new List<Button>();
        private List<Button> copyAccountButtons = new List<Button>();
        private List<Button> copyPasswordButtons = new List<Button>();
        private List<Button> editButtons = new List<Button>();
        private List<Button> deleteButtons = new List<Button>();
        private int editingIndex;
        private int deletedItemIndex = -1;


        public AccountManagerForm()
        {
            InitializeComponent();
        }

        private void FormMinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormQuitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AccountManagerForm_Load(object sender, EventArgs e)
        {
            // Find user's documents folder
            filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AccountManagerData\\";

            // Check has user used the application before
            if (!Directory.Exists(@"" + filePath))
            {
                Directory.CreateDirectory(@"" + filePath);
                LoginPanel.Visible = false;
                RegistrationPanel.Visible = true;
            }

            CreateAccountPlaceholders();
        }


        /*=====================================================*\
        |                    Add Account UI                     |
        \*=====================================================*/
        private void AddAccountTitleInput_Enter(object sender, EventArgs e)
        {
            if (AddAccountTitleInput.ForeColor == Color.Gray)
            {
                AddAccountTitleInput.Clear();
            }

            AddAccountTitleInput.ForeColor = Color.White;
        }

        private void AddAccountTitleInput_Leave(object sender, EventArgs e)
        {
            if (AddAccountTitleInput.Text == "")
            {
                AddAccountTitleInput.Text = "Title";
                AddAccountTitleInput.ForeColor = Color.Gray;
            }
        }

        private void AddAccountURLInput_Enter(object sender, EventArgs e)
        {
            if (AddAccountURLInput.ForeColor == Color.Gray)
            {
                AddAccountURLInput.Clear();
            }

            AddAccountURLInput.ForeColor = Color.White;
        }

        private void AddAccountURLInput_Leave(object sender, EventArgs e)
        {
            if (AddAccountURLInput.Text == "")
            {
                AddAccountURLInput.Text = "URL";
                AddAccountURLInput.ForeColor = Color.Gray;
            }
        }

        private void AddAccountNameInput_Enter(object sender, EventArgs e)
        {
            if (AddAccountNameInput.ForeColor == Color.Gray)
            {
                AddAccountNameInput.Clear();
            }

            AddAccountNameInput.ForeColor = Color.White;
        }

        private void AddAccountNameInput_Leave(object sender, EventArgs e)
        {
            if (AddAccountNameInput.Text == "")
            {
                AddAccountNameInput.Text = "Username";
                AddAccountNameInput.ForeColor = Color.Gray;
            }
        }

        private void AddAccountPasswordInput_Enter(object sender, EventArgs e)
        {
            if (AddAccountPasswordInput.ForeColor == Color.Gray)
            {
                AddAccountPasswordInput.Clear();
            }

            AddAccountPasswordInput.ForeColor = Color.White;
        }

        private void AddAccountPasswordInput_Leave(object sender, EventArgs e)
        {
            if (AddAccountPasswordInput.Text == "")
            {
                AddAccountPasswordInput.Text = "Password";
                AddAccountPasswordInput.ForeColor = Color.Gray;
            }
        }

        private void AddAccountNotesInput_Enter(object sender, EventArgs e)
        {
            if (AddAccountNotesInput.ForeColor == Color.Gray)
            {
                AddAccountNotesInput.Clear();
            }

            AddAccountNotesInput.ForeColor = Color.White;
        }

        private void AddAccountNotesInput_Leave(object sender, EventArgs e)
        {
            if (AddAccountNotesInput.Text == "")
            {
                AddAccountNotesInput.Text = "...";
                AddAccountNotesInput.ForeColor = Color.Gray;
            }
        }

        private void ResetAddAccountUI()
        {
            AddAccountTitleInput.Text = "Title";
            AddAccountTitleInput.ForeColor = Color.Gray;

            AddAccountURLInput.Text = "URL";
            AddAccountURLInput.ForeColor = Color.Gray;

            AddAccountNameInput.Text = "Username";
            AddAccountNameInput.ForeColor = Color.Gray;

            AddAccountPasswordInput.Text = "Password";
            AddAccountPasswordInput.ForeColor = Color.Gray;

            AddAccountNotesInput.Text = "...";
            AddAccountNotesInput.ForeColor = Color.Gray;

            AddAccountFeedback.Text = "";
        }


        /*=====================================================*\
        |                      Add Account                      |
        \*=====================================================*/
        private void AddAccountButton_Click(object sender, EventArgs e)
        {
            AccountsPanel.Visible = false;
            AddAccountButton.Enabled = false;
            SettingsButton.Enabled = false;
            LogoutButton.Enabled = false;
            ResetAddAccountUI();
            AddAccountPanel.Visible = true;
            AddAccountButton.Enabled = false;
        }

        private void AddAccountRandomButton_Click(object sender, EventArgs e)
        {
            AddAccountPasswordInput.Text = CreateRandomString();
            AddAccountPasswordInput.ForeColor = Color.White;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (AddAccountTitleInput.Text == "" || AddAccountTitleInput.ForeColor == Color.Gray)
            {
                AddAccountFeedback.Text = "Title is empty!";
                return;
            }

            accountData.Add(new KeyValuePair<string, string[]>(
                Encrypter.Encrypt(AddAccountTitleInput.Text, encryptionKey),
                new string[] {
                    (AddAccountURLInput.ForeColor == Color.Gray) ? Encrypter.Encrypt("", encryptionKey) : Encrypter.Encrypt(AddAccountURLInput.Text, encryptionKey),
                    (AddAccountNameInput.ForeColor == Color.Gray) ? Encrypter.Encrypt("", encryptionKey) : Encrypter.Encrypt(AddAccountNameInput.Text, encryptionKey),
                    (AddAccountPasswordInput.ForeColor == Color.Gray) ? Encrypter.Encrypt("", encryptionKey) : Encrypter.Encrypt(AddAccountPasswordInput.Text, encryptionKey),
                    (AddAccountNotesInput.ForeColor == Color.Gray) ? Encrypter.Encrypt("", encryptionKey) : Encrypter.Encrypt(AddAccountNotesInput.Text, encryptionKey),
                }
            ));

            SaveData();
            DisplayAccounts();
            AddAccountPanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            AccountsPanel.Visible = true;
        }

        private void CancelAddButton_Click(object sender, EventArgs e)
        {
            AddAccountPanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            AccountsPanel.Visible = true;
        }


        /*=====================================================*\
        |                    Edit Account UI                    |
        \*=====================================================*/
        private void EditAccountTitleInput_Enter(object sender, EventArgs e)
        {
            if (EditAccountTitleInput.ForeColor == Color.Gray)
            {
                EditAccountTitleInput.Clear();
            }

            EditAccountTitleInput.ForeColor = Color.White;
        }

        private void EditAccountTitleInput_Leave(object sender, EventArgs e)
        {
            if (EditAccountTitleInput.Text == "")
            {
                EditAccountTitleInput.Text = "Title";
                EditAccountTitleInput.ForeColor = Color.Gray;
            }
        }

        private void EditAccountURLInput_Enter(object sender, EventArgs e)
        {
            if (EditAccountURLInput.ForeColor == Color.Gray)
            {
                EditAccountURLInput.Clear();
            }

            EditAccountURLInput.ForeColor = Color.White;
        }

        private void EditAccountURLInput_Leave(object sender, EventArgs e)
        {
            if (EditAccountURLInput.Text == "")
            {
                EditAccountURLInput.Text = "Title";
                EditAccountURLInput.ForeColor = Color.Gray;
            }
        }

        private void EditAccountNameInput_Enter(object sender, EventArgs e)
        {
            if (EditAccountNameInput.ForeColor == Color.Gray)
            {
                EditAccountNameInput.Clear();
            }

            EditAccountNameInput.ForeColor = Color.White;
        }

        private void EditAccountNameInput_Leave(object sender, EventArgs e)
        {
            if (EditAccountNameInput.Text == "")
            {
                EditAccountNameInput.Text = "Title";
                EditAccountNameInput.ForeColor = Color.Gray;
            }
        }

        private void EditAccountPasswordInput_Enter(object sender, EventArgs e)
        {
            if (EditAccountPasswordInput.ForeColor == Color.Gray)
            {
                EditAccountPasswordInput.Clear();
            }

            EditAccountPasswordInput.ForeColor = Color.White;
        }

        private void EditAccountPasswordInput_Leave(object sender, EventArgs e)
        {
            if (EditAccountPasswordInput.Text == "")
            {
                EditAccountPasswordInput.Text = "Title";
                EditAccountPasswordInput.ForeColor = Color.Gray;
            }
        }

        private void EditAccountNotesInput_Enter(object sender, EventArgs e)
        {
            if (EditAccountNotesInput.ForeColor == Color.Gray)
            {
                EditAccountNotesInput.Clear();
            }

            EditAccountNotesInput.ForeColor = Color.White;
        }

        private void EditAccountNotesInput_Leave(object sender, EventArgs e)
        {
            if (EditAccountNotesInput.Text == "")
            {
                EditAccountNotesInput.Text = "...";
                EditAccountNotesInput.ForeColor = Color.Gray;
            }
        }


        private void ResetEditAccountUI()
        {
            if (EditAccountTitleInput.Text == "")
            {
                EditAccountTitleInput.Text = "Title";
                EditAccountTitleInput.ForeColor = Color.Gray;
            }

            if (EditAccountURLInput.Text == "")
            {
                EditAccountURLInput.Text = "URL";
                EditAccountURLInput.ForeColor = Color.Gray;
            }

            if (EditAccountNameInput.Text == "")
            {
                EditAccountNameInput.Text = "Username";
                EditAccountNameInput.ForeColor = Color.Gray;
            }

            if (EditAccountPasswordInput.Text == "")
            {
                EditAccountPasswordInput.Text = "Password";
                EditAccountPasswordInput.ForeColor = Color.Gray;
            }

            if (EditAccountNotesInput.Text == "")
            {
                EditAccountNotesInput.Text = "...";
                EditAccountNotesInput.ForeColor = Color.Gray;
            }

            EditAccountFeedback.Text = "";
        }


        /*=====================================================*\
        |                     Edit Account                      |
        \*=====================================================*/
        private void EditAccountButton_Click(object sender, EventArgs e)
        {
            if (EditAccountTitleInput.Text == "" || EditAccountTitleInput.ForeColor == Color.Gray)
            {
                EditAccountFeedback.Text = "Title is empty!";
                return;
            }

            accountData.RemoveAt(editingIndex);
            accountData.Insert(editingIndex, new KeyValuePair<string, string[]>(
                Encrypter.Encrypt(EditAccountTitleInput.Text, encryptionKey),
                new string[] {
                    (EditAccountURLInput.ForeColor == Color.Gray) ? Encrypter.Encrypt("", encryptionKey) : Encrypter.Encrypt(EditAccountURLInput.Text, encryptionKey),
                    (EditAccountNameInput.ForeColor == Color.Gray) ? Encrypter.Encrypt("", encryptionKey) : Encrypter.Encrypt(EditAccountNameInput.Text, encryptionKey),
                    (EditAccountPasswordInput.ForeColor == Color.Gray) ? Encrypter.Encrypt("", encryptionKey) : Encrypter.Encrypt(EditAccountPasswordInput.Text, encryptionKey),
                    (EditAccountNotesInput.ForeColor == Color.Gray) ? Encrypter.Encrypt("", encryptionKey) : Encrypter.Encrypt(EditAccountNotesInput.Text, encryptionKey),
                }
            ));
            SaveData();

            DisplayAccounts();
            EditAccountPanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            AccountsPanel.Visible = true;
        }

        private void EditAccountRandomButton_Click(object sender, EventArgs e)
        {
            EditAccountPasswordInput.Text = CreateRandomString(15, 20);
            EditAccountPasswordInput.ForeColor = Color.White;
        }

        private void CancelEditAccountButton_Click(object sender, EventArgs e)
        {
            EditAccountPanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            AccountsPanel.Visible = true;
        }


        /*=====================================================*\
        |                    Delete Account                     |
        \*=====================================================*/
        private void CancelDeleteAccountButton_Click(object sender, EventArgs e)
        {
            ConfirmDeletePanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            deletedItemIndex = -1;
        }

        private void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            accountData.RemoveAt(deletedItemIndex);
            DisplayAccounts();
            SaveData();

            ConfirmDeletePanel.Visible = false;
            AddAccountButton.Enabled = true;
            SettingsButton.Enabled = true;
            LogoutButton.Enabled = true;
            deletedItemIndex = -1;
        }
    }
}