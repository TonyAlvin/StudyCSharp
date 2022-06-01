using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace findMechine2
{
    class MultiThreadingApp
    {
        private static ConcurrentQueue<string> outputQueue = new ConcurrentQueue<string>();// 声明输出队列
        private static ConcurrentQueue<string> inputQueue = new ConcurrentQueue<string>();// 输入队列
        private static Action ReadFinished = null;             // 读取完成的动作
        private static Action<string> ReadLine = null;         // 读取动作
        private static Action<string> ProcessedLine = null;     // 操作结束动作
        static void Main(string[] args)
        {
            Console.WriteLine("Begin Process...");

            ReadFinished += () => { Console.WriteLine("读取完成后，我需要进行处理"); };
            ReadLine += (input) => { inputQueue.Enqueue(input); };
            ProcessedLine += (str) => { Console.WriteLine("hi" + str); };
            ProcessedLine += (str) => { ; };    // 可以添加写文件的程序

            //开启一个线程去读取文件
            Task.Run(() =>
            {
                string line = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader("./input.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        ReadLine?.Invoke(line);
                    }
                    ReadFinished?.Invoke();
                }
            });

            //开启一个线程去从输入队列中取数据，处理完成后，放到输出队列里
            Task.Run(() =>
            {
                while (true)
                {
                    if (inputQueue.TryDequeue(out string newline))
                    {
                        var result = newline.Replace("Hello world", "Hello legen");
                        ProcessedLine?.Invoke(result);
                    }
                }
            });
            Console.WriteLine("Any key to exit...");
            Console.ReadKey(true);              // 等待按键以结束程序，否则主线程结束后，创建的几个线程会被结束。
        }
    }
}

