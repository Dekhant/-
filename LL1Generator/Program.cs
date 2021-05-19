using System;
using System.IO;
using System.Collections.Generic;

namespace LL1Generator
{
    public class Program
    {
        public static void Main()
        {
            // AMOGUS ඞ
            var parsedRules = Parser.ParseInput(File.OpenRead("../../../input.txt"));
            var factorizedRules = Factorization.RemoveFactorization(parsedRules);
            var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var leads = Leads.FindLeads(removedRecursionRules);
            foreach (var rule in removedRecursionRules.Rules) Console.WriteLine(rule);
            var table = TableCreator.CreateTable(removedRecursionRules, leads);
            TableCreator.ExportTable(table);
            var input = TableRunner.ParseSentence(File.OpenRead("../../../sentence.txt"));
            try
            {
                TableRunner.Analyze(input, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine("Correct!");
        }

        private string convertLead(List<LL1Generator.Entities.RuleItem> lead)
        {
            string leadline = "";

            for(var i = 0; i < lead.Count; i++)
            {
                leadline += lead[i].Value;
                if(lead.Count > 1 && i != lead.Count-1)
                {
                    leadline += ", ";
                }
            }

            return leadline;
        }


        public List<string> checkTests(string way, List<string> rules)
        {
            var parsedRules = Parser.ParseInput(File.OpenRead(way));
            var factorizedRules = Factorization.RemoveFactorization(parsedRules);
            var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var leads = Leads.FindLeads(removedRecursionRules);
            for(var i = 0; i < removedRecursionRules.Rules.Count; i++)
            {
                rules.Add(removedRecursionRules.Rules[i].ToString() + " / " + convertLead(leads[i]));
            }

            return rules;
        }
    }
}