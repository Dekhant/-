using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Interpretater
{
    class Program
    {
        static int steps = 0;
        static string[,] CreateTable(ref int[] sizes, string fileName)
        {
            using var input = new StreamReader(fileName);
            string str1, str2, str3, str4;
            str1 = input.ReadLine();
            sizes[3] = int.Parse(str1);
            str2 = input.ReadLine();
            sizes[0] = int.Parse(str2);
            str3 = input.ReadLine();
            sizes[1] = int.Parse(str3);
            str4 = input.ReadLine();
            sizes[2] = int.Parse(str4);

            if (sizes[3] == 1)
            {
                string[,] table = new string[sizes[0], sizes[2]];
                for (int i = 0; i < sizes[0]; i++)
                {
                    string[] line = input.ReadLine().Split(" ");
                    int k = 0;
                    for (int j = 0; j < sizes[2]; j++)
                    {
                        table[i, j] = line[k] + "/" + line[k + 1];
                        k += 2;
                    }
                }
                return table;
            }
            if (sizes[3] == 2)
            {
                string[,] table = new string[sizes[0] + 1, sizes[2]];
                for (int i = 0; i < sizes[0] + 1; i++)
                {
                    string[] line = input.ReadLine().Split(" ");
                    for (int j = 0; j < sizes[2]; j++)
                    {
                        table[i, j] = line[j];
                    }
                }
                return table;
            }
            return null;
        }

        static string[,] MooreToMealy(string[,] table, int[] sizes)
        {
            string[,] newAutomat = new string[sizes[0], sizes[2]];
            for (int i = 0; i < sizes[0]; i++)
            {
                for (int j = 0; j < sizes[2]; j++)
                {
                    newAutomat[i, j] += table[i + 1, j] + "/" + table[0, int.Parse(table[i + 1, j])];
                    Console.Write(newAutomat[i, j] + " ");
                }
                Console.WriteLine();
            }
            return newAutomat;
        }

        static string[,] MealyToMoore(string[,] table, int[] sizes)
        {
            List<List<string>> nodes = new List<List<string>>();
            for (int i = 0; i < sizes[2]; i++)
            {
                List<string> nodes2 = new List<string>();
                nodes.Add(nodes2);
            }
            for (int i = 0; i < sizes[0]; i++)
            {
                for (int j = 0; j < sizes[2]; j++)
                {
                    if (!nodes[int.Parse(table[i, j].Split("/")[0])].Contains(table[i, j]))
                    {
                        nodes[int.Parse(table[i, j].Split("/")[0])].Add(table[i, j]);
                    }
                }
            }
            int columnsNum = 0, k = 0;
            foreach (var group in nodes)
            {
                columnsNum += group.Count();
            }
            string[] columns = new string[columnsNum];
            for (int i = 0; i < nodes.Count(); i++)
            {
                for (int j = 0; j < nodes[i].Count(); j++)
                {
                    columns[k] += nodes[i][j];
                    k++;
                }
            }
            string[,] newTable = new string[sizes[0] + 1, columnsNum];
            k = 0;
            for (int i = 0; i < columnsNum; i++)
            {
                newTable[0, i] += columns[i].Split("/")[1];
            }
            for (int i = 0; i < nodes.Count(); i++)
            {
                for (int j = 0; j < nodes[i].Count(); j++)
                {
                    for (int t = 0; t < sizes[0]; t++)
                    {
                        newTable[t + 1, k] += Array.IndexOf(columns, table[t, i]);
                    }
                    k++;
                }
            }
            for (int i = 0; i < sizes[0] + 1; i++)
            {
                for (int j = 0; j < columnsNum; j++)
                {
                    Console.Write(newTable[i, j] + " ");
                }
                Console.WriteLine();
            }
            return newTable;
        }

        static int[] LineExtractor(ref string[,] table, ref int[] sizes, ref string[,] origin)
        {
            int[] outSignals = new int[sizes[2]];
            string[,] temp = new string[sizes[0], sizes[2]];
            for (int i = 0; i < sizes[2]; i++)
            {
                outSignals[i] = int.Parse(table[0, i]);
            }
            for (int i = 0; i < sizes[0]; i++)
            {
                for (int j = 0; j < sizes[2]; j++)
                {
                    table[i, j] = table[i + 1, j];
                    origin[i, j] = origin[i + 1, j];
                }
            }
            return outSignals;
        }

        static List<List<int>> Grouping(ref int[] sizes, int[] outSignals)
        {
            List<List<int>> multitudes = new List<List<int>>();
            for (int i = 0; i < sizes[2]; i++)
            {
                if (multitudes.Count == 0)
                {
                    List<int> multitudes2 = new List<int>
                    {
                        outSignals[0]
                    };
                    multitudes.Add(multitudes2);
                }
                else
                {
                    bool found = false;
                    for (int j = 0; j < multitudes.Count; j++)
                    {
                        if (outSignals[multitudes[j][0]] == outSignals[i])
                        {
                            multitudes[j].Add(i);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        List<int> multitudes2 = new List<int>
                        {
                            i
                        };
                        multitudes.Add(multitudes2);
                    }
                }
            }
            return multitudes;
        }

        static List<List<int>> Division(ref string[,] table, ref int[] sizes, ref List<List<int>> oldMultitudes)
        {
            string[] groups = new string[sizes[2]];
            List<List<int>> multitudes = new List<List<int>>();
            if (steps == 0)
            {
                for (int i = 0; i < sizes[2]; i++)
                {
                    for (int j = 0; j < sizes[0]; j++)
                    {
                        groups[i] += table[j, i].Split("/")[1];
                    }
                }
            }
            else
            {
                for (int i = 0; i < sizes[2]; i++)
                {
                    for (int j = 0; j < sizes[0]; j++)
                    {
                        groups[i] += table[j, i];
                    }
                }
            }

            for (int i = 0; i < sizes[2]; i++)
            {
                if (multitudes.Count == 0)
                {
                    List<int> multitudes2 = new List<int>
                    {
                        i
                    };
                    multitudes.Add(multitudes2);
                }
                else
                {
                    if (steps == 0)
                    {
                        bool found = false;
                        for (int j = 0; j < multitudes.Count; j++)
                        {

                            if (groups[i] == groups[multitudes[j][0]])
                            {
                                multitudes[j].Add(i);
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            List<int> multitudes2 = new List<int>
                            {
                                i
                            };
                            multitudes.Add(multitudes2);
                        }
                    }
                    else
                    {
                        bool found = false;
                        int j = 0;
                        foreach (var node in multitudes)
                        {
                            if (groups[i] == groups[node[0]] && Search(oldMultitudes, i) == Search(oldMultitudes, node[0]))
                            {
                                multitudes[j].Add(i);
                                found = true;
                            }
                            j++;
                        }
                        if (!found)
                        {
                            List<int> multitudes2 = new List<int>
                            {
                                i
                            };
                            multitudes.Add(multitudes2);
                        }
                    }
                }
            }
            return multitudes;
        }

        static int Search(List<List<int>> oldMultitudes, int i)
        {
            for (int j = 0; j < oldMultitudes.Count; j++)
            {
                if (oldMultitudes[j].Contains(i))
                {
                    return j;
                }
            }
            return -1;
        }
        static string[,] UpdateTable(ref string[,] table, List<List<int>> multitudes, int[] sizes, string[,] origin)
        {
            for (int i = 0; i < sizes[0]; i++)
            {
                for (int j = 0; j < sizes[2]; j++)
                {
                    for (int z = 0; z < multitudes.Count; z++)
                    {
                        if (multitudes[z].Contains(int.Parse(origin[i, j].Split("/")[0])))
                        {
                            table[i, j] = z.ToString();
                        }
                    }
                }
            }
            return table;
        }

        static bool Compare(List<List<int>> multitudes, List<List<int>> oldMultitudes)
        {
            if (multitudes.Count != oldMultitudes.Count)
            {
                return false;
            }
            for (int i = 0; i < multitudes.Count; i++)
            {
                if (multitudes[i].Count != oldMultitudes[i].Count)
                {
                    return false;
                }
                for (int j = 0; j < multitudes[i].Count; j++)
                {
                    if (multitudes[i][j] != oldMultitudes[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static string[,] Transfering(int[] sizes, string[,] table)
        {
            string[,] newTable;
            if (sizes[3] == 1)
            {
                newTable = MealyToMoore(table, sizes);
            }
            else
            {
                newTable = MooreToMealy(table, sizes);
            }
            return newTable;
        }

        static void Minimization(ref string[,] table, int[] sizes)
        {
            string[,] origin = table.Clone() as string[,];
            List<List<int>> multitudes = new List<List<int>>();
            List<List<int>> oldMultitudes = new List<List<int>>();
            int[] outSignals = new int[0];
            while (!Compare(multitudes, oldMultitudes) || steps == 0)
            {
                oldMultitudes = multitudes;
                if (sizes[3] == 2 && steps == 0)
                {
                    outSignals = LineExtractor(ref table, ref sizes, ref origin);
                    multitudes = Grouping(ref sizes, outSignals);
                }
                else
                {
                    multitudes = Division(ref table, ref sizes, ref oldMultitudes);
                }
                UpdateTable(ref table, multitudes, sizes, origin);
                steps++;
            }
            string[,] result = Printing(table, origin, multitudes, outSignals, sizes);
        }

        static string[,] Printing(string[,] table, string[,] origin, List<List<int>> multitudes, int[] outSignals, int[] sizes)
        {
            string[,] result = new string[sizes[0], multitudes.Count];
            if (sizes[3] == 1)
            {
                for (int j = 0; j < multitudes.Count; j++)
                {
                    Console.Write("S" + j + "   ");
                }
                Console.WriteLine();
                for (int i = 0; i < multitudes.Count; i++)
                {
                    Console.Write("----");
                }
                Console.WriteLine();
                for (int i = 0; i < sizes[0]; i++)
                {
                    for (int j = 0; j < multitudes.Count; j++)
                    {
                        result[i, j] += "S" + table[i, multitudes[j][0]] + "/" + origin[i, multitudes[j][0]].Split("/")[1] + " ";
                        Console.Write(result[i, j]);
                    }
                    Console.WriteLine();
                }
                for (int i = 0; i < multitudes.Count; i++)
                {
                    Console.Write("S" + i + " = {");
                    for (int j = 0; j < multitudes[i].Count; j++)
                    {
                        if (j < multitudes[i].Count - 1)
                        {
                            Console.Write(multitudes[i][j] + ",");
                        }
                        else
                        {
                            Console.Write(multitudes[i][j]);
                        }
                    }
                    Console.WriteLine("}");
                }
            }
            if (sizes[3] == 2)
            {
                Console.WriteLine();
                for (int j = 0; j < multitudes.Count; j++)
                {
                    Console.Write("S" + j + " ");
                }
                Console.WriteLine();
                for (int i = 0; i < multitudes.Count; i++)
                {
                    Console.Write("---");
                }
                Console.WriteLine();
                for (int i = 0; i < multitudes.Count; i++)
                {
                    string head = "Y" + outSignals[multitudes[i][0]] + " ";
                    Console.Write(head);
                }
                Console.WriteLine();
                for (int i = 0; i < sizes[0]; i++)
                {
                    for (int j = 0; j < multitudes.Count; j++)
                    {
                        result[i, j] += "S" + table[i, multitudes[j][0]] + " ";
                        Console.Write(result[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                for (int i = 0; i < multitudes.Count; i++)
                {
                    Console.Write("S" + i + " = {");
                    for (int j = 0; j < multitudes[i].Count; j++)
                    {
                        if (j < multitudes[i].Count - 1)
                        {
                            Console.Write(multitudes[i][j] + ",");
                        }
                        else
                        {
                            Console.Write(multitudes[i][j]);
                        }
                    }
                    Console.WriteLine("}");
                }
                Console.WriteLine();
            }
            return result;
        }

        static void Interface()
        {
            Console.WriteLine("Введите имя файла");
            string fileName = "../../../" +  Console.ReadLine();
            int[] sizes = new int[4];
            string[,] table = CreateTable(ref sizes, fileName);
            int key = 0;
            while (key != 4)
            {
                Console.WriteLine("Что Вы хотите сделать:\n\t1)Минимизировать автомат\n\t2)Пeревод из одного автомата в другой\n\t3)Выбрать новый файл\n\t4)Выйти");
                key = int.Parse(Console.ReadLine());
                if (key == 1)
                {
                    Minimization(ref table, sizes);
                }
                if (key == 2)
                {
                    table = Transfering(sizes, table);
                }
                if (key == 3)
                {
                    Console.WriteLine("Введите имя файла");
                    fileName = Console.ReadLine();
                    sizes = new int[4];
                    table = CreateTable(ref sizes, fileName);
                }
            }
        }

        static void Main(string[] args)
        {
            Interface();
        }
    }
}
