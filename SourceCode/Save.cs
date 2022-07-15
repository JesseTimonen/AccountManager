using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace AccountManager
{
    public partial class AccountManagerForm
    {
        private bool GetData(string file)
        {
            try
            {
                // Make sure data array is empty
                accountData.RemoveRange(0, accountData.Count);

                // Read new data
                string[] lines = File.ReadAllLines(@"" + filePath + file + ".txt");

                // Password is stored to the first line of the file
                password = lines[0];

                // Saved passwords start from this line, changed to start at line 6 if settings are loaded
                int DataStartPoint = 1;

                // Settings are stored to lines 1-5, if settings are not found (older versions) skip them
                if (lines[1].ToLower() == "true" || lines[1].ToLower() == "false")
                {
                    SettingsIncludeAdditionalCheckbox.Checked = Convert.ToBoolean(lines[1]);

                    if (lines[2].ToLower() == "true" || lines[2].ToLower() == "false")
                    {
                        SettingsIncludeNumbersCheckbox.Checked = Convert.ToBoolean(lines[2]);
                    }
                    else
                    {
                        SettingsIncludeNumbersCheckbox.Checked = true;
                    }

                    if (lines[3].ToLower() == "true" || lines[3].ToLower() == "false")
                    {
                        SettingsIncludeSpecialCheckbox.Checked = Convert.ToBoolean(lines[3]);
                    }
                    else
                    {
                        SettingsIncludeSpecialCheckbox.Checked = false;
                    }

                    SettingsMinPasswordInput.Text = lines[4];
                    if (SettingsMinPasswordInput.Text != "16")
                    {
                        SettingsMinPasswordInput.ForeColor = Color.White;
                    }
                    SettingsMinPasswordInput_Leave(null, null);

                    SettingsMaxPasswordInput.Text = lines[5];
                    if (SettingsMaxPasswordInput.Text != "24")
                    {
                        SettingsMaxPasswordInput.ForeColor = Color.White;
                    }
                    SettingsMaxPasswordInput_Leave(null, null);

                    DataStartPoint = 6;
                }

                // Get rest of the data
                for (int i = DataStartPoint; i < lines.Length; i++)
                {
                    if ((i + 4) % 5 == 0)
                    {
                        try
                        {
                            accountData.Add(new KeyValuePair<string, string[]>(
                                lines[i],
                                new string[] {
                                    lines[i + 1],
                                    lines[i + 2],
                                    lines[i + 3],
                                    lines[i + 4]
                                }
                            ));
                        }
                        catch (Exception)
                        {
                            try
                            {
                                accountData.Add(new KeyValuePair<string, string[]>(
                                    lines[i],
                                    new string[] {
                                        lines[i + 1],
                                        lines[i + 2],
                                        lines[i + 3],
                                        "Corrupted"
                                    }
                                ));
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    accountData.Add(new KeyValuePair<string, string[]>(
                                        lines[i],
                                        new string[] {
                                            lines[i + 1],
                                            lines[i + 2],
                                            "Corrupted",
                                            "Corrupted"
                                        }
                                    ));
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        accountData.Add(new KeyValuePair<string, string[]>(
                                            lines[i],
                                            new string[] {
                                                lines[i + 1],
                                                "Corrupted",
                                                "Corrupted",
                                                "Corrupted"
                                            }
                                        ));
                                    }
                                    catch (Exception)
                                    {
                                        accountData.Add(new KeyValuePair<string, string[]>(
                                            lines[i],
                                            new string[] {
                                                "Corrupted",
                                                "Corrupted",
                                                "Corrupted",
                                                "Corrupted"
                                            }
                                        ));
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool SaveData()
        {
            try
            {
                using (FileStream fs = new FileStream(@"" + filePath + username + ".txt", FileMode.Create, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(password);
                    sw.Write(Environment.NewLine + SettingsIncludeAdditionalCheckbox.Checked.ToString());
                    sw.Write(Environment.NewLine + SettingsIncludeNumbersCheckbox.Checked.ToString());
                    sw.Write(Environment.NewLine + SettingsIncludeSpecialCheckbox.Checked.ToString());
                    sw.Write(Environment.NewLine + SettingsMinPasswordInput.Text);
                    sw.Write(Environment.NewLine + SettingsMaxPasswordInput.Text);

                    if (accountData.Any())
                    {
                        foreach (KeyValuePair<string, string[]> data in accountData)
                        {
                            sw.Write(Environment.NewLine + data.Key);
                            sw.Write(Environment.NewLine + data.Value[0]);
                            sw.Write(Environment.NewLine + data.Value[1]);
                            sw.Write(Environment.NewLine + data.Value[2]);
                            sw.Write(Environment.NewLine + data.Value[3]);
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private void deleteFile(string fileName)
        {
            if (File.Exists(@"" + filePath + fileName + ".txt"))
            {
                File.Delete(@"" + filePath + fileName + ".txt");
            }
        }

        private void ClearData()
        {
            // Clear user data
            accountData.RemoveRange(0, accountData.Count);
            username = null;
            password = null;

            // Crear UI elements
            if (titleLabels.Any())
            {
                ClearUI();
            }
        }
    }
}