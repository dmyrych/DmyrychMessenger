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
                    //Створюємо сокет
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    //коннектимосчя до сервера використовуючи створений сокет
                    clientSocket.Connect(ipAddress, port);

                    // Генеруємо пару ключів RSA та відправляємо публічний ключ співрозмовнику
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
                // Зашифровуємо повідомлення використовуючи ключ сесії та надсилаємо його співрозмовнику
                var encryptedMessage = AESEncryption.EncryptMessage(message, sessionKey);
                byte[] data = Encoding.Unicode.GetBytes(encryptedMessage);
                clientSocket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
        // Метод для надсилання ключа сесії в асиметрично зашифрованому вигляді
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
        //Отримання ключа сесії
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
        //метод длля надсиланя публічного ключа
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

        //метод для прийому публічного ключа
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
                // Отримання повідомлення та його розшифрування ключем сесії
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
        public void Register(string login, string password)
        {
            try
            {
                //відправляємо повідомлення на сервер
                string message = $"register: {login} {password}";
                byte[] data = Encoding.Unicode.GetBytes(message);
                clientSocket.Send(data);

                //отримуємо відповідь від сервера
                byte[] buffer = new byte[1024];
                int bytesRead = clientSocket.Receive(buffer);
                string response = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                Console.WriteLine(response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }

        public string Decrypt(string encryptedMessage, string key)
        {
            return AESEncryption.DecryptMessage(encryptedMessage, key);
        }
        public void WaitForConnection(int port)
        {
            try
            {
                //Cтворюємо сокет
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //Прикріплюємо сокет то виділеного порту
                clientSocket.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, port));

                //Прослуховування вхідних з'єднань
                clientSocket.Listen(10);

                Console.WriteLine($"Waiting for connection on port {port}...");

                //Прийом вхідного з'єднання
                clientSocket = clientSocket.Accept();

                // Очікуємо публічний ключ
                var buffer = new byte[2048];
                var bytesRead = clientSocket.Receive(buffer);
                var companionKey = Encoding.Unicode.GetString(buffer, 0, bytesRead);

                // Генерація ключа сесії
                sessionKey = KeyGenerator.GenerateSessionKey();
                var encryptedSessionKey = RSAEncryption.Encrypt(sessionKey, companionKey);
                SendSessionKey(encryptedSessionKey);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
        public async Task SendAsync(string message)
        {
            try
            {
                // Зашифрувати повідомлення використовуючи ключ сесії та надіслати його співрозмовнику
                var encryptedMessage = AESEncryption.EncryptMessage(message, sessionKey);
                byte[] data = Encoding.Unicode.GetBytes(encryptedMessage);
                await clientSocket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }

        public async Task<string> ReceiveAsync()
        {
            try
            {
                // Отримання повідомлення та розшифрувати його використовуючи ключ сесії
                var buffer = new byte[4096];
                var result = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                var encryptedMessage = Encoding.Unicode.GetString(buffer, 0, result);
                var message = AESEncryption.DecryptMessage(encryptedMessage, sessionKey);
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return null;
            }
        }
    }
}
