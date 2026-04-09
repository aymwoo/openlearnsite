using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LearnSite.Common
{
    public class LinkEncryption
    {
        private static readonly string EncryptionKey = "LearnSiteLink2024SecretKey!";
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("LinkSalt2024");
        private static string _currentFid = "";
        
        public static string EncryptUrl(string url, string sessionId)
        {
            if (string.IsNullOrEmpty(url))
                return "";
            
            string dataToEncrypt = url + "|" + sessionId + "|" + DateTime.Now.Ticks;
            
            return Encrypt(dataToEncrypt);
        }
        
        public static string DecryptUrl(string encryptedUrl, string sessionId)
        {
            if (string.IsNullOrEmpty(encryptedUrl))
                return "";
            
            try
            {
                string decrypted = Decrypt(encryptedUrl);
                
                string[] parts = decrypted.Split('|');
                if (parts.Length >= 3)
                {
                    string originalUrl = parts[0];
                    string originalSessionId = parts[1];
                    long timestamp = long.Parse(parts[2]);
                    
                    if (originalSessionId != sessionId)
                    {
                        return "";
                    }
                    
                    DateTime encryptTime = new DateTime(timestamp);
                    TimeSpan elapsed = DateTime.Now - encryptTime;
                    if (elapsed.TotalMinutes > 30)
                    {
                        return "";
                    }
                    
                    return originalUrl;
                }
                
                return "";
            }
            catch
            {
                return "";
            }
        }
        
        private static string Encrypt(string plainText)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(EncryptionKey, Salt, 1000);
                    aesAlg.Key = keyDerivation.GetBytes(32);
                    aesAlg.IV = keyDerivation.GetBytes(16);
                    
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            byte[] encrypted = msEncrypt.ToArray();
                            return Convert.ToBase64String(encrypted).Replace("+", "-").Replace("/", "_").Replace("=", "");
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        
        private static string Decrypt(string cipherText)
        {
            try
            {
                cipherText = cipherText.Replace("-", "+").Replace("_", "/");
                int padding = 4 - (cipherText.Length % 4);
                if (padding != 4)
                {
                    cipherText += new string('=', padding);
                }
                
                byte[] buffer = Convert.FromBase64String(cipherText);
                
                using (Aes aesAlg = Aes.Create())
                {
                    Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(EncryptionKey, Salt, 1000);
                    aesAlg.Key = keyDerivation.GetBytes(32);
                    aesAlg.IV = keyDerivation.GetBytes(16);
                    
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    
                    using (MemoryStream msDecrypt = new MemoryStream(buffer))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        
        public static string ProcessContentLinks(string content, string fid)
        {
            if (string.IsNullOrEmpty(content))
                return content;
            
            _currentFid = fid;
            
            System.Text.RegularExpressions.Regex linkRegex = new System.Text.RegularExpressions.Regex(
                @"<a\s+[^>]*href\s*=\s*[""']([^""']+)[""'][^>]*>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            
            string result = linkRegex.Replace(content, new System.Text.RegularExpressions.MatchEvaluator(LinkMatchEvaluator));
            
            return result;
        }
        
        private static string LinkMatchEvaluator(System.Text.RegularExpressions.Match match)
        {
            string fullMatch = match.Value;
            string href = match.Groups[1].Value;
            
            if (href.StartsWith("http://") || href.StartsWith("https://") || href.StartsWith("/") || href.StartsWith("~/"))
            {
                if (href.StartsWith("~/"))
                {
                    href = href.Substring(2);
                }
                
                string dataUrl = HttpUtility.HtmlEncode(href);
                string newLink = fullMatch.Replace(match.Groups[1].Value, "javascript:void(0);");
                
                if (!newLink.Contains("onclick"))
                {
                    newLink = newLink.Replace(">", " data-link=\"" + dataUrl + "\" data-fid=\"" + _currentFid + "\" onclick=\"openSecureLink(this);\">");
                }
                
                return newLink;
            }
            
            return fullMatch;
        }
    }
}
