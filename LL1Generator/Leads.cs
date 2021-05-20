using System;
using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Leads
    {
        private struct Used
        {
            public Used(Rule rule, int index)
            {
                Rule = rule;
                Index = index;
            }
            public Rule Rule { get; set; }
            public int Index { get; set; }
            public override string ToString() => $"({Rule}, {Index})";

        }
        private static List<RuleItem> FindUpRule(RuleList ruleList, RuleItem emptyItem, ref List<Used> usedRules)
        {
            var lead = new List<RuleItem>();
            for(int i = 0; i < ruleList.Rules.Count; i++)
            {
                var item = ruleList.Rules[i].Items.Where(x => x.Value == emptyItem.Value).ToList();
                if (item.Count != 0)
                {
                    for (int j = 0; j < item.Count; j++)
                    {
                        int index = ruleList.Rules[i].Items.IndexOf(item[j]);
                        if (!usedRules.Contains(new Used(ruleList.Rules[i], index)))
                        {
                            if (index == ruleList.Rules[i].Items.Count - 1)
                            {
                                usedRules.Add(new Used(ruleList.Rules[i], index));
                                emptyItem = new RuleItem(ruleList.Rules[i].NonTerminal, false);
                                lead.AddRange(FindUpRule(ruleList, emptyItem, ref usedRules));
                            }
                            else
                            {
                                lead.Add(ruleList.Rules[i].Items[index + 1]);
                            }
                        }
                    }
                }
            }
            return lead;
        }
        public static List<List<RuleItem>> FindLeads(RuleList ruleList)
        {
            var leads = new List<List<RuleItem>>(ruleList.Rules.Count);
            for(int i = 0; i < ruleList.Rules.Count; i++)
            {
                leads.Add(new List<RuleItem>());
                var rule = ruleList.Rules[i];
                if(rule.Items[0].Value == Constants.EmptySymbol)
                {
                    var emptyItem = new RuleItem(rule.NonTerminal, false);
                    var usedRules = new List<Used>();
                    var lead = FindUpRule(ruleList, emptyItem, ref usedRules).ToHashSet().ToList();
                    leads[i].AddRange(lead);
                    usedRules.Clear();
                }
                else
                {
                    leads[i].Add(rule.Items[0]);
                }
            }
            var foundNonTerm = true;
            while (foundNonTerm)
            {
                foundNonTerm = false;
                foreach (var nonTerm in ruleList.NonTerminals)
                {
                    var uniqueEntrances = new HashSet<List<RuleItem>>();
                    foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == nonTerm))
                    {
                        var index = ruleList.Rules.IndexOf(rule);
                        foreach (var leadSymbol in leads[index].ToList().Where(leadSymbol => !leadSymbol.IsTerminal))
                        {
                            foundNonTerm = true;
                            foreach (var leadRule in ruleList.Rules.Where(x => x.NonTerminal == leadSymbol.Value && x != rule))
                            {
                                var leadRuleIndex = ruleList.Rules.IndexOf(leadRule);
                                if (!uniqueEntrances.Contains(leads[leadRuleIndex]))
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
            int j = 0;
            foreach(var lead in leads.ToList())
            {
                leads[j] = lead.ToHashSet().ToList();
                j++;
            }
            return leads;

        }
    }
}