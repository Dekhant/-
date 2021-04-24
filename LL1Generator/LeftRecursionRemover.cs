using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class LeftRecursionRemover
    {
        public static IEnumerable<Rule> RemoveLeftRecursion(RuleList rules)
        {
            var newRuleList = new List<Rule>();
            foreach (var nonTerm in rules.NonTerminals)
            {
                var commonRules = new List<Rule>();
                var leftRecursionRules = new List<Rule>();
                var freeLetter = rules.Alphabet[0];
                foreach (var rule in rules.Rules.Where(x => x.NonTerminal == nonTerm))
                    if (rule.Items[0].Value == nonTerm)
                        leftRecursionRules.Add(rule);
                    else
                        commonRules.Add(rule);
                if (leftRecursionRules.Any())
                {
                    rules.Alphabet.RemoveAt(0);
                    foreach (var commonRule in commonRules)
                    {
                        commonRule.Items.Add(new RuleItem(freeLetter, false));
                        newRuleList.Add(commonRule);
                    }

                    foreach (var leftRecursionRule in leftRecursionRules)
                    {
                        leftRecursionRule.Items.Skip(1).ToList().Add(new RuleItem(freeLetter, false));
                        newRuleList.Add(new Rule
                        {
                            NonTerminal = freeLetter,
                            Items = leftRecursionRule.Items
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

            return newRuleList;
        }
    }
}