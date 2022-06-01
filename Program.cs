using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace findMechine2
{
    class MultiThreadingApp
    {
        private static ConcurrentQueue<string> outputQueue = new ConcurrentQueue<string>();// 声明输出队列
        private static ConcurrentQueue<string> inputQueue = new ConcurrentQueue<string>();// 输入队列
        static void replace_line()
        {
            while (true)
            {
                if (inputQueue.TryDequeue(out string newline))
                {
                    Console.WriteLine("replacing!");
                    var result = newline.Replace("Hello world", "Hello legen");
                    outputQueue.Enqueue(result);
                }
            }
        }
        static void dispay_line()
        {
            while (true)
            {
                if (outputQueue.TryDequeue(out string newline))
                {
                    Console.WriteLine("displaying!");
                    Console.WriteLine(newline);
                }
            }
        }
        static void read_line()
        {
            string line = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader("./input.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine("inputing line.");
                    inputQueue.Enqueue(line);
                    //newLineArrived?.Invoke(line);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Begin Process...");
            Task.Run(() => { read_line(); });   // 开启一个线程去从文件读取数据，放到输入队列里
            Task.Run(() => { replace_line(); });// 开启一个线程去从输入队列中取数据，处理完成后，放到输出队列里
            Task.Run(() => { dispay_line(); }); // 开启一个线程从输出队列中取数据，取到后放到显示出来
            Console.ReadKey(true);              // 等待按键以结束程序，否则主线程结束后，创建的几个线程会被结束。
        }
    }
}