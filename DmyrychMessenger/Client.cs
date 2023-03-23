using System;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;


namespace DmyrychMessenger
{
    public class Client
    {
        private Socket clientSocket;
        
        public void Connect(string ipAddress, int port)
        {
            try
            {
                //Creating socket
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //connecting to server using created socket
                clientSocket.Connect(ipAddress, port);
            } catch(Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }

        }
        public void Send(string message)
        {
            try
            {
                //відправляємо повідомлення на сервер
                byte[] data = Encoding.Unicode.GetBytes(message);
                clientSocket.Send(data);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Exception: {e}");
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

        //Client client = new Client();
        //client.Connect("127.0.0.1", 8888);
        //client.Send("Hello, server!");
        //client.Close();
        //it is only an example code of usage of this class
    }
}
