using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommender
{
    public class Movie
    {
        private int _id;
        Dictionary<int, int> _ratings = new Dictionary<int, int>();

        public int Id
        {
            get { return _id; }
        }

        public Dictionary<int, int> Ratings
        {
            get { return _ratings; }
        }

        public Movie(int id)
        {
            _id = id;
        }

        public void AddOrUpdate(int userId, int rating)
        {
            if (_ratings.ContainsKey(userId))
                _ratings[userId] = rating;
            else
                _ratings.Add(userId, rating);
        }

        public void RemoveRating(int userId)
        {
            _ratings.Remove(userId);
        }

        public int GetRating(int userId)
        {
            if (_ratings.ContainsKey(userId))
                return _ratings[userId];

            return 0;
        }

        public void AddOrUpdate(Dictionary<int, int> ratings)
        {
            foreach (var rating in ratings)
            {
                AddOrUpdate(rating.Key, rating.Value);
            }
        }
    }
}
