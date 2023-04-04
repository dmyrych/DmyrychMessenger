﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace DmyrychMessenger
{
    //статичний клас для генерації ключів
    class AsymmetricPair
    {
        public string publicKey;
        public string privateKey;

        public AsymmetricPair(string open, string closed)
        {
            publicKey = open;
            privateKey = closed;
        }

    }
    static class KeyGenerator
    {
        //цей метод генерує та повертає пару ключів для асиметричного шифрування
        public static AsymmetricPair GenerateRSAKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.KeySize = 2048;
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);
            AsymmetricPair pair = new AsymmetricPair(publicKey, privateKey);
            return pair;
        }
        //цей метод повертає ключ для симетричного шифрування
        public static string GenerateSessionKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.BlockSize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.Zeros;
                aes.KeySize = 256;
                aes.GenerateKey();
                string strKey = Encoding.Unicode.GetString(aes.Key);
                return strKey;
            }
        }
    }
}
