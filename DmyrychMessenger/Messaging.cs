using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmyrychMessenger
{
    class Messaging
    {
        async public Task Run()
        {
            int port = Container.getPort();
            Client client = new Client();
            Console.WriteLine("Enter '1' to start a new conversation or '2' to join an existing one:");
            var choice = Console.ReadLine();

            while (true)
            {
                if (choice == "1")
                {
                    Console.WriteLine($"Listening on port {port}");

                    client.WaitForConnection(port);

                    Console.WriteLine("Connected!");

                    Receive(client);

                    Send(client);

                    
                    client.Close();
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Enter IP address to connect to:");
                    var ipAddress = Console.ReadLine();

                    client.Connect(ipAddress, port);

                    Console.WriteLine($"Connected to {ipAddress}:{port}");

                    Receive(client);

                    Send(client);

                    client.Close();
                }
                else
                {
                    Console.WriteLine("Invalid choice!");
                }
                Console.WriteLine("Enter '1' to start a new conversation or '2' to join an existing one:");
                choice = Console.ReadLine();
            }
        }
        public void Receive(Client client)
        {
            var receiveTask = Task.Run(async () =>
            {
                while (true)
                {
                    var receivedMessage = await client.ReceiveAsync();
                    if (receivedMessage == null)
                    {
                        Console.WriteLine("Connection lost");
                        break;
                    }
                    Console.MoveBufferArea(0, Console.CursorTop, Console.BufferWidth, 1, 0, Console.CursorTop - 1);
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine($"Person: {receivedMessage}");
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                    Console.Write("You: ");
                }
            });
        }
        public void Send(Client client)
        {
            var message = "";
            while (message.ToLower() != "exit")
            {
                Console.Write("You: ");
                message = Console.ReadLine();
                if (message != "")
                {
                    client.SendAsync(message);
                    Console.WriteLine();
                }

            }
        }
    }
}
