using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace Recommender
{
    class Factorization
    {
        private const int K = 25;
        private const double n = 0.001;
        private int movieCount;
        private int userCount;

        DenseMatrix A;
        DenseMatrix B;

        Matrix<double> convergedMovie;
        Matrix<double> convergedUser;

        public void Factorize(Matrix meanMatrix)
        {
            int count = 0;
            movieCount = meanMatrix.RowCount;
            userCount = meanMatrix.ColumnCount;
            convergedMovie = DenseMatrix.Build.Dense(K, movieCount, 0);
            convergedUser = DenseMatrix.Build.Dense(userCount, K, 0);

            A = new DenseMatrix(DenseColumnMajorMatrixStorage<double>.OfMatrix(DenseMatrix.Build.Dense(K, movieCount, 0.1).Storage));
            B = new DenseMatrix(DenseColumnMajorMatrixStorage<double>.OfMatrix(DenseMatrix.Build.Dense(userCount, K, 0.1).Storage));

            while (count < 50)
            {
                for (int k = 0; k < K; k++)
                {
                    for (int m = 0; m < movieCount; m++)
                    {
                        for (int u = 0; u < userCount; u++)
                        {
                            train(k, u, m, meanMatrix[m, u]);
                        }
                    }
                }
                Console.WriteLine(count++);
                if (IsConverged())
                    break;
            }
        }

        public void Evaluate(Matrix meanMatrix)
        {
            
        }

        private bool IsConverged()
        {
            bool result = ((int)convergedMovie.ColumnSums().Sum()) == convergedMovie.RowCount*convergedMovie.ColumnCount;
            if (result && ((int)convergedUser.ColumnSums().Sum()) == convergedUser.RowCount * convergedUser.ColumnCount)
                return true;

            convergedMovie = DenseMatrix.Build.Dense(K, movieCount, 0);
            convergedUser = DenseMatrix.Build.Dense(userCount, K, 0);

            return false;
        }



        private void train(int k, int u, int m, double rating)
        {
            double err = n * (rating - SumAmiBiu(m, u));
            double prevB = B[u, k];
            double prevA = A[k, m];
            B[u, k] += err * A[k, m];
            A[k, m] += err * prevB;
            if (Math.Abs(prevB - B[u, k]) / prevB < 0.0001)
                convergedUser[u, k] = 1;
            if (Math.Abs(prevA - A[k, m]) / prevA < 0.0001)
                convergedMovie[k, m] = 1;
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
