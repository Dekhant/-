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
            var factorizedRules = Factorization.RemoveFactorization(removedRecursionRules);
            // AMOGUS ඞ
            foreach (var rule in factorizedRules.Rules) Console.WriteLine(rule);
        }
    }
}