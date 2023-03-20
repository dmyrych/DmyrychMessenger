using System;
using System.Text;
using System.Security.Cryptography;
using DmyrychMessenger;

public class RSAMessendger
{
    

    public static void Main()
    {
        RSAEncryption rsa = new

        

        //Console.WriteLine($"Original Message: {message}");
        //Console.WriteLine($"Encrypted Message: {encryptedMessage}");
        //Console.WriteLine($"Decrypted Message: {decryptedMessage}");
    }
    public static void GenerateKeys()
    {
        // Encrypt and decrypt a message using the keys
        string message = "hello, world!";
        string encryptedmessage = encrypt(message, publickey);
        string decryptedmessage = decrypt(encryptedmessage, privatekey);
    }
}