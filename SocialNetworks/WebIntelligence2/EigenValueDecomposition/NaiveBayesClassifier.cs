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

        private int Count
        {
            get { return reviews.Count; }
        }

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


        public double p(Label rating)
        {
            double count = N(rating);

            return count / Count;
        }

        public double p(string word, Label rating)
        {
            
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

    class Review
    {
        private Label rating;
        private string reviewText;

        public Review(string rating, string reviewText)
        {
            Enum.TryParse(rating, true, out this.rating);
            this.reviewText = reviewText;
        }

        public Label Rating
        {
            get { return rating; }
        }

        public string ReviewText
        {
            get { return reviewText; }
        }
    }

    enum Label
    {
        Pos,
        Neg
    }

}
