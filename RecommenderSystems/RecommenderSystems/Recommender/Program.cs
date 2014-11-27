using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Recommender
{
    class Program
    {
        static void Main(string[] args)
        {
            bool readFromFile = true;

            if (!readFromFile)
            {
                Dictionary<int, Movie> allTestMovies = MovieParser.ParseFromFolder(@"C:\Users\Frederik\Desktop\download\download\training_set\training_set", 1000);
                List<Movie> rawProbes = ProbeParser.Parse(@"C:\Users\Frederik\Desktop\download\download\probe.txt", 1000);

                List<Movie> tempProbes = new List<Movie>();
                foreach (var probe in rawProbes)
                {
                    if (allTestMovies.ContainsKey(probe.Id))
                    {
                        Movie movie = new Movie(probe.Id);
                        foreach (var userId in probe.Ratings.Keys)
                        {
                            int rating = allTestMovies[probe.Id].GetRating(userId);
                            movie.AddOrUpdate(userId, rating);
                            allTestMovies[probe.Id].RemoveRating(userId);
                        }
                        tempProbes.Add(movie);
                    }
                }
                rawProbes = null;
                GC.Collect();
                //WriteListToFile("testMovies.txt", allTestMovies.Values.ToList());
                allTestMovies = null;
                GC.Collect();
                WriteListToFile("probeMovie.txt", tempProbes);
                tempProbes = null;
                GC.Collect();
            }

            //List<Movie> testMovies = MovieParser.Parse("testMovies.txt");
            List<Movie> probes = MovieParser.Parse("probeMovie.txt");

            Preprocesser preprocesser = new Preprocesser();
            preprocesser.BuildRatingMatrix(probes);
            SparseMatrix matrix = preprocesser.GetMeanMatrix();
            Factorization factorization = new Factorization();
            factorization.Factorize(matrix);
            //double sum = matrix.SumMatrix();
            //Console.WriteLine(sum);


            Console.ReadLine();
        }

        private static void WriteListToFile(string file, IEnumerable<Movie> movies)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                foreach (var movie in movies)
                {
                    writer.WriteLine("{0}:", movie.Id);
                    foreach (var rating in movie.Ratings)
                    {
                        writer.WriteLine("{0},{1}", rating.Key, rating.Value);
                    }
                }
            }
        }
    }
}
