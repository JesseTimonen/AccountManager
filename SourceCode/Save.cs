using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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

                // Get rest of the data
                for (int i = 1; i < lines.Length; i++)
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