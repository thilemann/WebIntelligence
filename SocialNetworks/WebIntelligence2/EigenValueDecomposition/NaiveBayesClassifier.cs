using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EigenValueDecomposition
{
    class NaiveBayesClassifier
    {
        private List<string> vocabulary = new List<string>();
        List<Review> reviews = new List<Review>();

        public void Parse(string file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string label = null;
                string reviewText = null;
                while (reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (line.Contains("label:"))
                    {
                        label = line.Replace("label:", "").Trim();
                    }
                    else if (line.Contains("reviewText:"))
                    {
                        reviewText = line.Replace("reviewText:", "").Trim();
                        Review review = new Review(label, reviewText);
                        reviews.Add(review);

                        foreach (var item in reviewText.Split(null))
                        {
                            if (!vocabulary.Contains(item))
                                vocabulary.Add(item);
                        }
                        label = null;
                        reviewText = null;
                    }
                }
            }
        }

        public double logS(Review review, Label rating)
        {
            double summation = 0;
            string[] words = review.ReviewText.Split(null);
            for (int i = 0; i < words.Length; i++)
            {
                summation += p(words[i], rating);
            }
            return Math.Log(p(rating)) + summation;
        }

        public double S(Review review, Label rating)
        {
            double result = 1;
            string[] words = review.ReviewText.Split(null);
            for (int j = 0; j < words.Length; j++)
            {
                double pValue = p(words[j], rating);
                result *= pValue / (1 - pValue);
            }
            return SStarEmpty(rating)*result;
        }

        public double SStarEmpty(Label rating)
        {
            double result = 1;
            for (int i = 0; i < vocabulary.Count; i++)
            {
                result *= NotP(vocabulary[i], rating);
            }
            return result*p(rating);
        }

        public double p(Label rating)
        {
            double count = N(rating);

            return count / reviews.Count;
        }

        public double p(string word, Label rating)
        {
            double count = N(word, rating);
            return count/reviews.Count;
        }

        public double NotP(string word, Label rating)
        {
            return 1 - p(word, rating);
        }
        
        public double N(Label rating)
        {
            int count = 0;
            foreach (var review in reviews)
            {
                if (review.Rating == rating)
                    count++;
            }
            return count;
        }

        public double N(string word, Label rating)
        {
            int count = 0;
            foreach (var review in reviews)
            {
                if (review.Rating == rating)
                {
                    string[] words = review.ReviewText.Split(null);
                    foreach (var s in words)
                    {
                        if (s.Equals(word))
                            count++;
                    }
                }
            }
            return count;
        }

    }
}
