using LL1Generator.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LL1Generator
{
    public class TableRunner
    {
        public static string[] ParseSentence(Stream input)
        {
            using var sr = new StreamReader(input);
            string line = sr.ReadLine();

            return line.Split(new char[] { ' ' });
        }
        public static ImmutableList<int> Analyze(string[] input, List<TableRule> table)
        {
            var stack = new Stack<int>();
            var inputQ = new Queue<string>(input);
            var index = 0;
            var history = new List<int>();
            var inItem = inputQ.Peek();
            while (true)
            {
                history.Add(index);
                var tableItem = table[index];
                if (!tableItem.DirSet.Contains(inItem))
                {
                    if (tableItem.IsError)
                        GenerateException($"DirSet doesn't contain token: {inItem ?? "NULL"}");
                    index++;
                    continue;
                }

                if (tableItem.IsShift)
                    if (inputQ.Count == 0)
                    {
                        GenerateException("Input is empty but we need to shift.");
                    }
                    else
                    {
                        inputQ.Dequeue();
                        inItem = inputQ.Count > 0 ? inputQ.Peek() : null;
                    }

                if (tableItem.MoveToStack)
                    stack.Push(index + 1);
                if (tableItem.GoTo != null)
                {
                    index = tableItem.GoTo.Value;
                }
                else
                {
                    if (stack.Count > 0)
                        index = stack.Pop();
                    else if (inItem == null && tableItem.IsEnd)
                        break;
                    else
                        GenerateException("GoTo is null, stack is empty but other conditions are not met.");
                }
            }

            void GenerateException(string err)
            {
                throw new ArgumentException("[Syntax Analyzer Error] " + err +
                                            $"\nToken Number: {input.Length - inputQ.Count}, " +
                                            $"Stack: [{string.Join(", ", stack)}], InputQ: [{string.Join(", ", inputQ)}], TableItem: {index}\nHistory: [{string.Join(", ", history)}]");
            }

            return history.ToImmutableList();
        }
    }
}
