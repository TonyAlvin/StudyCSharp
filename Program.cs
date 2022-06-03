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
        public IPAddress ips;
        public IPEndPoint ipnode;
        public int socketport;
        public bool isclient { get; }
        private Socket socket_commu;
        private Socket socket_ser_listener;

        public SocketConnection()
        {
            socketport = 80;
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
            return socket_commu.Receive(buffer);
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
        private static ActionBlock<DataRecv> fileWriteBlock = new ActionBlock<DataRecv>((input) =>
        {
            using (FileStream f = new FileStream("./output.docx", FileMode.Create, FileAccess.ReadWrite))
            {
                byte[] writeTemp = new byte[input.length];
                writeTemp = input.buffer;
                BinaryWriter bf = new BinaryWriter(f);
                Console.WriteLine("writing file.");
                bf.Write(writeTemp);
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

            connection1 = new SocketConnection();
            connection1.connect(1); // 建立连接

            if (connection1.isclient)
            {
                Task.Run(() =>
                {
                    byte[] recbuf = new byte[200];
                    socketRecBlock.Post(recbuf);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    FileStream input = new FileStream("./input.docx", FileMode.Open, FileAccess.Read);
                    BinaryReader binInput = new BinaryReader(input);
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
                });
            }

            Console.WriteLine("trans finished.");
            Console.ReadKey(true);
        }

    }
}
