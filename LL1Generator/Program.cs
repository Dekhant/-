using System;
using System.IO;

namespace LL1Generator
{
    class Program
    {
        static void Main()
        {
            var rulesStream = File.OpenRead("../../../input.txt");
            var rules = Parser.ParseInput(rulesStream);
            var RemovedRecursion = ParsedRuleProcessor.RemoveLeftRecursion(rules);
            foreach(var rule in RemovedRecursion)
            {
                Console.WriteLine(rule);
            }
        }
    }
}
