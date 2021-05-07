using System;
using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Leads
    {
        private static void FindUpRules(RuleList ruleList, string nonTerm, ref List<List<RuleItem>> leads, ref HashSet<Rule> usedNonTerms)
        {
            foreach (var rule in ruleList.Rules)
            {
                if (!usedNonTerms.Contains(rule))
                {
                    foreach (var item in rule.Items.Where(item => item.Value == nonTerm && rule.NonTerminal != nonTerm))
                    {
                        if (rule.Items.IndexOf(item) == rule.Items.Count - 1)
                        {
                            usedNonTerms.Add(rule);
                            FindUpRules(ruleList, rule.NonTerminal, ref leads, ref usedNonTerms);
                        }
                        else
                        {
                            var lead = new List<RuleItem> { rule.Items[rule.Items.IndexOf(item) + 1] };
                            leads.Add(lead);
                        }
                    }
                }
            }
        }

        public static List<List<RuleItem>> FindLeads(RuleList ruleList)
        {
            var leads = new List<List<RuleItem>>();
            foreach (var rule in ruleList.Rules)
                if (rule.Items[0].Value == Constants.EmptySymbol)
                {
                    var usedNonTerms = new HashSet<Rule>();
                    FindUpRules(ruleList, rule.NonTerminal, ref leads, ref usedNonTerms);
                }
                else
                {
                    var lead = new List<RuleItem> {rule.Items[0]};
                    leads.Add(lead);
                }

            var foundNonTerm = true;
            while (foundNonTerm)
            {
                foundNonTerm = false;
                foreach(string nonTerm in ruleList.NonTerminals)
                {
                    var uniqueEntrances = new HashSet<List<RuleItem>>();
                    foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == nonTerm))
                    {
                        int index = ruleList.Rules.IndexOf(rule);
                        foreach(var leadSymbol in leads[index].ToList().Where(leadSymbol => !leadSymbol.IsTerminal))
                        {
                            foundNonTerm = true;
                            foreach(var leadRule in ruleList.Rules.Where(x => x.NonTerminal == leadSymbol.Value))
                            {
                                var leadRuleIndex = ruleList.Rules.IndexOf(leadRule);
                                if(!uniqueEntrances.Contains(leads[leadRuleIndex]))
                                {
                                    uniqueEntrances.Add(leads[leadRuleIndex]);
                                    leads[index].AddRange(leads[leadRuleIndex]);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            leads[index].RemoveAt(leads[index].IndexOf(leadSymbol));
                        }
                    }
                }
            }
            return leads;
        }
    }
}