using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Client
{
    static void Main()
    {
        Socket client = new Socket(AddressFamily.InterNetwork,
                                   SocketType.Stream,
                                   ProtocolType.Tcp);

        client.Connect("127.0.0.1", 5000);

        Console.WriteLine("Đã kết nối server");

        // Thread nhận tin
        Thread receiveThread = new Thread(() =>
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytes = client.Receive(buffer);

                if (bytes == 0) break;

                string msg = Encoding.UTF8.GetString(buffer, 0, bytes);

                Console.WriteLine("Server: " + msg);
            }
        });

        receiveThread.Start();

        // gửi tin
        while (true)
        {
            string msg = Console.ReadLine();

            byte[] data = Encoding.UTF8.GetBytes(msg);

            client.Send(data);
        }
    }
}