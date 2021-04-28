using LL1Generator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LL1Generator
{
    public static class Factorization
    {
        public static List<Rule> GetLongestCommonPrefix(List<Rule> rules, ref List<string> alphabet, ref bool didChange, ref List<string> nonTermsToAdd)
        {
            List<List<Rule>> factorContainer = new List<List<Rule>>();
            HashSet<List<RuleItem>> prefixes = new HashSet<List<RuleItem>>();
            var newRules = new List<Rule>();
            while (rules.Any())
            {
                var commonRulesList = new List<Rule>();
                bool foundSimilar = false;
                for (int i = 1; i < rules.Count; i++)
                {
                    if(rules[0].Items[0].Value == rules[i].Items[0].Value)
                    {
                        foundSimilar = true;
                        commonRulesList.Add(rules[i]);
                        rules.RemoveAt(i);
                        i--;
                    }
                }
                if(foundSimilar)
                {
                    commonRulesList.Add(rules[0]);
                }
                else
                {
                    newRules.Add(rules[0]);
                }
                rules.RemoveAt(0);
                if(commonRulesList.Any())
                {
                    factorContainer.Add(commonRulesList);
                }
            }
            if(factorContainer.Any())
            {
                didChange = true;
            }
            foreach (var factorContainerItem in factorContainer)
            {
                int maxRuleCount = Int32.MaxValue;
                foreach(var factorRules in factorContainerItem.Skip(1))
                {
                    int maxCount = 0;
                    for(int j = 0; j < factorRules.Items.Count; j++)
                    {
                        if(factorContainerItem[0].Items[j].Value == factorRules.Items[j].Value)
                        {
                            maxCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if(maxCount < maxRuleCount)
                    {
                        maxRuleCount = maxCount;
                    }
                }
                var rule = new Rule();
                rule.Items = new List<RuleItem>();
                string freeLetter = alphabet[0];
                for (int j = 0; j < maxRuleCount; j++)
                {
                    rule.Items.Add(new RuleItem(factorContainerItem[0].Items[j].Value, factorContainerItem[0].Items[j].IsTerminal));
                }
                rule.NonTerminal = factorContainerItem[0].NonTerminal;
                rule.Items.Add(new RuleItem(freeLetter, false));
                newRules.Add(rule);
                foreach (var factorItem in factorContainerItem)
                {
                    var factorRule = new Rule();
                    factorRule.Items = new List<RuleItem>();
                    if(factorItem.Items.Count == maxRuleCount)
                    {
                        factorRule.Items.Add(new RuleItem(Constants.EmptySymbol, true));
                    }
                    for (int j = maxRuleCount; j < factorItem.Items.Count; j++)
                    {
                        factorRule.Items.Add(new RuleItem(factorItem.Items[j].Value, factorItem.Items[j].IsTerminal));
                    }
                    factorRule.NonTerminal = freeLetter;
                    newRules.Add(factorRule);
                }
                alphabet.RemoveAt(0);
                nonTermsToAdd.Add(freeLetter);
            }
            return newRules;

        }
        
        public static RuleList RemoveFactorization(RuleList ruleList)
        {
            bool didChange = true;
            while (didChange)
            {
                didChange = false;
                var nonTermsToAdd = new List<string>();
                foreach (var nonTerm in ruleList.NonTerminals.ToList())
                {
                    var alphabet = ruleList.Alphabet;
                    List<Rule> rulesToPrefixCheck = new List<Rule>();

                    foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == nonTerm))
                    {
                        rulesToPrefixCheck.Add(rule);
                    }

                    List<Rule> factorizatedRules = GetLongestCommonPrefix(rulesToPrefixCheck, ref alphabet, ref didChange, ref nonTermsToAdd);
                    ruleList.Alphabet = alphabet;

                    foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == nonTerm).ToList())
                    {
                        ruleList.Rules.Remove(rule);
                    }
                    foreach (var factorizatedRule in factorizatedRules)
                    {
                        ruleList.Rules.Add(factorizatedRule);
                    }
                    foreach (var item in nonTermsToAdd)
                    {
                        ruleList.NonTerminals.Add(item);
                    }
                }
                
            }

            return ruleList;
        }
    }
}