using System;
using System.IO;

namespace LL1Generator
{
    class Program
    {
        static void Main()
        {
            var rulesStream = File.OpenRead("rules.txt");
            Console.WriteLine("Hello World!");
        }
    }
}
