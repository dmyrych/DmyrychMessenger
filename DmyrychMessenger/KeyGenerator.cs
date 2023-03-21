using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DmyrychMessenger
{
    static class KeyGenerator
    {
        public static string[] GenerateKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);
            string[] arr = new string[] { publicKey, privateKey };
            return arr;

        }
        public static byte[] GenerateSessionKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                return aes.Key;
            }
        }
    }
}
