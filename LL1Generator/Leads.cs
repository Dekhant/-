using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Leads
    {
        private static IEnumerable<RuleItem> FindUpRule(RuleList ruleList, RuleItem emptyItem, ref List<Used> usedRules)
        {
            var lead = new HashSet<RuleItem>();
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
                                lead.UnionWith(FindUpRule(ruleList, newEmptyItem, ref usedRules));
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

        public static List<HashSet<RuleItem>> FindLeads(RuleList ruleList)
        {
            var leads = new List<HashSet<RuleItem>>(ruleList.Rules.Count);
            for (var i = 0; i < ruleList.Rules.Count; i++)
            {
                leads.Add(new HashSet<RuleItem>());
                var rule = ruleList.Rules[i];
                if (rule.Items[0].Value == Constants.EmptySymbol)
                {
                    var emptyItem = new RuleItem(rule.NonTerminal, false);
                    var usedRules = new List<Used>();
                    var lead = FindUpRule(ruleList, emptyItem, ref usedRules).ToHashSet().ToList();
                    leads[i].UnionWith(lead);
                    usedRules.Clear();
                }
                else
                {
                    leads[i].Add(rule.Items[0]);
                }
            }
           for(; ; )
            {
                var somethingChanged = false;
                foreach(var lead in leads)
                {
                    var nonTerms = lead.Where(x => !x.IsTerminal).ToList();
                    if(nonTerms.Count > 0)
                    {
                        somethingChanged = true;
                    }
                    foreach(var nonTerm in nonTerms)
                    {
                        lead.Remove(nonTerm);
                        var rulesWithNonTerm = ruleList.Rules.Select((x, i) => (x, i)).Where(x => x.x.NonTerminal == nonTerm.Value).Select(x => x.i).ToList();
                        foreach(var fVal in rulesWithNonTerm.SelectMany(rule => leads[rule]).ToList())
                        {
                            lead.Add(fVal);
                        }
                    }
                }
                if(!somethingChanged)
                {
                    break;
                }
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