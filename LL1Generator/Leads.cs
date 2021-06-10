using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Leads
    {
        private static IEnumerable<RuleItem> FindUpRule(RuleList ruleList, RuleItem emptyItem, ref List<Used> usedRules)
        {
            var lead = new List<RuleItem>();
            foreach (var rule in ruleList.Rules)
            {
                var item = rule.Items.Where(x => x.Value == emptyItem.Value).ToList();
                if (item.Count != 0)
                {
                    foreach (var index in item.Select(t => rule.Items.IndexOf(t)))
                    {
                        if (!usedRules.Contains(new Used(rule, index)))
                        {
                            if (index == rule.Items.Count - 1)
                            {
                                usedRules.Add(new Used(rule, index));
                                var newEmptyItem = new RuleItem(rule.NonTerminal, false);
                                lead.AddRange(FindUpRule(ruleList, newEmptyItem, ref usedRules));
                            }
                            else
                            {
                                lead.Add(rule.Items[index + 1]);
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
            for (var i = 0; i < ruleList.Rules.Count; i++)
            {
                leads.Add(new List<RuleItem>());
                var rule = ruleList.Rules[i];
                if (rule.Items[0].Value == Constants.EmptySymbol)
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
                    foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == nonTerm))
                    {
                        var index = ruleList.Rules.IndexOf(rule);
                        foreach (var leadSymbol in leads[index].ToList().Where(leadSymbol => !leadSymbol.IsTerminal))
                        {
                            foundNonTerm = true;
                            foreach (var leadRule in ruleList.Rules.Where(x =>
                                x.NonTerminal == leadSymbol.Value && x != rule))
                            {
                                var leadRuleIndex = ruleList.Rules.IndexOf(leadRule);

                                leads[index].AddRange(leads[leadRuleIndex]);
                            }

                            leads[index].RemoveAt(leads[index].IndexOf(leadSymbol));
                        }
                    }
                }
            }

            var j = 0;
            foreach (var lead in leads.ToList())
            {
                leads[j] = lead.ToHashSet().ToList();
                j++;
            }

            return leads;
        }

        private readonly struct Used
        {
            public Used(Rule rule, int index)
            {
                Rule = rule;
                Index = index;
            }

            private Rule Rule { get; }
            private int Index { get; }
            public override string ToString() => $"({Rule}, {Index})";
        }
    }
}