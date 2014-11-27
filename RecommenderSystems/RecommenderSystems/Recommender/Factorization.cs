using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;
using DenseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;

namespace Recommender
{
    class Factorization
    {
        private const int K = 25;
        private const double n = 0.001;

        DenseMatrix A;
        DenseMatrix B;

        public void Factorize(Matrix meanMatrix)
        {
            int count = 0;
            int movieCount = meanMatrix.RowCount;
            int userCount = meanMatrix.ColumnCount;

            A = new DenseMatrix(K, movieCount, 0.1);
            B = new DenseMatrix(userCount, K, 0.1);

            while (count < 10)
            {
                bool shouldBreak = false;
                for (int k = 0; k < K; k++)
                {
                    for (int m = 0; m < movieCount; m++)
                    {
                        for (int u = 0; u < userCount; u++)
                        {
                            shouldBreak = train(k, u, m, meanMatrix[m, u]);
                        }
                    }
                }
                Console.WriteLine(count++);
                if (shouldBreak)
                    break;
            }
        }

        private bool train(int k, int u, int m, double rating)
        {
            bool different = true;
            double err = n * (rating - SumAmiBiu(m, u));
            double temp = B[u, k];
            B[u, k] += err * A[k, m];
            A[k, m] += err*temp;
            if (Math.Abs(temp - B[u, k])/temp < 0.05)
                different = false;

            return different;
        }

        private double SumAmiBiu(int m, int u)
        {
            double sum = 0;
            for (int i = 0; i < K; i++)
            {
                sum += A[i, m]*B[u, i];
            }
            return sum;
        }

    }
}
