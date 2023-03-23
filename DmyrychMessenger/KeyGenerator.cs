using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Sockets;

namespace DmyrychMessenger
{
    //статичний клас для генерації ключів
    static class KeyGenerator
    {
        //цей метод генерує та повертає пару ключів для асиметричного шифрування
        public static string[] GenerateKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);
            string[] arr = new string[] { publicKey, privateKey };
            return arr;

        }
        //цей метод повертає ключ для симетричного шифрування
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
