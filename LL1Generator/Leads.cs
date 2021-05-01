﻿using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Leads
    {
        private static void FindUpRules(RuleList ruleList, string nonTerm, ref List<List<RuleItem>> leads)
        {
            foreach (var rule in ruleList.Rules)
            foreach (var item in rule.Items.Where(item => item.Value == nonTerm))
                if (rule.Items.IndexOf(item) == rule.Items.Count - 1)
                {
                    FindUpRules(ruleList, rule.NonTerminal, ref leads);
                }
                else
                {
                    var lead = new List<RuleItem> {rule.Items[rule.Items.IndexOf(item) + 1]};
                    leads.Add(lead);
                }
        }

        public static List<List<RuleItem>> FindLeads(RuleList ruleList)
        {
            var leads = new List<List<RuleItem>>();
            foreach (var rule in ruleList.Rules)
                if (rule.Items[0].Value == Constants.EmptySymbol)
                {
                    FindUpRules(ruleList, rule.NonTerminal, ref leads);
                }
                else
                {
                    var lead = new List<RuleItem> {rule.Items[0]};
                    leads.Add(lead);
                }

            var foundNonTerm = true;
            while (foundNonTerm)
            {
                var uniqueEntrances = new HashSet<List<RuleItem>>();
                foundNonTerm = false;
                foreach (var lead in leads)
                foreach (var leadSymbol in lead.ToList().Where(leadSymbol => !leadSymbol.IsTerminal))
                {
                    foundNonTerm = true;
                    foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == leadSymbol.Value))
                    {
                        var index = ruleList.Rules.IndexOf(rule);
                        if (!uniqueEntrances.Contains(leads[index]))
                        {
                            uniqueEntrances.Add(leads[index]);
                            lead.AddRange(leads[index]);
                        }
                        else
                        {
                            return null;
                        }
                    }

                    lead.RemoveAt(lead.IndexOf(leadSymbol));
                }
            }

            return leads;
        }
    }
}