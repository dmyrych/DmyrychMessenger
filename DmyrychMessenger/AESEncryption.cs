using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AESEncryption
{
    // Шифрує текстове повідомлення з використанням симетричного ключа
    public static string EncryptMessage(string message, string key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.Unicode.GetBytes(key);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(message);
                    }
                    byte[] encryptedMessage = ms.ToArray();
                    return Convert.ToBase64String(encryptedMessage);
                }
            }
        }
    }

    // Розшифровує зашифроване повідомлення з використанням симетричного ключа
    public static string DecryptMessage(string encryptedMessage, string key)
    {
        byte[] encryptedData = Convert.FromBase64String(encryptedMessage);
        using (Aes aes = Aes.Create())
        {
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.Unicode.GetBytes(key);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}