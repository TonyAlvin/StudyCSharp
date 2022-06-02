using System;
using System.Threading.Tasks.Dataflow;
using System.IO;
using System.Threading.Tasks;

namespace TPL
{
    class MyProgram
    {
        //定义一个 文本处理流程
        private static TransformBlock<string, string> transformBlock = new TransformBlock<string, string>((input) =>
        {
            return input.Replace("Hello world", "Hello legen");
        });

        // 定义一个命令行输出流程
        private static ActionBlock<string> consoleBlock = new ActionBlock<string>((input) =>
        {
            Console.WriteLine(input);
        });

        // 定义一个文件输出流程
        private static ActionBlock<string> fileBlock = new ActionBlock<string>((input) =>
        {
            using (StreamWriter f = new StreamWriter("./output.txt", true))
            {
                f.WriteLine(input);
            }
        });
        // 多播流程
        private static BroadcastBlock<string> broadcastBlock = new BroadcastBlock<string>(p => p);
        static void Main()
        {
            Console.WriteLine("begin process...");
            // 连接流程
            transformBlock.LinkTo(broadcastBlock);
            broadcastBlock.LinkTo(consoleBlock);
            broadcastBlock.LinkTo(fileBlock);

            Task.Run(() =>
            {
                string line = null;
                using (StreamReader f = new StreamReader("./input.txt"))
                {
                    while ((line = f.ReadLine()) != null)
                    {
                        transformBlock.Post(line);
                    }
                }
            });

            Console.ReadKey(true);
        }
    }
}