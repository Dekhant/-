using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System;
using System.Windows.Forms;
using NKR;
using System.Collections.Generic;
using System.IO;

namespace NKRVisualization
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var form = new Form();
            var viewer = new GViewer();
            var graph = new Graph("K/DA");
            int[] sizes = new int[4];
            string fileName = "../../../input.txt";
            string outputName = "../../../output.txt";
            List<int>[,] table = NKR.Program.CreateTable(ref sizes, fileName);
            HashSet<List<int>> Nodes = new HashSet<List<int>>
            {
                new List<int>(1) { 0 }
            };
            List<List<int>>[] newTable = NKR.Program.Determinization(ref table, sizes, ref Nodes);
            int iter = 0;
            foreach(var i in Nodes)
            {
                string node1 = "";
                foreach (var j in i)
                {
                    node1 += j.ToString();
                }
                for (int j = 0; j < sizes[0]; j++)
                {
                    string node2 = "";
                    for (int k = 0; k < newTable[j][iter].Count; k++)
                    {
                        node2 += newTable[j][iter][k].ToString();
                    }
                    graph.AddEdge(node1, j.ToString(), node2);

                }
                iter++;

            }
            using var output = new StreamWriter(outputName);
            char ch = 'A';
            foreach (var i in Nodes)
            {
                output.Write(ch + "{");
                foreach (var j in i)
                {
                    output.Write(j.ToString());
                }
                output.Write("}");
                ch++;
                output.Write("\t");
            }
            output.WriteLine();
            for (int i = 0; i < sizes[0]; i++)
            {
                for (int j = 0; j < newTable[i].Count; j++)
                {
                    for (int k = 0; k < newTable[i][j].Count; k++)
                    {

                        output.Write(newTable[i][j][k].ToString());
                    }
                    output.Write("\t");
                }
                output.WriteLine();
            }
            viewer.Graph = graph;
            form.SuspendLayout();
            viewer.Dock = DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            form.ShowDialog();
        }
    }
}
