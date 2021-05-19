using System;
using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Leads
    {
        private static void FindUpRules(RuleList ruleList, string nonTerm, ref List<List<RuleItem>> leads,
            ref HashSet<Rule> usedNonTerms)
        {
            foreach (var rule in ruleList.Rules)
            {
                if (usedNonTerms.Contains(rule))
                {
                    continue;   
                }
                foreach (var item in rule.Items.Where(item => item.Value == nonTerm /*&& rule.NonTerminal != nonTerm*/))
                {
                    if (rule.Items.IndexOf(item) == rule.Items.Count - 1)
                    {
                        usedNonTerms.Add(rule);
                        FindUpRules(ruleList, rule.NonTerminal, ref leads, ref usedNonTerms);
                    }
                    else
                    {
                        if (!leads.Last().Contains(rule.Items[rule.Items.IndexOf(item) + 1]))
                        {
                            leads.Last().Add(rule.Items[rule.Items.IndexOf(item) + 1]);
                        }
                    }
                }
            }
        }
        //ОТРЫЖКИНА(НЕХОРОШКОВА) ДОЧЬ ЕБАНОЙ ШЛЮХИ, КОТОРУЮ ЕБУТ НЕГРЫ ПО КД ВО ВСЕ ЩЕЛИ
        public static List<List<RuleItem>> FindLeads(RuleList ruleList)
        {
            var leads = new List<List<RuleItem>>();
            foreach (var rule in ruleList.Rules)
                if (rule.Items[0].Value == Constants.EmptySymbol)
                {
                    var usedNonTerms = new HashSet<Rule>();
                    leads.Add(new List<RuleItem>());
                    FindUpRules(ruleList, rule.NonTerminal, ref leads, ref usedNonTerms);
                    usedNonTerms.Clear();
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
            Console.WriteLine();
            return leads;
        }
    }
}