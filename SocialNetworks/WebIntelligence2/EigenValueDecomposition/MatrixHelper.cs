using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace SocialMediaAnalysis
{
    class MatrixHelper
    {

        public static Matrix<double> calculateLaplacian(Matrix<double> adjacency, Matrix<double> diagonal)
        {
            return diagonal.Subtract(adjacency);
        }

        private static Matrix<double> createMatrixOfSize(Matrix<double> matrix)
        {
            return Matrix<double>.Build.Dense(matrix.RowCount, matrix.ColumnCount, 0);
        }

        public static Matrix<double> calculateDiagonal(Matrix<double> matrix)
        {
            Matrix<double> diagonal = createMatrixOfSize(matrix);
            Vector<double> row = matrix.RowSums();
            for (int i = 0; i < matrix.RowCount; i++)
            {
                diagonal[i, i] = row.At(i);
            }
            return diagonal;
        }
    }

    
}
