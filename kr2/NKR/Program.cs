using System;
using System.Collections.Generic;
using System.IO;

namespace NKR
{
    public class Program
    {
        public static bool Compare(List<int> multitudes, List<int> oldMultitudes)
        {
            if (multitudes.Count != oldMultitudes.Count)
            {
                return false;
            }
            for (int i = 0; i < multitudes.Count; i++)
            {
                if(multitudes[i] != oldMultitudes[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool HashCheckContain(HashSet<List<int>> Nodes, List<int> element)
        {
            foreach (List<int> el in Nodes)
            {
                if (Compare(el, element))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<int>[,] CreateTable(ref int[] sizes, string fileName)
        {
            using var input = new StreamReader(fileName);
            string str2, str3, str4;
            str2 = input.ReadLine();
            sizes[0] = int.Parse(str2);
            str3 = input.ReadLine();
            sizes[1] = int.Parse(str3);
            str4 = input.ReadLine();
            sizes[2] = int.Parse(str4);
            List<int>[,] table = new List<int>[sizes[0], sizes[2]];
            for (int i = 0; i < sizes[0]; i++)
            {
                string[] line = input.ReadLine().Split(" ");
                for (int j = 0; j < sizes[2]; j++)
                {
                    table[i, j] = new List<int>();
                    string[] values = line[j].Split(",");
                    for(int z = 0; z < values.Length; z++)
                    {
                        table[i, j].Add(int.Parse(values[z]));
                    }
                }
            }
            return table;
        }

        public static List<List<int>>[] Determinization(ref List<int>[,] table, int[] sizes, ref HashSet<List<int>> Nodes)
        {
            Queue<List<int>> MyQueue = new Queue<List<int>>();
            List<List<int>>[] newTable = new List<List<int>>[sizes[0]];
            for(int j = 0; j < sizes[0]; j++)
            {
                if (!HashCheckContain(Nodes, table[j, 0]))
                {
                    Nodes.Add(table[j, 0]);
                    MyQueue.Enqueue(table[j, 0]);
                }
            }
            int CurrTableState = 0;
            for (int j = 0; j < sizes[0]; j++)
            {
                newTable[j] = new List<List<int>>(0)
                {
                    table[j, CurrTableState]
                };
            }
            CurrTableState++;
            while (MyQueue.Count != 0)
            {
                List<int> Node = MyQueue.Dequeue();
                for (int j = 0; j < sizes[0]; j++)
                {
                    newTable[j].Add(new List<int>(0));
                }
                for (int j = 0; j < sizes[0]; j++)
                {
                    List<int> State = new List<int>();
                    foreach (int el in Node)
                    {
                        List<int> Elements = new List<int>();
                        Elements = table[j, el];
                        foreach (int elem in Elements)
                        {
                            if (elem != -1 && !State.Contains(elem))
                            {
                                State.Add(elem);
                            }
                        }
                        State.Sort();
                    }
                    if (!HashCheckContain(Nodes, State) && State.Count != 0)
                    {
                        Nodes.Add(State);
                        MyQueue.Enqueue(State);
                    }
                    newTable[j][CurrTableState] = State;
                }
                CurrTableState++;
            }
            return newTable;
        }

        public static void Main()
        {
        }
    }
}
