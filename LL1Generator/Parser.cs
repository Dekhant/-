using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Parser
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

            var alphabet = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => ((char) i).ToString()).ToList();
            foreach (var nonTerminal in nonTerminals.ToList()) alphabet.Remove(nonTerminal);

            return new RuleList(nonTerminals, rules)
            {
                Alphabet = alphabet
            };
        }
    }
}