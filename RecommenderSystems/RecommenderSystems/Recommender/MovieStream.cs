using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommender
{
    internal class MovieStream : IDisposable
    {
        private StreamReader _streamReader;

        public MovieStream(StreamReader streamReader)
        {
            _streamReader = streamReader;
        }

        /*public Movie ReadMovie()
        {
            Movie movie;
            int movieId = default(int);
            Dictionary<int, int> ratings = new Dictionary<int, int>();
            if (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Contains(":"))
                {
                    if (movieId != default(int))
                    {
                        Movie movie = new Movie(movieId);
                        foreach (var rating in ratings)
                        {
                            movie.AddOrUpdate(rating.Key, rating.Value);
                        }
                        movies.Add(movie);
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
            return 
        }*/


        public void Dispose()
        {
            if (_streamReader != null)
            {
                _streamReader.Close();
                _streamReader.Dispose();
            }
        }
    }
    
}
