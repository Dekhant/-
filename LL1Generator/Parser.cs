using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace LL1Generator
{
    public static class Constants
    {
        public const string EmptySymbol = "e";
        public const string NewLineSymbol = "$";
    }

    public class Rule
    {
        public string NonTerminal { get; set; }
        public List<RuleItem> Items { get; set; }
        public override string ToString()
        {
            return NonTerminal.ToString() + " -> " + string.Join(" ", Items.Select(x => x.Value));
        }
    }

    public class RuleList
    {
        public readonly HashSet<string> NonTerminals;
        public readonly List<Rule> Rules;
        public List<string> Alphabet = new List<string>();


        public RuleList(HashSet<string> nonTerminals, List<Rule> rules)
        {
            NonTerminals = nonTerminals;
            Rules = rules;
        }
    }

    public class RuleItem
    {
        public readonly string Value;
        public readonly bool IsTerminal;

        public RuleItem(string Value, bool IsTerminal)
        {
            this.Value = Value;
            this.IsTerminal = IsTerminal;
        }
    }

    public class Parser
    {
        
        public static RuleList ParseInput(Stream input)
        {
            using var sr = new StreamReader(input);
            string line;
            var rawRules = new List<(string LeftBody, string RightBody)>();
            while ((line = sr.ReadLine()) != null)
            {
                var split = line.Split("->", StringSplitOptions.TrimEntries);
                var localRules = split[1].Split("|", StringSplitOptions.TrimEntries);
                rawRules.AddRange(localRules.Select(rule => (split[0], rule)));
            }

            var nonTerminals = rawRules.Select(x => x.LeftBody).ToHashSet();
            var rules = rawRules.Select(rawRule => new Rule
            {
                NonTerminal = rawRule.LeftBody,
                Items = rawRule.RightBody.Split(" ", StringSplitOptions.TrimEntries)
                    .Select(x => new RuleItem(x, !nonTerminals.Contains(x)))
                    .ToList()
            }).ToList();

            if (rules[0].Items[^1].Value != Constants.NewLineSymbol)
                rules[0].Items.Add(new RuleItem(Constants.NewLineSymbol, true));
            var alphabet = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => ((Char)i).ToString()).ToList();
            foreach (var nonTeminal in nonTerminals.ToList())
            {
                alphabet.Remove(nonTeminal);
            }

            return new RuleList(nonTerminals, rules)
            {
                Alphabet = alphabet
            };
        }
    }

    
}
