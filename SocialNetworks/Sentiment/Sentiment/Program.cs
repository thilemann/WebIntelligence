using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sentiment
{
    class Program
    {   
        public static void Main()
        {
            Console.WriteLine("Started parsing");
            TestDataParser parser = new TestDataParser("Resources/SentimentTrainingData.txt");
            parser.Parse();
            Console.WriteLine("Finished parsing\n");
            List<Review> posReviews = new List<Review>();
            List<Review> negReviews = new List<Review>();

            Console.WriteLine("Sorting reviews");
            foreach (var review in parser.Reviews)
            {
                if (review.Rating == Label.Neg)
                    negReviews.Add(review);
                else if (review.Rating == Label.Pos)
                    posReviews.Add(review);
            }

            List<Review>[] folds = SplitReviews(posReviews, 10);
            List<Review>[] negFold = SplitReviews(negReviews, 10);

            Console.WriteLine("Combing reviews");
            for (int i = 0; i < folds.Length; i++)
            {
                folds[i].AddRange(negFold[i]);
            }

            Console.WriteLine("Testing reviews:\n");
            for (int j = 0; j < folds.Length; j++)
            {
                List<Review> testReviews = folds[j];
                List<Review> trainReviews = new List<Review>();
                NaiveBayesClassifier classifier = new NaiveBayesClassifier();
                for (int i = 0; i < folds.Length; i++)
                {
                    if (j == i) continue;

                    trainReviews.AddRange(folds[i]);
                }
                classifier.Train(trainReviews);
                int correct = 0;
                foreach (var testReview in testReviews)
                {
                    double posScore = classifier.Score(testReview.Summary + testReview.Text, Label.Pos);
                    double negScore = classifier.Score(testReview.Summary + testReview.Text, Label.Neg);
                    Label calculatedLabel;
                    if (posScore > negScore)
                        calculatedLabel = Label.Pos;
                    else
                        calculatedLabel = Label.Neg;

                    if (calculatedLabel == testReview.Rating)
                        correct++;
                }
                Console.WriteLine("Test fold {0}: {1} / {2}", j, correct, testReviews.Count);
            }
            Console.ReadLine();
        }

        public static List<Review>[] SplitReviews(List<Review> reviews, int parts)
        {
            List<Review>[] reviewFold = new List<Review>[parts];
            int posEach = reviews.Count / parts;
            int k = 0;
            for (int i = 0; i < reviews.Count; i++)
            {
                if (k == parts) break;

                if (reviewFold[k] == null)
                    reviewFold[k] = new List<Review>();

                reviewFold[k].Add(reviews[i]);

                if (reviewFold[k].Count == posEach && k < parts)
                    k++;
            }
            return reviewFold;
        } 

        
    }
}
