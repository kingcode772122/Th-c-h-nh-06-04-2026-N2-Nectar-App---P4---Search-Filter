using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    static void Main()
    {
        Socket server = new Socket(AddressFamily.InterNetwork,
                                   SocketType.Stream,
                                   ProtocolType.Tcp);

        server.Bind(new IPEndPoint(IPAddress.Any, 5000));
        server.Listen(5);

        Console.WriteLine("Server đang chờ kết nối...");

        Socket client = server.Accept();

        Console.WriteLine("Client đã kết nối!");

        // Thread nhận tin
        Thread receiveThread = new Thread(() =>
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytes = client.Receive(buffer);

                if (bytes == 0) break;

                string msg = Encoding.UTF8.GetString(buffer, 0, bytes);

                Console.WriteLine("Client: " + msg);
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