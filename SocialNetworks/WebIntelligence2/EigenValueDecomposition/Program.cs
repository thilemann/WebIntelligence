using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Accord.Math;
using Accord.Math.Decompositions;
using EigenValueDecomposition;

namespace WebIntelligence2
{
    class Program
    {
        static void Main(string[] args)
        {

            DateTime start = DateTime.Now;
            //FriendListParser parser = new FriendListParser("Resources/friendships.txt");
            //parser.Parse();

            //double[,] adjacency = parser.GetAdjacencyMatrix();
            //Console.WriteLine("Finished adjacency: " + DateTime.Now.Subtract(start).ToString(@"hh\:mm\:ss"));

            //Laplacian laplacian = new Laplacian();
            //double[,] l = laplacian.calculateLaplacian(adjacency);
            //Console.WriteLine("Finished laplacian: " + DateTime.Now.Subtract(start).ToString(@"hh\:mm\:ss"));
            //EigenvalueDecomposition eigenValue = new EigenvalueDecomposition(l);
            //double[,] eigenValueMatrix = eigenValue.Eigenvectors;
            //Console.WriteLine("Finished eigenvalue: " + DateTime.Now.Subtract(start).ToString(@"hh\:mm\:ss"));

            //PrintMatrix(eigenValueMatrix);
            //Console.WriteLine("Finished everything. Press to exit");
            //Console.ReadLine();
            ReadMatrix("Resources/eigenValueMatrix.csv");
            Console.WriteLine("Finished loading: " + DateTime.Now.Subtract(start).ToString(@"hh\:mm\:ss"));

            //PrintMatrix(adjacency);
            //PrintMatrix(l);
            //PrintMatrix(eigenValueMatrix);

        }

        private static void PrintMatrix(double[,] matrix)
        {
            using (StreamWriter writer = new StreamWriter("Resources/eigenValueMatrix.csv"))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    string line = string.Join(";", matrix.GetRow(i).ToList());
                    writer.WriteLine(line);
                }
            }
        }

        private static double[,] ReadMatrix(string filepath)
        {
            double[,] matrix = null;
            using (StreamReader reader = new StreamReader(filepath))
            {
                int rowIndex = 0;
                while (!reader.EndOfStream)
                {
                    string[] cells = reader.ReadLine().Split(';');
                    int length = cells.Length;
                    if (matrix == null)
                        matrix = Matrix.Create<double>(length, length, 0);

                    double[] row = new double[length];
                    for (int i = 0; i < length; i++)
                    {
                        row[i] = double.Parse(cells[i]);
                    }
                    matrix.SetRow(rowIndex++, row);
                }
            }
            return matrix;
        }

        private static double[,] InitMatrix()
        {
            double[,] adjacency = Matrix.Create<double>(9, 9, 0);
            adjacency[0, 1] = 1;
            adjacency[0, 2] = 1;
            adjacency[1, 0] = 1;
            adjacency[1, 2] = 1;
            adjacency[2, 0] = 1;
            adjacency[2, 1] = 1;
            adjacency[2, 3] = 1;
            adjacency[2, 4] = 1;
            adjacency[3, 2] = 1;
            adjacency[3, 4] = 1;
            adjacency[3, 5] = 1;
            adjacency[3, 6] = 1;
            adjacency[4, 2] = 1;
            adjacency[4, 3] = 1;
            adjacency[4, 5] = 1;
            adjacency[4, 6] = 1;
            adjacency[5, 3] = 1;
            adjacency[5, 4] = 1;
            adjacency[5, 6] = 1;
            adjacency[5, 7] = 1;
            adjacency[6, 3] = 1;
            adjacency[6, 4] = 1;
            adjacency[6, 5] = 1;
            adjacency[6, 7] = 1;
            adjacency[7, 5] = 1;
            adjacency[7, 6] = 1;
            adjacency[7, 8] = 1;
            adjacency[8, 7] = 1;
            return adjacency;
        }
    }
}
