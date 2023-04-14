using System;
using System.Net.Sockets;

namespace DmyrychMessenger
{
    class Program
    {
        //програма для консольного зашифрованого текстового листування на peer-to-peer основі
        //консольний клієнт можна запустити як у режимі прийому вхідних з'єднань так і в режимі налагодження з'єднання
        //в першому випадку ми просто очікуємо доки на наш ip прийде запит на заздалегідь визначений порт
        //в другому, треба ввести ip співрозмовника
        //якщо треба перевірити працездатність на одному ПК, можна запустити додаток двічі, при чому перший запустити у режимі прийому а другий
        // - в режимі з'єднання. ip вказати 127.0.0.1 
        //з'єднання між кількома ПК - вводитити ip співрозмовника.

        //програма використовує асиметричне шифрування для передачі симетричного ключа сесії в зашифрованому вигляді, аби його не можна було
        //перехопити
        static async Task Main(string[] args)
        {
            int port = 45675;
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

                    var message = "";
                    while (message.ToLower() != "exit")
                    {
                        Console.Write("You: ");
                        message = Console.ReadLine();
                        if (message != "")
                        {
                            await client.SendAsync(message);
                            Console.WriteLine();
                        }

                    }

                    await receiveTask;
                    client.Close();
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Enter IP address to connect to:");
                    var ipAddress = Console.ReadLine();

                    client.Connect(ipAddress, port);

                    Console.WriteLine($"Connected to {ipAddress}:{port}");

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
                            Console.WriteLine($"Server: {receivedMessage}");
                            Console.SetCursorPosition(0, Console.CursorTop + 1);
                            Console.Write("You: ");
                        }
                    });

                    var message = "";
                    while (message.ToLower() != "exit")
                    {
                        Console.Write("You: ");
                        message = Console.ReadLine();
                        if (message != "")
                        {
                            await client.SendAsync(message);
                            Console.WriteLine();
                        }

                    }

                    await receiveTask;
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
    }
}