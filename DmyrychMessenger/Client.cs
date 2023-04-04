using System;
using System.Net.Sockets;
using System.Text;

namespace DmyrychMessenger
{
    public class Client
    {
        private Socket clientSocket;
        private byte[] keyExchangePublicKey;
        private string sessionKey;

        public void Connect(string ipAddress = null, int port = 0)
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                try
                {
                    //Creating socket
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    //connecting to server using created socket
                    clientSocket.Connect(ipAddress, port);

                    // Generate RSA Key Pair and exchange public key with the companion
                    AsymmetricPair rsaKeys = KeyGenerator.GenerateRSAKeys();
                    string publicKey = rsaKeys.publicKey;
                    SendPublicKey(publicKey);


                    string encryptedSessionKey = ReceiveSessionKey();
                    sessionKey = RSAEncryption.Decrypt(encryptedSessionKey, rsaKeys.privateKey);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }
            else
            {
                WaitForConnection(port);
            }
        }

        public void Send(string message)
        {
            try
            {
                // Encrypt the message using the session key and send it to companion
                var encryptedMessage = AESEncryption.EncryptMessage(message, sessionKey);
                byte[] data = Encoding.Unicode.GetBytes(encryptedMessage);
                clientSocket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
        // Sending session key
        public void SendSessionKey(string cryptedSessionKey)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(cryptedSessionKey);
                clientSocket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
        //recieving Session Key
        public string ReceiveSessionKey()
        {
            try
            {
                // Receive session key
                var buffer = new byte[512];
                var bytesRead = clientSocket.Receive(buffer);
                var message = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return null;
            }
        }
        //use this method to send public key
        public void SendPublicKey(string publicKey)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(publicKey);
                clientSocket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }

        //use this method to recieve public key
        public string ReceivePublicKey()
        {
            try
            {
                var buffer = new byte[2048];
                var bytesRead = clientSocket.Receive(buffer);
                var publicKey = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                return publicKey;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return null;
            }
        }

        public string Receive()
        {
            try
            {
                // Receive the message from the server and decrypt it using the session key
                var buffer = new byte[4096];
                var bytesRead = clientSocket.Receive(buffer);
                var encryptedMessage = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                var message = AESEncryption.DecryptMessage(encryptedMessage, sessionKey);
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return null;
            }
        }

        public void Close()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
        public string Encrypt(string message, string key)
        {
            return AESEncryption.EncryptMessage(message, key);
        }

        public string Decrypt(string encryptedMessage, string key)
        {
            return AESEncryption.DecryptMessage(encryptedMessage, key);
        }
        public void WaitForConnection(int port)
        {
            try
            {
                //Creating socket
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //Binding socket to the specified port
                clientSocket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, port));

                //Listening to incoming connections
                clientSocket.Listen(10);

                Console.WriteLine($"Waiting for connection on port {port}...");

                //Accepting the incoming connection
                clientSocket = clientSocket.Accept();

                // Wait for public key of the companion
                var buffer = new byte[2048];
                var bytesRead = clientSocket.Receive(buffer);
                var companionKey = Encoding.Unicode.GetString(buffer, 0, bytesRead);

                // Decrypt the client public key and generate the session key
                sessionKey = KeyGenerator.GenerateSessionKey();
                var encryptedSessionKey = RSAEncryption.Encrypt(sessionKey, companionKey);
                SendSessionKey(encryptedSessionKey);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
    }
}
