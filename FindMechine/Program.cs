using System;
using System.Text.RegularExpressions;

namespace FindMechine
{
    class Program
    {
        static void Main(string[] args)
        {
            string input_str;   // usr input
            Regex pattern = new Regex("([H,h]ello )world"); // 根据用例与介绍，H不区分大小写，其余不知道
            Console.Write("Hi, input your name please:");
            string usr_name = Console.ReadLine();   // 先获取名字
            while (true)
            {
                Console.Write("input a string now:");
                input_str = Console.ReadLine(); // get input string
                Console.WriteLine(pattern.Replace(input_str, "$1" + usr_name));
            }
        }
    }
}
