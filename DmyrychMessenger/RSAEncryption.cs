using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DmyrychMessenger
{
    //клас реалізує практичну реалізацію шифрування алгоритмом RSA
    //в ньому генеруються пари відкритий-закритий(публічний-приватний) ключів
    //відкритий передається на сервер і може бути використаний будь-яким користувачем для ЗАшифрування повідомлення
    //розшифрування повідомлення можливе лише за допомогою закритого ключа, відповідного відкритому
    //за допомогою якого було зашифроване повідомлення
    class RSAEncryption
    {
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        RSAEncryption()
        {

        }
        public RSACryptoServiceProvider GenerateKeys()
        {

        }
        //Метод для зашифровування повідомлення відкритим(публічним) ключем
        public static string Encrypt(string originalMessage, string publicKey)
        {
            byte[] bytesToEncrypt = _encoder.GetBytes(originalMessage);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                byte[] encryptedBytes = rsa.Encrypt(bytesToEncrypt, false);

                return Convert.ToBase64String(encryptedBytes);
            }
        }
        //метод для розшифрування повідомлення закритим(приватним) ключем
        public static string Decrypt(string encryptedMessage, string privateKey)
        {
            byte[] bytesToDecrypt = Convert.FromBase64String(encryptedMessage);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);

                byte[] decryptedBytes = rsa.Decrypt(bytesToDecrypt, false);

                return _encoder.GetString(decryptedBytes);
            }
        }
        public static
    }
}
