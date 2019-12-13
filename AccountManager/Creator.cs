using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Encryption;

namespace AccountManager
{
    public partial class AccountManagerForm
    {
        private void CreateAccountPlaceholders()
        {
            // Prevent creating new elements if they already exists
            if (accountPanels.Any()) { return; }

            // Create placeholders for UI elements
            for (int i = 0; i < accountLimit; i++)
            {
                Panel accountPanel = CreatePanel(360, 170);
                Label titleLabel = CreateLabel(accountPanel, 20, 20, 320, 25, 12, Color.White);
                Label accountLabel = CreateLabel(accountPanel, 20, 50, 320, 25, 10, Color.Gainsboro);
                Label passwordLabel = CreateLabel(accountPanel, 20, 75, 320, 25, 10, Color.Gainsboro);
                Label maskedPasswordLabel = CreateLabel(accountPanel, 20, 75, 320, 25, 10, Color.Gainsboro);
                Button togglePasswordButton = CreateButton(accountPanel, 20, 110, 60, 30, "Show", titleLabel, accountLabel, passwordLabel, maskedPasswordLabel, i);
                Button copyButton = CreateButton(accountPanel, 85, 110, 60, 30, "Copy", titleLabel, accountLabel, passwordLabel, maskedPasswordLabel, i);
                Button editButton = CreateButton(accountPanel, 150, 110, 60, 30, "Edit", titleLabel, accountLabel, passwordLabel, maskedPasswordLabel, i);
                Button deleteButton = CreateButton(accountPanel, 215, 110, 60, 30, "Delete", titleLabel, accountLabel, passwordLabel, maskedPasswordLabel, i);
                accountPanels.Add(accountPanel);
                titleLabels.Add(titleLabel);
                accountLabels.Add(accountLabel);
                passwordLabels.Add(passwordLabel);
                maskedPasswordLabels.Add(maskedPasswordLabel);
                togglePasswordButtons.Add(togglePasswordButton);
                copyButtons.Add(copyButton);
                editButtons.Add(editButton);
                deleteButtons.Add(deleteButton);
            }
        }

        public Panel CreatePanel(int width, int height)
        {
            Panel panel = new Panel();
            AccountsPanel.Controls.Add(panel);
            panel.Visible = false;
            panel.Width = width;
            panel.Height = height;
            return panel;
        }

        public Label CreateLabel(Panel parent, int x, int y, int width, int height, int fontSize, Color color)
        {
            Label label = new Label();
            parent.Controls.Add(label);
            label.Visible = false;
            label.Left = x;
            label.Top = y;
            label.Width = width;
            label.Height = height;
            label.ForeColor = color;
            label.Font = new Font("Sans Serif", fontSize);
            return label;
        }

        public Button CreateButton(Panel parent, int x, int y, int width, int height, string text, Label titleLabel, Label accountLabel, Label passwordLabel, Label securePasswordLabel, int listIndex)
        {
            Button button = new Button();
            parent.Controls.Add(button);
            button.Visible = false;
            button.Left = x;
            button.Top = y;
            button.Width = width;
            button.Height = height;
            button.Font = new Font("Sans Serif", 8);
            button.FlatStyle = FlatStyle.Flat;
            button.TabStop = false;
            button.TabIndex = 0;
            button.Text = text;
            button.Click += (sender, EventArgs) => { button_Click(sender, EventArgs, titleLabel, accountLabel, passwordLabel, securePasswordLabel, listIndex); };
            return button;
        }

        protected void button_Click(object sender, EventArgs e, Label titleLabel, Label accountLabel, Label passwordLabel, Label securePasswordLabel, int index)
        {
            Button button = (sender as Button);

            if (button.Text == "Show" || button.Text == "Hide")
            {
                button.Text = (button.Text == "Show") ? "Hide" : "Show";
                securePasswordLabel.Visible = !securePasswordLabel.Visible;
                passwordLabel.Visible = !passwordLabel.Visible;
            }
            else if (button.Text == "Copy")
            {
                Clipboard.SetText(passwordLabel.Text);
            }
            else if (button.Text == "Delete")
            {
                deletedItemIndex = index;
                AddAccountButton.Enabled = false;
                SettingsButton.Enabled = false;
                LogoutButton.Enabled = false;
                ConfirmDeleteLabel.Text = "Are you sure you wish to delete account: " + Encrypter.Decrypt(accountData[deletedItemIndex].Key, encryptionKey) + "?";
                ConfirmDeletePanel.Visible = true;
            }
            else if (button.Text == "Edit")
            {
                AccountsPanel.Visible = false;
                AddAccountButton.Enabled = false;
                SettingsButton.Enabled = false;
                LogoutButton.Enabled = false;
                EditAccountPanel.Visible = true;
                EditAccountTitleInput.Text = titleLabel.Text;
                EditAccountNameInput.Text = accountLabel.Text;
                EditAccountPasswordInput.Text = passwordLabel.Text;
                EditAccountTitleInput.ForeColor = Color.White;
                EditAccountNameInput.ForeColor = Color.White;
                EditAccountPasswordInput.ForeColor = Color.White;
                editingIndex = index;
            }
        }

        private void DisplayAccounts()
        {
            ClearUI();

            // Insert new data into UI
            int index = 0;
            foreach (KeyValuePair<string, string[]> data in accountData)
            {
                titleLabels[index].Text = Encrypter.Decrypt(data.Key, encryptionKey);
                accountLabels[index].Text = Encrypter.Decrypt(data.Value[0], encryptionKey);
                passwordLabels[index].Text = Encrypter.Decrypt(data.Value[1], encryptionKey);
                maskedPasswordLabels[index].Text = string.Concat(Enumerable.Repeat("*", passwordLabels[index].Text.Length));
                togglePasswordButtons[index].Text = "Show";

                accountPanels[index].Visible = true;
                titleLabels[index].Visible = true;
                accountLabels[index].Visible = true;
                maskedPasswordLabels[index].Visible = true;
                togglePasswordButtons[index].Visible = true;
                editButtons[index].Visible = true;
                deleteButtons[index].Visible = true;
                copyButtons[index].Visible = true;

                if (index + 1 >= accountLimit)
                {
                    AddAccountButton.Enabled = false;
                    break;
                }

                index++;
            }
        }

        private void ClearUI()
        {
            for (int i = 0; i < accountLimit; i++)
            {
                titleLabels[i].Text = "";
                accountLabels[i].Text = "";
                passwordLabels[i].Text = "";
                maskedPasswordLabels[i].Text = "";

                titleLabels[i].Visible = false;
                accountLabels[i].Visible = false;
                passwordLabels[i].Visible = false;
                maskedPasswordLabels[i].Visible = false;
                togglePasswordButtons[i].Visible = false;
                editButtons[i].Visible = false;
                deleteButtons[i].Visible = false;
                copyButtons[i].Visible = false;
            }
        }
    }
}