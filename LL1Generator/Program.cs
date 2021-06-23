using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public class Program
    {
        public static void Main()
        {
            var parsedRules = Parser.ParseInput(File.OpenRead("../../../input.txt"));
            var factorizedRules = Factorization.RemoveFactorization(parsedRules);
            var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var leads = Leads.FindLeads(removedRecursionRules);
            foreach (var nonTerm in removedRecursionRules.NonTerminals)
            {
                var rules = removedRecursionRules.Rules.Where(x => x.NonTerminal == nonTerm).ToList();
                if (rules.Count > 1)
                {
                    var uniqueLeads = new List<RuleItem>();
                    foreach (var rule in rules)
                    {
                        var index = removedRecursionRules.Rules.IndexOf(rule);
                        var lead = leads[index];
                        if (lead.Any(item => uniqueLeads.Any(unique => unique.Value == item.Value)))
                        {
                            Console.WriteLine("Not LL grammar");
                            return;
                        }

                        uniqueLeads.AddRange(lead);
                    }
                }
            }

            foreach (var rule in removedRecursionRules.Rules) Console.WriteLine(rule);
            var table = TableCreator.CreateTable(removedRecursionRules, leads);
            TableCreator.ExportTable(table);
            var input = TableRunner.ParseSentence(File.OpenRead("../../../sentence.txt"));
            List<int> history;
            try
            {
                history = TableRunner.Analyze(input, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"Correct! History: [{string.Join(", ", history)}]");
        }
    }
}