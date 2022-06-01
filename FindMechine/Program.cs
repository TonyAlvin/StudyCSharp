using System;
using System.Text.RegularExpressions;

namespace FindMechine
{
    class Program
    {
        static void Main(string[] args)
        {
            string input_line;
            Console.Write("Hi, input your name please:");
            string usr_name = Console.ReadLine();   // 先获取名字
            Regex pattern = new Regex("([H,h]ello )world");
            System.IO.TextReader txt_file_input = new System.IO.StreamReader("../input.txt"); // 读取文件
            System.IO.StreamWriter txt_file_output = new System.IO.StreamWriter("../output.txt");
            while ((input_line = txt_file_input.ReadLine()) != null)  // 按行处理
            {
                txt_file_output.WriteLine(pattern.Replace(input_line, "$1" + usr_name));
            }
            txt_file_output.Close();
            // string input_str;   // usr input
            // Regex pattern = new Regex("([H,h]ello )world"); // 根据用例与介绍，H不区分大小写，其余不知道
            // Console.Write("Hi, input your name please:");
            // string usr_name = Console.ReadLine();   // 先获取名字
            // while (true)
            // {
            //     Console.Write("input a string now:");
            //     input_str = Console.ReadLine(); // get input string
            //     Console.WriteLine(pattern.Replace(input_str, "$1" + usr_name));
            // }
        }
    }
}
