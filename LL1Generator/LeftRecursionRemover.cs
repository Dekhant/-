using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class LeftRecursionRemover
    {
        public static RuleList RemoveLeftRecursion(RuleList ruleList)
        {
            var newRuleList = new List<Rule>();
            var nonTerminals = ruleList.NonTerminals;
            var nonTermsToAdd = new HashSet<string>();
            foreach (var nonTerm in nonTerminals)
            {
                var commonRules = new List<Rule>();
                var leftRecursionRules = new List<Rule>();
                var freeLetter = ruleList.Alphabet[0];
                foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == nonTerm))
                    if (rule.Items[0].Value == nonTerm)
                        leftRecursionRules.Add(rule);
                    else
                        commonRules.Add(rule);
                if (leftRecursionRules.Any())
                {
                    nonTermsToAdd.Add(freeLetter);
                    ruleList.Alphabet.RemoveAt(0);
                    foreach (var commonRule in commonRules)
                    {
                        if (commonRule.Items.Count == 1 && commonRule.Items[0].Value == Constants.EmptySymbol)
                        {
                            commonRule.Items.RemoveAt(0);
                        }
                        commonRule.Items.Add(new RuleItem(freeLetter, false));
                        newRuleList.Add(commonRule);
                    }

                    foreach (var leftRecursionRule in leftRecursionRules)
                    {
                        var leftRecursionItems = leftRecursionRule.Items.Skip(1).ToList();
                        leftRecursionItems.Add(new RuleItem(freeLetter, false));
                        newRuleList.Add(new Rule
                        {
                            NonTerminal = freeLetter,
                            Items = leftRecursionItems
                        });
                    }

                    newRuleList.Add(new Rule
                    {
                        NonTerminal = freeLetter,
                        Items = new List<RuleItem> {new(Constants.EmptySymbol, true)}
                    });
                }
                else
                {
                    newRuleList.AddRange(commonRules);
                }
            }

            ruleList.NonTerminals.UnionWith(nonTermsToAdd);
            return new RuleList(nonTerminals, newRuleList)
            {
                Alphabet = ruleList.Alphabet
            };
        }
    }
}