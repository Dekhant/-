using System;
using System.IO;

namespace LL1Generator
{
    internal static class Program
    {
        private static void Main()
        {
            // AMOGUS ඞ
            var parsedRules = Parser.ParseInput(File.OpenRead("../../../input.txt"));
            var factorizedRules = Factorization.RemoveFactorization(parsedRules);
            var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            foreach (var rule in removedRecursionRules.Rules) Console.WriteLine(rule);
        }
    }
}