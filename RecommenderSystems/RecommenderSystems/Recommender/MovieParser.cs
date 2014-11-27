using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommender
{
    public static class MovieParser
    {
        private const int NOLIMIT = -1;

        public static Dictionary<int, Movie> ParseFromFolder(string folder, int limit = NOLIMIT)
        {
            if (!Directory.Exists(folder)) return null;

            Dictionary<int, Movie> movies = new Dictionary<int, Movie>();
            foreach (var file in Directory.EnumerateFiles(folder))
            {
                if (limit > -1 && movies.Count == limit)
                    break;

                string[] lines = File.ReadAllLines(file);
                int movieId = Int32.Parse(lines[0].Replace(":", string.Empty));
                Movie movie = new Movie(movieId);
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');
                    int userId = Int32.Parse(fields[0]);
                    int rating = Int32.Parse(fields[1]);
                    movie.AddOrUpdate(userId, rating);
                }
                movies.Add(movieId, movie);
            }

            return movies;
        }

        public static List<Movie> Parse(string file, int limit = NOLIMIT)
        {
            if (!File.Exists(file)) return null;

            List<Movie> movies = new List<Movie>();

            int previousId = default(int);
            int movieId = default(int);
            Dictionary<int, int> ratings = new Dictionary<int, int>();
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.Contains(":"))
                    {
                        if (movieId != previousId)
                        {
                            Movie movie = new Movie(movieId);
                            movie.AddOrUpdate(ratings);
                            movies.Add(movie);
                            previousId = movieId;
                            ratings = new Dictionary<int, int>();
                        }
                        movieId = Int32.Parse(line.Replace(":", string.Empty));
                    }
                    else
                    {
                        string[] fields = line.Split(',');
                        int userId = Int32.Parse(fields[0]);
                        int rating = Int32.Parse(fields[1]);
                        ratings.Add(userId, rating);
                    }
                }
                Movie m = new Movie(movieId);
                m.AddOrUpdate(ratings);
                movies.Add(m);
            }

            return movies;
        }
    }
}
