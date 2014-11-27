using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Recommender
{
    class Preprocesser
    {
        private SparseMatrix ratingMatrix;
        private Dictionary<int, int> userMap = new Dictionary<int, int>();
        private Dictionary<int, int> movieMap = new Dictionary<int, int>();

        private SparseMatrix InitMatrix(List<Movie> movies)
        {
            int userIndex = 0;
            foreach (var movie in movies)
            {
                foreach (var rating in movie.Ratings)
                {
                    if (!userMap.ContainsKey(rating.Key))
                        userMap.Add(rating.Key, userIndex++);
                }
            }

            return new SparseMatrix(movies.Count, userMap.Count);
        }

        public void BuildRatingMatrix(List<Movie> movies)
        {
            ratingMatrix = InitMatrix(movies);

            for (int i = 0; i < movies.Count; i++)
            {
                Dictionary<int, int> ratings = movies[i].Ratings;
                foreach (var rating in ratings)
                {
                    ratingMatrix[i, userMap[rating.Key]] = rating.Value;
                }
                movieMap.Add(movies[i].Id, i);
            }
        }

        public SparseMatrix GetMeanMatrix()
        {
            SparseMatrix meanMatix = new SparseMatrix(ratingMatrix.RowCount, ratingMatrix.ColumnCount);

            double totalMean = ratingMatrix.SumMatrix() * (1 / (double)ratingMatrix.NonZerosCount);

            SparseVector movieMeans = ratingMatrix.MeanRowVector();
            SparseVector userMeans = ratingMatrix.MeanColumnVector();

            for (int i = 0; i < meanMatix.RowCount; i++)
            {
                for (int j = 0; j < meanMatix.ColumnCount; j++)
                {
                    double cell = ratingMatrix[i, j];
                    if (cell > 0 || cell < 0)
                        meanMatix[i, j] = ratingMatrix[i, j] - movieMeans[i] - userMeans[j] + totalMean;
                }
            }

            return meanMatix;
        }
    }
}
