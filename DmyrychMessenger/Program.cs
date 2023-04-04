using System;

namespace DmyrychMessenger
{
    class Program
    {
        static void Main(string[] args)
        {

            int port = 45675;
            Client client = new Client();
            Console.WriteLine("Enter '1' to start a new conversation or '2' to join an existing one:");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine($"Listening on port {port}");

                client.WaitForConnection(port);

                Console.WriteLine("Connected!");

                var message = "";
                while (message.ToLower() != "exit")
                {
                    Console.Write("You: ");
                    message = Console.ReadLine();
                    client.Send(message);
                    if (message.ToLower() != "exit")
                    {
                        var receivedMessage = client.Receive();
                        Console.WriteLine($"Client: {receivedMessage}");
                    }
                }

                client.Close();
            }
            else if (choice == "2")
            {
                Console.WriteLine("Enter IP address to connect to:");
                var ipAddress = Console.ReadLine();

                client.Connect(ipAddress, port);

                Console.WriteLine($"Connected to {ipAddress}:{port}");

                var message = "";
                while (message.ToLower() != "exit")
                {
                    var receivedMessage = client.Receive();
                    Console.WriteLine($"Server: {receivedMessage}");

                    Console.Write("You: ");
                    message = Console.ReadLine();
                    client.Send(message);
                }

                client.Close();
            }
            else
            {
                Console.WriteLine("Invalid choice!");
            }
        }

        //string original_message = "Oh, my, John Wick Chapter 4 is such a good movie!";
        //Console.WriteLine(original_message);
        //AsymmetricPair pair = KeyGenerator.GenerateRSAKeys();
        //string key = KeyGenerator.GenerateSessionKey();
        //Console.WriteLine($"key: {key}");
        //key = RSAEncryption.Encrypt(key, pair.publicKey);
        //Console.WriteLine($"key: {key}");
        //key = RSAEncryption.Decrypt(key, pair.privateKey);

        //string crypted = AESEncryption.EncryptMessage(original_message, key);
        //Console.WriteLine($"Crypted message : {crypted}");
        //original_message = AESEncryption.DecryptMessage(crypted, key);
        //Console.WriteLine($"Decrypted message: {original_message}");
    }
}