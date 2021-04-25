using LL1Generator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LL1Generator
{
    public static class Factorization
    {
        public static int GetLongestCommonPrefix(List<Rule> rules)
        {
            List<List<Rule>> factorContainer = new List<List<Rule>>();
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
                rules.RemoveAt(0);
                if(commonRulesList.Any())
                {
                    factorContainer.Add(commonRulesList);
                }
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
                Console.WriteLine(maxRuleCount);
                
            }
            return 1;
        }

        public static RuleList RemoveFactorization(RuleList ruleList)
        {
            foreach (var nonTerm in ruleList.NonTerminals)
            {
                List<Rule> rulesToPrefixCheck = new List<Rule>();

                foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == nonTerm))
                {
                    rulesToPrefixCheck.Add(rule);
                }

                int a = GetLongestCommonPrefix(rulesToPrefixCheck);
            }

            return ruleList;
        }
    }
}