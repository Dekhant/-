using System;
using System.Collections.Generic;
using System.Linq;
using LL1Generator.Entities;

namespace LL1Generator
{
    public static class Leads
    {
        static bool isLLgram(ref List<List<RuleItem>> leads, ref RuleList ruleList)
        {
            for(int i = 0; i < leads.Count; i++)
            {
                for(int j = 0; j < leads.Count; j++)
                {
                    if(i == j)
                    {
                        break;
                    }
                    if(leads[i].Count == leads[j].Count)
                    {
                        string l1 = "";
                        string r1 = "";
                        string l2 = "";
                        string r2 = "";
                        for (int n = 0; n < leads[i].Count; n++)
                        {
                            l1 += leads[i][n].Value;
                            l2 += leads[j][n].Value;
                        }
                        for (int n = 0; n < ruleList.Rules[i].Items.Count; n++)
                        {
                            r1 += ruleList.Rules[i].Items[n].Value;
                        }
                        for (int n = 0; n < ruleList.Rules[j].Items.Count; n++)
                        {
                            r2 += ruleList.Rules[j].Items[n].Value;
                        }
                        if (l1 == l2 && r1 == r2)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static void FindUpRules(RuleList ruleList, string nonTerm, ref List<List<RuleItem>> leads)
        {
            foreach(var rule in ruleList.Rules)
            {
                foreach(var item in rule.Items)
                {
                    if(item.Value == nonTerm)
                    {
                        if(rule.Items.IndexOf(item) == (rule.Items.Count - 1))
                        {
                            FindUpRules(ruleList, rule.NonTerminal, ref leads);
                        }
                        else
                        {
                            var lead = new List<RuleItem>();
                            lead.Add(rule.Items[rule.Items.IndexOf(item) + 1]);
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
            {
                if(rule.Items[0].Value == Constants.EmptySymbol)
                {
                    FindUpRules(ruleList, rule.NonTerminal, ref leads);
                }
                else
                {
                    var lead = new List<RuleItem>();
                    lead.Add(rule.Items[0]);
                    leads.Add(lead);
                }
            }
            bool foundNonTerm = true;
            while (foundNonTerm)
            {
                var uniqueEnterances = new HashSet<List<RuleItem>>();
                foundNonTerm = false;
                foreach (var lead in leads)
                {          
                    foreach (var leadSymbol in lead.ToList())
                    {
                        if (!leadSymbol.IsTerminal)
                        {
                            foundNonTerm = true;
                            foreach (var rule in ruleList.Rules.Where(x => x.NonTerminal == leadSymbol.Value))
                            {
                                int index = ruleList.Rules.IndexOf(rule);
                                if (!uniqueEnterances.Contains(leads[index]))
                                {
                                    uniqueEnterances.Add(leads[index]);   
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
                }
            }
            if(!isLLgram(ref leads, ref ruleList))
            {
                return null;
            }
            return leads;
        }
    }
}