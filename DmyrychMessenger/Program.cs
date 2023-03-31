using System;
using System.Text;
using System.Security.Cryptography;
using DmyrychMessenger;

public class RSAMessendger
{
    public static void Main() 
    {
        Client client = new Client();
        client.Connect("127.0.0.1", 29875);
        client.Register("Georgiy", "29875"); 
        string message;
        while (true)
        {
            message = Console.ReadLine();
            client.Send(message);
        }

        Console.ReadLine(); 
        client.Close();

    }

}