using System;
using System.IO;

namespace LL1Generator
{
    internal static class Program
    {
        private static void Main()
        {
            var parsedRules = Parser.ParseInput(File.OpenRead("../../../input.txt"));
            var factorizedRules = Factorization.RemoveFactorization(parsedRules);
            var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var leads = Leads.FindLeads(removedRecursionRules);
            foreach (var rule in removedRecursionRules.Rules) Console.WriteLine(rule);
        }
    }
}