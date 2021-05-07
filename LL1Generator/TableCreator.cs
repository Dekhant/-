using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using LL1Generator.Entities;
using Spire.Xls;

namespace LL1Generator
{
    public static class TableCreator
    {
        public static List<TableRule> CreateTable(RuleList ruleList, List<List<RuleItem>> leads)
        {
            var table = new List<TableRule>();
            var id = 0;
            int? goTo = ruleList.Rules.Count;
            foreach (var rule in ruleList.Rules)
            {
                var nonTerms = ruleList.Rules.Where(x => x.NonTerminal == rule.NonTerminal);
                var lastIndex = ruleList.Rules.IndexOf(nonTerms.Last());
                table.Add(new TableRule {Id = id, NonTerminal = rule.NonTerminal, FirstsSet = leads[id], GoTo = goTo});
                table[id].IsError = id == lastIndex;
                id++;
                goTo += rule.Items.Count;
            }

            foreach (var rule in ruleList.Rules)
            {
                foreach (var item in rule.Items)
                {
                    var lead = new List<RuleItem>();
                    if (item.IsTerminal && item.Value != Constants.EmptySymbol)
                    {
                        lead.Add(item);
                        bool shift;
                        if (item.Value != Constants.EndSymbol)
                        {
                            if (rule.Items.IndexOf(item) == rule.Items.Count - 1)
                            {
                                shift = true;
                                goTo = null;
                            }
                            else
                            {
                                shift = true;
                                goTo = id + 1;
                            }
                        }
                        else
                        {
                            shift = true;
                            goTo = null;
                        }
                        var isEnd = item.Value == Constants.EndSymbol;
                        table.Add(new TableRule
                        {
                            Id = id,
                            NonTerminal = item.Value,
                            IsError = true,
                            GoTo = goTo,
                            FirstsSet = lead,
                            IsShift = shift,
                            MoveToStack = false,
                            IsEnd = isEnd
                        });
                    }
                    else
                    {
                        List<Rule> nonTerms = new List<Rule>();
                        if (!item.IsTerminal)
                        {
                            nonTerms = ruleList.Rules.Where(x => x.NonTerminal == item.Value).ToList();
                            goTo = ruleList.Rules.IndexOf(nonTerms.First());
                        }
                        else
                        {
                            nonTerms.Add(ruleList.Rules[ruleList.Rules.IndexOf(rule)]);
                            goTo = null;
                        }
                        foreach (var index in nonTerms.Select(nonTerm => ruleList.Rules.IndexOf(nonTerm)))
                        {
                            lead.AddRange(leads[index]);
                        }

                        var stack = rule.Items.IndexOf(item) != rule.Items.Count - 1;
                        table.Add(new TableRule
                        {
                            Id = id,
                            NonTerminal = item.Value,
                            IsError = true,
                            GoTo = goTo,
                            FirstsSet = lead,
                            IsShift = false,
                            MoveToStack = stack
                        });
                    }

                    id++;
                }
            }

            foreach(var row in table)
            {
                var ss = new HashSet<string>();
                foreach(var tabledir in row.FirstsSet)
                {
                    ss.Add(tabledir.Value);
                }
                row.DirSet = ss;
            }

            return table;
        }

        private static Worksheet InitSheet(Workbook wbToStream)
        {
            var sheet = wbToStream.Worksheets[0];

            sheet.Name = "Output";

            var style = wbToStream.Styles.Add("newStyle");
            style.Font.Size = 11;
            sheet.ApplyStyle(style);
            sheet.SetRowHeight(1, 20);
            sheet.SetColumnWidth(2, 15);
            sheet.SetColumnWidth(7, 15);
            sheet.Range["A1:H1"].Style.Font.IsBold = true;

            sheet.Range["A1:H1"].Style.Color = Color.LightBlue;
            sheet.Range["A1"].Text = "Id";
            sheet.Range["B1"].Text = "NonTerm";
            sheet.Range["C1"].Text = "firsts";
            sheet.Range["D1"].Text = "GoTo";
            sheet.Range["E1"].Text = "IsShift";
            sheet.Range["F1"].Text = "IsError";
            sheet.Range["G1"].Text = "MoveToStack";
            sheet.Range["H1"].Text = "IsEnd";

            return sheet;
        }

        public static void ExportTable(List<TableRule> table)
        {
            var wbToStream = new Workbook();
            var sheet = InitSheet(wbToStream);
            for (var i = 0; i < table.Count; i++)
            {
                sheet.Range[$"A{i + 2}"].NumberValue = table[i].Id + 1;
                sheet.Range[$"B{i + 2}"].Text = table[i].NonTerminal;
                // var firsts = table[i].FirstsSet.Aggregate(string.Empty, (current, lead) => current + lead.Value);
                var firsts = string.Empty;
                foreach (var lead in table[i].FirstsSet) firsts += lead.Value;
                sheet.Range[$"C{i + 2}"].Text = firsts;
                if (table[i].GoTo != null)
                    sheet.Range[$"D{i + 2}"].NumberValue = (double) table[i].GoTo;
                else
                    sheet.Range[$"D{i + 2}"].Text = "NULL";
                sheet.Range[$"E{i + 2}"].NumberValue = Convert.ToInt32(table[i].IsShift);
                sheet.Range[$"F{i + 2}"].NumberValue = Convert.ToInt32(table[i].IsError);
                sheet.Range[$"G{i + 2}"].NumberValue = Convert.ToInt32(table[i].MoveToStack);
                sheet.Range[$"H{i + 2}"].NumberValue = Convert.ToInt32(table[i].IsEnd);
            }

            var fileStream = new FileStream("../../../Output.xls", FileMode.Create);
            wbToStream.SaveToStream(fileStream);
            fileStream.Close();
        }
    }
}