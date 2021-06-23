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
            foreach (var nonterm in removedRecursionRules.NonTerminals)
            {
                var rules = removedRecursionRules.Rules.Where(x => x.NonTerminal == nonterm).ToList();
                if (rules.Count > 1)
                {
                    var uniqueLeads = new List<RuleItem>();
                    foreach (var rule in rules)
                    {
                        var index = removedRecursionRules.Rules.IndexOf(rule);
                        var lead = leads[index];
                        foreach (var item in lead)
                        {
                            foreach (var unique in uniqueLeads)
                            {
                                if (unique.Value == item.Value)
                                {
                                    Console.WriteLine("Not LL grammar");
                                    return;
                                }
                            }
                        }
                        uniqueLeads.AddRange(lead);
                    }
                }
            }
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

        //private static string ConvertLead(HashSet<RuleItem> lead)
        //{
        //    var leadLine = "";

        //    for (var i = 0; i < lead.Count; i++)
        //    {
        //        leadLine += lead[i].Value;
        //        if (lead.Count > 1 && i != lead.Count - 1)
        //        {
        //            leadLine += ", ";
        //        }
        //    }

        //    return leadLine;
        //}


        //public static List<string> CheckTests(string way, List<string> rules)
        //{
        //    var parsedRules = Parser.ParseInput(File.OpenRead(way));
        //    var factorizedRules = Factorization.RemoveFactorization(parsedRules);
        //    var removedRecursionRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
        //    var leads = Leads.FindLeads(removedRecursionRules);
        //    if(leads == null)
        //    {
        //        return null;
        //    }
        //    for (var i = 0; i < removedRecursionRules.Rules.Count; i++)
        //    {
        //        rules.Add(removedRecursionRules.Rules[i] + " / " + ConvertLead(leads[i]));
        //    }

        //    return rules;
        //}
    }
}