using LL1Generator.Entities;
using System;
using System.Collections.Generic;
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
            var leads = Leads.FindLeads(removedRecursionRules);
            foreach (var rule in removedRecursionRules.Rules) Console.WriteLine(rule);
            List < TableRule > table = TableCreator.CreateTable(removedRecursionRules, leads);
            TableCreator.ExportTable(table);
            var input = TableRunner.ParseSentence(File.OpenRead("../../../sentence.txt"));
            TableRunner.Analyze(input, table);
        }
    }
}