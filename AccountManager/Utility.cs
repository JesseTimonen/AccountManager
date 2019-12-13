using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AccountManager
{
    public partial class AccountManagerForm
    {
        private bool validateUsername(string username)
        {
            if (Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                return true;
            }

            return false;
        }

        private bool validatePassword(string password, string confirmationPassword)
        {
            if (password.Length < 8)
            {
                return false;
            }

            if (password == password.ToLower() || password == password.ToUpper())
            {
                return false;
            }

            if (password != confirmationPassword)
            {
                return false;
            }

            return true;
        }

        private string CreateRandomString(int minLenght, int maxLenght)
        {
            Random random = new Random();
            int length = random.Next(minLenght, maxLenght + 1);

            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_*=/<>#)]";
            StringBuilder randomString = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    randomString.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return randomString.ToString();
        }
    }
}