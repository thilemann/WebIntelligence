using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Providers.LinearAlgebra.Mkl;
using WebIntelligence2;

namespace SocialMediaAnalysis
{
    class CommunityIdentifier
    {
        private const string EigenValueMatrixFile = "Resources/eigenValueMatrix.csv";

        private List<KeyValuePair<int, double>> _eigenvector;
        private Community _rootCommunity;

        public CommunityIdentifier(Community community)
        {
            _rootCommunity = community;
        }

        private List<KeyValuePair<int, double>> CalculateEigenvector()
        {
            Vector<double> eigenValue = getEigenvectors();
            return SortEigenvector(eigenValue);
        }

        private Vector<double> getEigenvectors()
        {
            DateTime start = DateTime.Now;
            Control.UseNativeMKL();
            Matrix<double> adjacencyMatrix = _rootCommunity.AdjacencyMatrix;
            Console.WriteLine("Adjacency: " + DateTime.Now.Subtract(start).ToString("g"));
            Matrix<double> diagonalMatrix = MatrixHelper.calculateDiagonal(adjacencyMatrix);
            Console.WriteLine("diagonal: " + DateTime.Now.Subtract(start).ToString("g"));
            Matrix<double> numericsLaplacianMatrix = MatrixHelper.calculateLaplacian(adjacencyMatrix, diagonalMatrix);
            Console.WriteLine("laplacian: " + DateTime.Now.Subtract(start).ToString("g"));
            MathNet.Numerics.LinearAlgebra.Factorization.Evd<double> numericsEigen = numericsLaplacianMatrix.Evd(Symmetricity.Symmetric);
            Console.WriteLine("eigenvector: " + DateTime.Now.Subtract(start).ToString("g"));

            //using (StreamWriter writer = new StreamWriter("eigen.csv"))
            //{
            //    for (int i = 0; i < numericsEigen.EigenVectors.ColumnCount; i++)
            //    {
            //        writer.Write(i +";");
            //    }
            //    writer.Write("\n");
            //    for (int i = 0; i < numericsEigen.EigenVectors.RowCount; i++)
            //    {
            //        writer.WriteLine(string.Join(";", numericsEigen.EigenVectors.Row(i)));
            //    }
            //}

            return numericsEigen.EigenVectors.Column(1);
        }

        public List<Community> Identify(double lastDiff = double.MinValue)
        {
            List<Community> communities = new List<Community>();
            if (_rootCommunity.Users.Count < 2)
                return communities;

            Console.WriteLine("Root community size: {0}", _rootCommunity.Users.Count);

            _eigenvector = CalculateEigenvector();
            //Console.WriteLine("Eigenvector:\n\t" + string.Join("\n\t", _eigenvector));

            double largestDiff = lastDiff;
            int nodeIndexBeforeDiff = -1;
            for (int i = 1; i < _eigenvector.Count; i++)
            {
                double diff = Math.Abs(_eigenvector[i].Value - _eigenvector[i - 1].Value);
                if (diff > largestDiff)
                {
                    nodeIndexBeforeDiff = i;
                    largestDiff = diff;
                }
            }

            Console.WriteLine("Largest diff: " + largestDiff);

            if (nodeIndexBeforeDiff > -1) // We did discover subcommunities
            {
                Community community1 = new Community();
                for (int i = 0; i < nodeIndexBeforeDiff; i++)
                {
                    User user = _rootCommunity.Users[_eigenvector[i].Key];
                    community1.Users.Add(user);
                }
                Community community2 = new Community();
                for (int i = nodeIndexBeforeDiff; i < _eigenvector.Count; i++)
                {
                    User user = _rootCommunity.Users[_eigenvector[i].Key];
                    community2.Users.Add(user);
                }

                Console.WriteLine("Community1: " + community1.Size);
                Console.WriteLine("Community2: " + community2.Size);
                Console.WriteLine();

                if (community1.Size > 1 && community2.Size > 1)
                {
                    lastDiff = largestDiff*0.6;
                    CommunityIdentifier identifier1 = new CommunityIdentifier(community1);
                    List<Community> communities1 = identifier1.Identify(lastDiff);
                    if (communities1.Count > 0)
                        communities.AddRange(communities1);
                    else
                        communities.Add(community1);

                    CommunityIdentifier identifier2 = new CommunityIdentifier(community2);
                    List<Community> communities2 = identifier2.Identify(lastDiff);
                    if (communities2.Count > 0)
                        communities.AddRange(communities2);
                    else
                        communities.Add(community2);
                }
            }
            Console.WriteLine("Communities: {0}\n", communities.Count);
            return communities;
        }

        private List<KeyValuePair<int, double>> SortEigenvector(Vector<double> eigenVector)
        {
            SortedList<int, double> eigenvector = new SortedList<int, double>();
            for (int i = 0; i < eigenVector.Count; i++)
            {
                eigenvector.Add(i, eigenVector[i]);
            }
            
            return eigenvector.OrderBy(kvp => kvp.Value).ToList();
        }
    }
}
