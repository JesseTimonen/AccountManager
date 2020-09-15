using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AccountManager
{
    public partial class AccountManagerForm
    {
        private void GetData(string file)
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
                if ((i + 2) % 3 == 0)
                {
                    try
                    {
                        accountData.Add(new KeyValuePair<string, string[]>(lines[i], new string[] { lines[i + 1], lines[i + 2] }));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            accountData.Add(new KeyValuePair<string, string[]>(lines[i], new string[] { lines[i + 1], "Corrupted" }));
                        }
                        catch (Exception)
                        {
                            accountData.Add(new KeyValuePair<string, string[]>(lines[i], new string[] { "Corrupted", "Corrupted" }));
                        }
                    }
                }
            }
        }

        private void SaveData()
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
                    }
                }
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