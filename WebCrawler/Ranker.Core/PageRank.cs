using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranking.Core
{
    public class PageRank
    {
        private const int N = 10;
        private float[][] matrix;

        public PageRank()
        {
            float alpha = 0.1f;

            // Initialize array
            float[][] teleport = createMatrix();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    teleport[i][j] = 0.1f;
                }
            }
            teleport = multiplyMatrix(teleport, alpha);

            matrix = createMatrix();
            matrix[0][5] += 1.1f;
            matrix[2][5] += 0.6f;
            matrix[2][6] += 0.6f;
            matrix[4][5] += 1.1f;
            matrix[5][1] += 0.4f + (1.0f / 3);
            matrix[5][4] += 0.4f + (1.0f / 3);
            matrix[5][8] += 0.4f + (1.0f / 3);
            matrix[6][3] += 0.6f;
            matrix[6][5] += 0.6f;
            matrix[8][9] += 1.1f;
            matrix[9][7] += 1.1f;
            matrix = multiplyMatrix(matrix, (1 - alpha));
            matrix = addMatrix(matrix, teleport);
        }
        private float[][] createMatrix()
        {
            float[][] result = new float[N][];
            for (int i = 0; i < N; i++)
            {
                result[i] = new float[N];
            }
            return result;
        }

        private float[][] addMatrix(float[][] m1, float[][] m2)
        {
            for (int i = 0; i < m1.Length; i++)
            {
                for (int j = 0; j < m1.Length; j++)
                {
                    m1[i][j] += m2[i][j];
                }
            }
            return m1;
        }

        private float[][] multiplyMatrix(float[][] m, float value)
        {
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m[i][j] *= value;
                }
            }
            return m;
        }

        public float[] DoRank()
        {
            float[] vector = { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            return DoRankHelper(vector);
        }

        private float[] DoRankHelper(float[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write(vector[i] + " ");
            }
            Console.Write("\n");  

            float[] newVector = CalculateVector(vector);

            if (IsEqual(vector, newVector))
            {
                return newVector;
            }

            return DoRankHelper(newVector);
        }

        private bool IsEqual(float[] vec1, float[] vec2)
        {
            int length = vec1.Length;

            for (int i = 0; i < length; i++)
            {
                float diff = vec1[i] - vec2[i];
                if (Math.Abs(diff) > .00001f)
                {
                    return false;
                }
            }
            return true;
        }

        private float[] CalculateVector(float[] vector)
        {
            float[] p = new float[vector.Length];

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    p[i] += matrix[j][i]*vector[j];
                }
            }

            return p;
        }


    }
}
