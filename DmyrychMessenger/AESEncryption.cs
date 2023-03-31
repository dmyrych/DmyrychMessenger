using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AESEncryption
{
    // Шифрує текстове повідомлення з використанням симетричного ключа
    public static string EncryptMessage(string message, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV(); // генеруємо вектор ініціалізації
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(message);
                    }
                    byte[] encryptedMessage = ms.ToArray();
                    byte[] combinedIvCt = new byte[aes.IV.Length + encryptedMessage.Length];
                    Array.Copy(aes.IV, 0, combinedIvCt, 0, aes.IV.Length);
                    Array.Copy(encryptedMessage, 0, combinedIvCt, aes.IV.Length, encryptedMessage.Length);
                    return Convert.ToBase64String(combinedIvCt);
                }
            }
        }
    }

    // Розшифровує зашифроване повідомлення з використанням симетричного ключа
    public static string DecryptMessage(string encryptedMessage, byte[] key)
    {
        byte[] combinedIvCt = Convert.FromBase64String(encryptedMessage);
        using (Aes aes = Aes.Create())
        {
            int ivLength = aes.IV.Length;
            byte[] iv = new byte[ivLength];
            byte[] encryptedData = new byte[combinedIvCt.Length - ivLength];
            Array.Copy(combinedIvCt, iv, ivLength);
            Array.Copy(combinedIvCt, ivLength, encryptedData, 0, encryptedData.Length);
            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
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
