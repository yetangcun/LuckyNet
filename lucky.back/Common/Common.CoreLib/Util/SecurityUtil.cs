using System.Text;
using System.Security.Cryptography;

namespace Common.CoreLib.Helper
{
    /// <summary>
    /// 安全工具类
    /// </summary>
    public class SecurityUtil
    {
        #region AES加密解密

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ky"></param>
        /// <param name="iv"></param>
        public static string Encrypt(string source, string ky, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                aesAlg.Key = Encoding.UTF8.GetBytes(ky);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(source);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pwdTxt"></param>
        /// <param name="ky"></param>
        /// <param name="iv"></param>
        public static string Decrypt(string pwdTxt, string ky, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                aesAlg.Key = Encoding.UTF8.GetBytes(ky);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(pwdTxt)))
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

        #endregion

        #region md5

        /// <summary>
        /// 源字符串
        /// 32位
        /// </summary>
        public static string GetMd5(string sourceStr)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(sourceStr));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 16位
        /// </summary>
        public static string Get16Md5(string sourceStr)
        {
            using (var md5 = MD5.Create())
            {
                string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sourceStr)), 4, 8);
                t2 = t2.Replace("-", "");
                return t2.ToLower();
            }
        }
        #endregion
    }
}
