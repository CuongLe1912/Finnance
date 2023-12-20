using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace URF.Core.Helper.Helpers
{
    public class SecurityHelper
    {
        public static string GenerateOtp()
        {
            var iOTPLength = 6;
            var sOTP = string.Empty;
            var random = new Random();
            var saAllowedCharacters = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            for (var i = 0; i < iOTPLength; i++)
                sOTP += saAllowedCharacters[random.Next(0, saAllowedCharacters.Length)];
            return sOTP;
        }

        public static string Generate8Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return string.Format("{0:D8}", random);
        }

        public static string CreateHashMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("X2"));
            return sb.ToString().ToLower();
        }

        public static string GenerateVerifyCode(int max = 6)
        {
            var str = string.Empty;
            var random = new Random();
            var chArray = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < max; i++)
            {
                int index = random.Next(1, chArray.Length);
                if (!str.Contains(chArray.GetValue(index).ToString()))
                {
                    str = str + chArray.GetValue(index);
                }
                else
                {
                    i--;
                }
            }
            return str;
        }

        public static string CreateHash256(string message, string secret)
        {
            secret ??= "";
            var encoding = new ASCIIEncoding();
            byte[] secretBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(secretBytes);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage);
        }

        public static string GenerateUUID()
        {
            Guid myuuid = Guid.NewGuid();
            return myuuid.ToString();
        }
    }
}
