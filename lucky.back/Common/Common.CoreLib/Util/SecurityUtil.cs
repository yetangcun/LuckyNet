using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        #region MD5加密
        #endregion
    }
}
