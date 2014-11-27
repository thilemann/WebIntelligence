using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Recommender
{
    class ProbeParser
    {
        private const int NOLIMIT = -1;


        public static List<Movie> Parse(string file, int limit = NOLIMIT)
        {
            if (!File.Exists(file)) return null;

            List<Movie> movies = new List<Movie>();
            
            int movieId = default(int);
            List<int> userIds = new List<int>();
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    if (limit > -1 && movies.Count == limit)
                        break;

                    string line = reader.ReadLine();
                    if (line.Contains(":"))
                    {
                        if (movieId != default(int))
                        {
                            Movie movie = new Movie(movieId);
                            foreach (var userId in userIds)
                            {
                                movie.AddOrUpdate(userId, 0);
                            }
                            movies.Add(movie);
                            userIds = new List<int>();
                        }
                        movieId = Int32.Parse(line.Replace(":", string.Empty));
                    }
                    else
                    {

                        int userId = Int32.Parse(line);
                        userIds.Add(userId);
                    }
                }
            }

            return movies;
        }
    }
}
