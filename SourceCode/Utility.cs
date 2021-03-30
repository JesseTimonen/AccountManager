using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AccountManager
{
    public partial class AccountManagerForm
    {
        private bool validateUsername(string username)
        {
            return Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$");
        }


        private bool validatePassword(string password, string confirmationPassword)
        {
            if (password.Length < 8 || password == password.ToLower() || password == password.ToUpper() || password != confirmationPassword)
            {
                return false;
            }

            return true;
        }


        private string CreateRandomString(int minLenght = 15, int maxLenght = 25)
        {
            int length = random.Next(minLenght, maxLenght + 1);
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!?@$€-.,~'*=/#&%<>{}()[]";
            stringBuilder.Clear();

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    stringBuilder.Append(validCharacters[(int)(num % (uint)validCharacters.Length)]);
                }
            }

            return stringBuilder.ToString();
        }
    }
}