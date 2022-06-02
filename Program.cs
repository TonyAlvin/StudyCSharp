using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace socket_sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("process begin..");
            Socket server_socketListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ips = IPAddress.Parse("172.21.0.248");
            IPEndPoint ipNode = new IPEndPoint(ips, 80); // 开80端口
            server_socketListen.Bind(ipNode);   // 开始监听
            server_socketListen.Listen(10);
            Socket socket_commu = server_socketListen.Accept();
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024];
                int num = socket_commu.Receive(buffer);
                string str = Encoding.UTF8.GetString(buffer, 0, num);
                Console.WriteLine("收到客户端数据 : " + str);
                //发送消息到客户端。
                socket_commu.Send(Encoding.UTF8.GetBytes("服务端：" + str));
            }
        }
    }
}
