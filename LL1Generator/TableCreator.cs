using LL1Generator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL1Generator
{
    public static class TableCreator
    {
        public static void CreateTable(RuleList ruleList, List<List<RuleItem>> leads)
        {
            var table = new List<TableRule>();
            int id = 0;
            int? goTo = ruleList.Rules.Count;
            foreach (var rule in ruleList.Rules)
            {
                var nonTerms = ruleList.Rules.Where(x => x.NonTerminal == rule.NonTerminal);
                int lastIndex = ruleList.Rules.IndexOf(nonTerms.Last());
                table.Add(new TableRule() { Id = id, NonTerminal = rule.NonTerminal, FirstsSet = leads[id], GoTo = goTo});
                if(id == lastIndex)
                {
                    table[id].IsError = true;
                }
                else
                {
                    table[id].IsError = false;
                }
                id++;
                goTo += rule.Items.Count;
            }
            foreach(var rule in ruleList.Rules)
            {
                foreach(var item in rule.Items)
                {
                    var lead = new List<RuleItem>();
                    if(item.IsTerminal)
                    {
                        lead.Add(item);
                        bool shift;
                        if(rule.Items.IndexOf(item) == (rule.Items.Count - 1))
                        {
                            shift = false;
                            goTo = null;
                        }
                        else
                        {
                            shift = true;
                            goTo = id + 1;
                        }
                        bool isEnd = false;
                        if(item.Value == Constants.EndSymbol)
                        {
                            isEnd = true;
                        }
                        table.Add(new TableRule() { Id = id, NonTerminal = item.Value, IsError = true, GoTo = goTo, FirstsSet = lead,  IsShift = shift, MoveToStack = false, IsEnd = isEnd});
                    }
                    else
                    {
                        var nonTerms = ruleList.Rules.Where(x => x.NonTerminal == item.Value);
                        goTo = ruleList.Rules.IndexOf(nonTerms.First());
                        bool stack;
                        foreach(var nonTerm in nonTerms)
                        {
                            int index = ruleList.Rules.IndexOf(nonTerm);
                            lead.AddRange(leads[index]); 
                        }
                        if (rule.Items.IndexOf(item) == (rule.Items.Count - 1))
                        {
                            stack = false;
                        }
                        else
                        {
                            stack = true;
                        }
                        table.Add(new TableRule() { Id = id, NonTerminal = item.Value, IsError = true, GoTo = goTo, FirstsSet = lead, IsShift = false, MoveToStack = stack });
                    }
                    id++;
                    
                }
            }
            Console.WriteLine("huita");
        }
    }
}
