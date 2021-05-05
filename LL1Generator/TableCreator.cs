using LL1Generator.Entities;
using System.Collections.Generic;
using System.Linq;
using Spire.Xls;
using System.IO;
using System.Windows;
using System;
using System.Drawing;

namespace LL1Generator
{
    public static class TableCreator
    {
        public static List<TableRule> CreateTable(RuleList ruleList, List<List<RuleItem>> leads)
        {
            var table = new List<TableRule>();
            int id = 0;
            int? goTo = ruleList.Rules.Count;
            foreach (var rule in ruleList.Rules)
            {
                var nonTerms = ruleList.Rules.Where(x => x.NonTerminal == rule.NonTerminal);
                int lastIndex = ruleList.Rules.IndexOf(nonTerms.Last());
                table.Add(new TableRule() { Id = id, NonTerminal = rule.NonTerminal, FirstsSet = leads[id], GoTo = goTo });
                if (id == lastIndex)
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
            foreach (var rule in ruleList.Rules)
            {
                foreach (var item in rule.Items)
                {
                    var lead = new List<RuleItem>();
                    if (item.IsTerminal)
                    {
                        lead.Add(item);
                        bool shift;
                        if (rule.Items.IndexOf(item) == (rule.Items.Count - 1))
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
                        if (item.Value == Constants.EndSymbol)
                        {
                            isEnd = true;
                        }
                        table.Add(new TableRule() { Id = id, NonTerminal = item.Value, IsError = true, GoTo = goTo, FirstsSet = lead, IsShift = shift, MoveToStack = false, IsEnd = isEnd });
                    }
                    else
                    {
                        var nonTerms = ruleList.Rules.Where(x => x.NonTerminal == item.Value);
                        goTo = ruleList.Rules.IndexOf(nonTerms.First());
                        bool stack;
                        foreach (var nonTerm in nonTerms)
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
            return table;
        }

        private static Worksheet InitSheet(Workbook wbToStream)
        {
            Worksheet sheet = wbToStream.Worksheets[0];

            sheet.Name = "Test";

            CellStyle style = wbToStream.Styles.Add("newStyle");
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
            Workbook wbToStream = new Workbook();
            Worksheet sheet = InitSheet(wbToStream);
            for (var i = 0; i < table.Count; i++)
            {
                sheet.Range[$"A{i + 2}"].NumberValue = table[i].Id;
                sheet.Range[$"B{i+2}"].Text = table[i].NonTerminal;
                string firsts = string.Empty;
                foreach(var lead in table[i].FirstsSet)
                {
                    firsts += lead.Value;
                }
                sheet.Range[$"C{i + 2}"].Text = firsts;
                if(table[i].GoTo != null)
                {
                    sheet.Range[$"D{i + 2}"].NumberValue = (double)table[i].GoTo;
                }
                else
                {
                    sheet.Range[$"D{i + 2}"].Text = "NULL";
                }
                sheet.Range[$"E{i + 2}"].NumberValue = Convert.ToInt32(table[i].IsShift);
                sheet.Range[$"F{i + 2}"].NumberValue = Convert.ToInt32(table[i].IsError);
                sheet.Range[$"G{i + 2}"].NumberValue = Convert.ToInt32(table[i].MoveToStack);
                sheet.Range[$"H{i + 2}"].NumberValue = Convert.ToInt32(table[i].IsEnd);
            }
            FileStream file_stream = new FileStream("../../../Output.xls", FileMode.Create);
            wbToStream.SaveToStream(file_stream);
            file_stream.Close();

        }
    }
}
