using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace WebIntelligence2
{
    class Laplacian
    {

        public double[,] calculateLaplacian(double[,] adjacency)
        {
            double[,] result = Matrix.Create<double>(adjacency.GetLength(0), adjacency.GetLength(1), 0);
            double[,] diagonal = calculateDiagonal(adjacency);
            result = diagonal.Subtract(adjacency);
            return result;
        }

        public double[,] calculateDiagonal(double[,] matrix)
        {
            double[,] diagonal = Matrix.Create<double>(matrix.GetLength(0), matrix.GetLength(1), 0);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double sum = 0;
                double[] row = matrix.GetRow(i);
                for (int j = 0; j < row.Length; j++)
                {
                    sum += row[j];
                }
                diagonal[i, i] = sum;
            }
            return diagonal;
        }

        public double[] getEigenvectors(double[,] matrix)
        {
            return matrix.GetColumn(1);
        }

        public List<double[,]> FindComunities(double[,] matrix)
        {
            return null;
        }
    }

    
}
