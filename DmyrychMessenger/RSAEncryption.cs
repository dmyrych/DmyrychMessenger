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
    //відкритий передається на сервер і може бути використаний будь-яким користувачем для зашифрування повідомлення
    //в моїй реалізації, асиметричне шифрування використовуватиметься для обміну симетричним ключем сесії між користувачами
    //для безпосередньо шифрування повідомлень буде використано AES
    //розшифрування повідомлення можливе лише за допомогою закритого ключа, відповідного відкритому
    //за допомогою якого було зашифроване повідомлення
    static class RSAEncryption
    {
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        //Метод для зашифровування повідомлення відкритим(публічним) ключем
        public static string Encrypt(string originalMessage, string publicKey)
        {
            byte[] bytesToEncrypt = _encoder.GetBytes(originalMessage);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.KeySize = Container.getRSAKeyLength();
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
                rsa.KeySize = Container.getRSAKeyLength();
                rsa.FromXmlString(privateKey);

                byte[] decryptedBytes = rsa.Decrypt(bytesToDecrypt, false);

                return _encoder.GetString(decryptedBytes);
            }
        }
    }
}
