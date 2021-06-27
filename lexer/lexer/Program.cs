using System;
using System.IO;

namespace lexer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("wrong arguments number");
                return;
            }

            var sr = new StreamReader("../../../input.txt");
            var sw = new StreamWriter("../../../output.txt");

            var lexer = new CLexer(ref sr, ref sw);
            lexer.Run();
            sr.Close();
            sw.Close();
        }
    }
}