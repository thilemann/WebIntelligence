using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Recommender
{
    public static class MathHelper
    {
        public static double SumMatrix(this SparseMatrix matrix)
        {
            double sum = 0;
            for (int i = 0; i < matrix.RowCount; i++)
            {
                sum += matrix.Row(i).Sum();
            }
            return sum;
        }

        public static SparseVector MeanColumnVector(this Matrix<double> matrix)
        {
            SparseVector meanVector = new SparseVector(matrix.ColumnCount);
            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                SparseVector column = new SparseVector(matrix.Column(i));
                double elements = column.NonZerosCount;
                if (elements > 0)
                {
                    double sum = column.Sum();
                    meanVector[i] = sum*(1/elements);
                }
                else
                {
                    meanVector[i] = 0;
                }

            }
            return meanVector;
        }

        public static SparseVector MeanRowVector(this Matrix<double> matrix)
        {
            SparseVector meanVector = new SparseVector(matrix.RowCount);
            for (int i = 0; i < matrix.RowCount; i++)
            {
                SparseVector row = new SparseVector(matrix.Row(i));
                double elements = row.NonZerosCount;
                if (elements > 0)
                {
                    meanVector[i] = row.Sum() * (1 / elements);
                }
                else
                {
                    meanVector[i] = 0;
                }
            }
            return meanVector;
        }
    }
}
