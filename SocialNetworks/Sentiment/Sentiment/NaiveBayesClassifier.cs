using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentiment
{
    public class NaiveBayesClassifier : IDisposable
    {
        private const int C_COUNT = 2;

        private Dictionary<string, NaiveTerm> _vocabulary = new Dictionary<string, NaiveTerm>();
        private List<Review> _trainingReviews;

        private int _posReviewCount = 0;
        private int _negReviewCount = 0;

        readonly Tokenizer _tokenizer;

        public NaiveBayesClassifier()
        {
            _tokenizer = new Tokenizer();
        }

        public void Train(List<Review> reviews)
        {
            int curPos = Console.CursorLeft;
            _trainingReviews = reviews;
            int reviewCount = _trainingReviews.Count;

            InitializeReview_N(_trainingReviews);

            for (int i = 0; i < reviewCount; i++)
            {
                Review review = _trainingReviews[i];
                foreach (var token in _tokenizer.Tokenize(review.Summary))
                {
                    AddToVocabulary(token, review.Rating);
                }
                foreach (var token in _tokenizer.Tokenize(review.Text))
                {
                    AddToVocabulary(token, review.Rating);
                }
                Console.SetCursorPosition(curPos, Console.CursorTop);
                float progress = ((float)i) / reviewCount * 100;
                Console.Write("{0}%", Convert.ToInt32(progress));
            }
            Console.WriteLine();
        }

        public void Train(TestDataParser parser)
        {
            parser.Parse();
            Train(parser.Reviews);
        }

        public double Score(string text, Label rating)
        {
            List<string> tokens = _tokenizer.Tokenize(text);
            return S(tokens, rating);
        }

        public double ScoreLog(string text, Label rating)
        {
            List<string> tokens = _tokenizer.Tokenize(text);
            return logS(tokens, rating);
        }

        private void AddToVocabulary(string word, Label rating)
        {
            if (_vocabulary.ContainsKey(word))
            {
                _vocabulary[word].Add(rating);
            }
            else
            {
                _vocabulary.Add(word, new NaiveTerm(rating));
            }
        }

        private double logS(IEnumerable<string> tokenizedWords, Label rating)
        {
            double summation = 0;
            foreach (var word in tokenizedWords)
            {
                summation += Math.Log(p(word, rating));
            }
            return Math.Log(p(rating)) + summation;
        }

        private double S(IEnumerable<string> tokenizedWords, Label rating)
        {
            double result = 1;
            foreach (var word in tokenizedWords)
            {
                double pValue = p(word, rating);
                result *= pValue / (1 - pValue);
            }

            return SStarEmpty(rating)*result;
        }

        private double SStarEmpty(Label rating)
        {
            double result = 1;
            foreach (var word in _vocabulary.Keys)
            {
                result *= NotP(word, rating);
            }
            return result*p(rating);
        }

        private double p(Label rating)
        {
            return (N(rating) + 1) / (_trainingReviews.Count + C_COUNT);
            //return N(rating) / _trainingReviews.Count;
        }

        private double p(string word, Label rating)
        {
            return (N(word, rating) + 1) / (N(rating) + _vocabulary.Count);
            //return N(word, rating) / N(rating);
        }

        private double NotP(string word, Label rating)
        {
            double value = 1 - p(word, rating);
            return value;
        }

        private double N(Label rating)
        {
            if (rating == Label.Neg)
                return _negReviewCount;
            
            if (rating == Label.Pos)
                return _posReviewCount;

            return 0;
        }

        private void InitializeReview_N(IEnumerable<Review> reviews)
        {
            foreach (var review in reviews)
            {
                if (review.Rating == Label.Neg)
                    _negReviewCount++;
                else if (review.Rating == Label.Pos)
                    _posReviewCount++;
            }
        }

        private double N(string word, Label rating)
        {
            if (_vocabulary.ContainsKey(word))
            {
                return _vocabulary[word].N(rating);
            }
            return 0;
        }

        public void Dispose()
        {
            _trainingReviews = null;
            _vocabulary = null;
        }
    }
}
