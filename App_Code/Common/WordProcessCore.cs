using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LearnSite.Common
{
    public static class WordProcessCore
    {
        private static readonly Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static readonly Regex RegUsername = new Regex(@"^[A-Za-z0-9]+$");
        private static readonly Regex RegEnglish = new Regex(@"^[A-Za-z]+$");
        private static readonly Regex RegNum = new Regex(@"^[0-9]+$");
        private static readonly Regex RegCnEnNum = new Regex("^[a-zA-Z0-9_\u4e00-\u9fa5]+$");
        private static readonly Regex RegIsNum = new Regex("^-?(0|([0].[0-9]*[1-9])|([1-9]+((.[0-9]*[1-9])|([0-9]*))))$");

        private static readonly char[] AlphaNumericChars =
        {
            '0','1','2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
        };

        private static readonly char[] NumericChars =
        {
            '0','1','2','3','5','6','7','8','9'
        };

        public static bool IsCnEnNum(string strInput)
        {
            return RegCnEnNum.Match(strInput).Success;
        }

        public static bool IsNum(string strInput)
        {
            return RegNum.Match(strInput).Success;
        }

        public static bool IsIntNum(string strInput)
        {
            return RegIsNum.Match(strInput).Success;
        }

        public static bool StrLength(string strInput)
        {
            return strInput.Length <= 8;
        }

        public static bool IsEnglish(string strInput)
        {
            return RegEnglish.Match(strInput).Success;
        }

        public static bool IsZh(string strInput)
        {
            return RegCHZN.Match(strInput).Success;
        }

        public static bool IsEnNum(string strInput)
        {
            return RegUsername.Match(strInput).Success;
        }

        public static string Hash(string toHash)
        {
            using (var crypto = new MD5CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF7.GetBytes(toHash);
                bytes = crypto.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte num in bytes)
                {
                    sb.AppendFormat("{0:x2}", num);
                }

                return sb.ToString();
            }
        }

        public static string GetMd5Lower(string value)
        {
            using (var md5 = MD5.Create())
            {
                byte[] input = Encoding.UTF8.GetBytes(value);
                byte[] hash = md5.ComputeHash(input);
                StringBuilder sb = new StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static string GetMD5(string value)
        {
            return GetMd5Lower(value);
        }

        public static string GetMD5_8bit(string value)
        {
            return GetMd5Lower(value).Substring(0, 8);
        }

        public static string GetMD5_16bit(string value)
        {
            return GetMd5Lower(value).Substring(0, 16);
        }

        public static string GetMD5_Nbit(string value, int length)
        {
            return GetMd5Lower(value).Substring(0, length);
        }

        public static string StrToLower(string value)
        {
            return value.ToLower();
        }

        public static string Serv_u_Md5(string pwd)
        {
            Random ran = new Random();
            string prefix = Convert.ToChar(ran.Next(26) + 'a').ToString() + Convert.ToChar(ran.Next(26) + 'a').ToString();
            return prefix + GetMd5Lower(prefix + pwd);
        }

        public static string GenerateRandom(int length)
        {
            StringBuilder newRandom = new StringBuilder(36);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(AlphaNumericChars[rd.Next(36)]);
            }

            return newRandom.ToString();
        }

        public static string GenerateRandomNum(int length)
        {
            StringBuilder newRandom = new StringBuilder(9);
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(NumericChars[rd.Next(9)]);
            }

            return newRandom.ToString();
        }

        public static int GetRandomNum(int numMax)
        {
            if (numMax <= 0)
            {
                return 0;
            }

            Random random = new Random();
            return random.Next(numMax);
        }
    }
}
