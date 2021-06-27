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
            var useLexer = true;
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


            for (var i = 0; i < removedRecursionRules.Rules.Count; i++)
            {
                Console.Write(removedRecursionRules.Rules[i] + " / ");
                foreach (var lead in leads[i]) Console.Write(lead + ", ");
                Console.WriteLine();
            }

            var table = TableCreator.CreateTable(removedRecursionRules, leads);
            TableCreator.ExportTable(table);
            var input = TableRunner.ParseSentence(File.OpenRead("../../../sentence.txt"));
            List<int> history;
            if (useLexer)
            {
                var sr = new StreamReader("../../../sentence.txt");
                var sw = new StreamWriter("../../../outputLexer.txt");
                var lexer = new CLexer(ref sr, ref sw);
                string[] newInput;
                var result = new List<string>();
                foreach (var str in input.ToList())
                {
                    var typeToken = "";
                    lexer.RunPerToken(str, ref typeToken);
                    if (typeToken == "Identifier")
                        result.Add("id");
                    else if (typeToken == "Integer")
                        result.Add("!int");
                    else if (typeToken == "Float")
                        result.Add("!float");
                    else if (typeToken == "Char")
                        result.Add("!char");
                    else if (typeToken == "String")
                        result.Add("!string");
                    else
                        result.Add(str);
                }

                sr.Close();
                sw.Close();
                newInput = result.ToArray();
                try
                {
                    history = TableRunner.Analyze(newInput, table, useLexer);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else
            {
                try
                {
                    history = TableRunner.Analyze(input, table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }

            Console.WriteLine($"Correct! History: [{string.Join(", ", history)}]");
        }
    }
}