using System;
using System.IO;
using SLRGenerator.Table;

namespace SLRGenerator
{
    internal static class Program
    {
        private static void Main()
        {
            var useLexer = true;
            var rules = SimpleRulesParser.Parse(File.OpenRead("rules.txt"));
            var tableBuilder = new TableBuilder(rules);
            var tableRules = tableBuilder.CreateTable();
            CsvExport.SaveToCsv(tableRules);

            var input = File.OpenRead("input.txt");

            if (useLexer)
            {
                var analyzer = new Analyzer(input, tableRules, rules);
                try
                {
                    analyzer.Analyze();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}