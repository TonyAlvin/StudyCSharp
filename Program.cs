using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FileTrans
{
    class SocketConnection
    {
        public IPAddress ips;       // ip地址
        public IPEndPoint ipnode;
        public int socketport;      // 可连接数量
        public bool isclient { get; }   // 服务器还是客户端
        private Socket socket_commu;    // 数据传输
        private Socket socket_ser_listener; // 监听

        public SocketConnection()
        {
            socketport = 80;    // 80端口
            ConsoleKey key_input;
            string ipinput = null;
            while (true)
            {
                try
                {
                    while (true)
                    {
                        Console.Write("press \"1\" for server, \"2\" for client:");
                        key_input = Console.ReadKey(false).Key;
                        Console.Write("\n");
                        if (key_input == ConsoleKey.D1)
                        {
                            isclient = false;
                            break;
                        }
                        else if (key_input == ConsoleKey.D2)
                        {
                            isclient = true;
                            break;
                        }
                    }
                    Console.Write("input IP Address:");
                    ipinput = Console.ReadLine();
                    ips = IPAddress.Parse(ipinput);
                    ipnode = new IPEndPoint(ips, socketport);
                    if (isclient)
                        socket_commu = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    else
                        socket_ser_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    break;
                }
                catch
                {
                    Console.WriteLine("Error input. again:");
                }
            }
        }
        public void connect(int clientnum)
        {
            if (isclient)
            {
                socket_commu.Connect(ipnode);
            }
            else
            {
                Console.WriteLine("bingin connection.");
                socket_ser_listener.Bind(ipnode);   // 开始监听
                socket_ser_listener.Listen(clientnum);
                socket_commu = socket_ser_listener.Accept();
            }
        }
        public void disconnect()
        {
            try
            {
                socket_commu.Disconnect(true);
                socket_ser_listener.Disconnect(true);
            }
            catch
            {

            }
        }
        public void send(byte[] data)
        {
            socket_commu.Send(data);
        }
        public int recv(byte[] buffer)
        {
            try
            {
                return socket_commu.Receive(buffer);
            }
            catch
            {
                return -1;
            }
        }
    }

    struct DataRecv
    {
        public byte[] buffer;
        public int length;
    };
    class Program
    {
        static SocketConnection connection1;
        static FileStream f;
        static BinaryWriter bf;
        private static ActionBlock<DataRecv> fileWriteBlock = new ActionBlock<DataRecv>((input) =>
        {
            if (input.length >= 0)
            {
                byte[] writeTemp = new byte[input.length];
                writeTemp = input.buffer;
                Console.WriteLine("writing file.");
                bf.Write(writeTemp);
            }
            else
            {
                bf.Close();
                f.Close();
            }
        });

        private static TransformBlock<byte[], byte[]> fileReadBlock = new TransformBlock<byte[], byte[]>(p => p);
        private static TransformBlock<byte[], DataRecv> socketRecBlock = new TransformBlock<byte[], DataRecv>(buffer =>
        {
            DataRecv rec = new DataRecv { };
            rec.length = connection1.recv(buffer);
            rec.buffer = buffer;
            return rec;
        });

        private static ActionBlock<byte[]> socketTransBlock = new ActionBlock<byte[]>((input) =>
        {
            connection1.send(input);
        });

        static void Main(string[] args)
        {
            fileReadBlock.LinkTo(socketTransBlock);
            socketRecBlock.LinkTo(fileWriteBlock);
            string file_name = "";

            connection1 = new SocketConnection();
            connection1.connect(1); // 建立连接

            while (true)
            {
                Console.Write("Input the file name:");
                file_name = "./" + Console.ReadLine();
                try
                {
                    FileStream f = new FileStream(file_name, FileMode.Create, FileAccess.ReadWrite);
                    break;
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine("file open fieled.");
                }
            }
            Task.Run(() =>
            {
                if (connection1.isclient)
                {
                    bf = new BinaryWriter(f);
                    while (true)
                    {
                        try
                        {
                        byte[] recbuf = new byte[200];
                        socketRecBlock.Post(recbuf);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                else
                {
                    BinaryReader binInput = new BinaryReader(f);
                    while (true)
                    {
                        try
                        {
                            byte[] buffer = binInput.ReadBytes(200);
                            fileReadBlock.Post(buffer);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            });
            Console.WriteLine("trans finished.");
            Console.ReadKey(true);
        }

    }
}
