using System;
using System.IO;

namespace LL1Generator
{
    internal static class Program
    {
        private static void Main()
        {
            var parsedRules = Parser.ParseInput(File.OpenRead("../../../input.txt"));
            var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(parsedRules);
            foreach (var rule in removedRecursionRules) Console.WriteLine(rule);
        }
    }
}