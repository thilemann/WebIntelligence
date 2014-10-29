using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using Accord.Math.Decompositions;
using WebIntelligence2;

namespace EigenValueDecomposition
{
    class CommunityIdentifier
    {
        private const string EigenValueMatrixFile = "Resources/eigenValueMatrix.csv";

        private List<Community> _communities;
        private double[,] _eigenValueMatrix;
        private double[,] _adjacencyMatrix;
        private List<KeyValuePair<int, double>> _eigenvector;
        private Community _rootCommunity;
        private bool _useEigenMatrixFile;

        public CommunityIdentifier(Community community, bool useEigenMatrixFile = false)
        {
            _rootCommunity = community;
            _communities = new List<Community>();
            _useEigenMatrixFile = useEigenMatrixFile;
        }

        private double[,] CalculateEigenvector()
        {
            double[,] adjacency = _rootCommunity.AdjacencyMatrix;
            Laplacian laplacian = new Laplacian();
            double[,] l = laplacian.calculateLaplacian(adjacency);
            double[,] eigenValueMatrix;
            if (_useEigenMatrixFile)
            {
                eigenValueMatrix = ReadMatrix(EigenValueMatrixFile);
            }
            else
            {
                EigenvalueDecomposition eigenValue = new EigenvalueDecomposition(l);
                eigenValueMatrix = eigenValue.Eigenvectors;
                StoreMatrix(eigenValueMatrix, EigenValueMatrixFile);
            }
            return eigenValueMatrix;
        }

        public List<Community> Identify(double lastDiff = 0)
        {
            List<Community> communities = new List<Community>();
            _eigenValueMatrix = CalculateEigenvector();
            _eigenvector = SortEigenvector();
            Console.WriteLine("Eigenvector:\n\t" + string.Join("\n\t", _eigenvector));

            double largestDiff = double.MinValue;
            int nodeIndexBeforeDiff = -1;
            for (int i = 1; i < _eigenvector.Count; i++)
            {
                double diff = Math.Abs(_eigenvector[i - 1].Value - _eigenvector[i].Value);
                if (diff < lastDiff)
                    break;
                if (diff > largestDiff)
                {
                    nodeIndexBeforeDiff = i;
                    largestDiff = diff;
                }
            }

            if (nodeIndexBeforeDiff > -1) // We did discover subcommunities
            {
                Community community1 = new Community();
                for (int i = 0; i < nodeIndexBeforeDiff; i++)
                {
                    User user = _rootCommunity.Users[_eigenvector[i].Key];
                    community1.Users.Add(user);
                }
                communities.Add(community1);
                Community community2 = new Community();
                for (int i = nodeIndexBeforeDiff; i < _eigenvector.Count; i++)
                {
                    User user = _rootCommunity.Users[_eigenvector[i].Key];
                    community2.Users.Add(user);
                }
                communities.Add(community2);


                // TODO: make this work
                lastDiff = largestDiff * 0.6;
                CommunityIdentifier identifier1 = new CommunityIdentifier(community1);
                identifier1.Identify(lastDiff);
                CommunityIdentifier identifier2 = new CommunityIdentifier(community2);
                identifier2.Identify(lastDiff);
            }
            
            return communities;
        }

        private List<KeyValuePair<int, double>> SortEigenvector()
        {
            SortedList<int, double> eigenvector = new SortedList<int, double>();
            double[] eigenvectorArray = _eigenValueMatrix.GetColumn(1);
            for (int i = 0; i < eigenvectorArray.Length; i++)
            {
                eigenvector.Add(i, eigenvectorArray[i]);
            }
            
            return eigenvector.OrderBy(kvp => kvp.Value).ToList();
        }

        private static void StoreMatrix(double[,] matrix, string outputFile)
        {
            using (StreamWriter writer = new StreamWriter(outputFile))
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
                    var readLine = reader.ReadLine();
                    if (readLine == null)
                        continue;

                    string[] cells = readLine.Split(';');
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
    }
}
