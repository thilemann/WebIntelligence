﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentiment
{
    public class NaiveBayesClassifier
    {
        private Dictionary<string, NaiveTerm> vocabulary = new Dictionary<string, NaiveTerm>();
        private List<Review> _trainingReviews;

        private int _posReview = 0;
        private int _negReview = 0;

        Tokenizer tokenizer;

        public NaiveBayesClassifier()
        {
            tokenizer = new Tokenizer();
        }

        public void Train(TestDataParser parser)
        {
            int curPos = Console.CursorLeft;
            parser.Parse();
            _trainingReviews = parser.Reviews;
            int reviewCount = _trainingReviews.Count;

            InitializeReview_N(_trainingReviews);

            for (int i = 0; i < reviewCount; i++)
            {
                Review review = _trainingReviews[i];
                foreach (var token in tokenizer.Tokenize(review.Summary))
                {
                    AddToVocabulary(token, review.Rating);
                }
                foreach (var token in tokenizer.Tokenize(review.Text))
                {
                    AddToVocabulary(token, review.Rating);
                }
                Console.SetCursorPosition(curPos, Console.CursorTop);
                Console.Write("{0}%", (((float)i) / reviewCount) * 100);
            }
            Console.WriteLine();
        }

        public double Score(string text, Label rating)
        {
            List<string> tokens = tokenizer.Tokenize(text);
            return logS(tokens, rating);
        }

        private void AddToVocabulary(string word, Label rating)
        {
            if (vocabulary.ContainsKey(word))
            {
                vocabulary[word].Add(rating);
            }
            else
            {
                vocabulary.Add(word, new NaiveTerm(rating));
            }
        }

        public double logS(List<string> tokenizedWords, Label rating)
        {
            double summation = 0;
            foreach (var word in tokenizedWords)
            {
                summation += Math.Log(p(word, rating));
            }
            return Math.Log(p(rating)) + summation;
        }

        public double S(List<string> tokenizedWords, Label rating)
        {
            double result = 1;
            foreach (var word in tokenizedWords)
            {
                double pValue = p(word, rating);
                result *= pValue / (1 - pValue);
            }

            return SStarEmpty(rating)*result;
        }

        public double SStarEmpty(Label rating)
        {
            double result = 1;
            foreach (var word in vocabulary.Keys)
            {
                result *= NotP(word, rating);
            }
            return result*p(rating);
        }

        public double p(Label rating)
        {
            return N(rating) / _trainingReviews.Count;
        }

        public double p(string word, Label rating)
        {
            return N(word, rating) / _trainingReviews.Count;
        }

        public double NotP(string word, Label rating)
        {
            return 1 - p(word, rating);
        }
        
        public double N(Label rating)
        {
            if (rating == Label.Neg)
                return _negReview;
            
            if (rating == Label.Pos)
                return _posReview++;

            return 0;
        }

        private void InitializeReview_N(List<Review> reviews)
        {
            foreach (var review in reviews)
            {
                if (review.Rating == Label.Neg)
                    _negReview++;
                else if (review.Rating == Label.Pos)
                    _posReview++;
            }
        }

        public double N(string word, Label rating)
        {
            if (vocabulary.ContainsKey(word))
            {
                return vocabulary[word].N(rating);
            }
            return 0;
        }

    }
}
