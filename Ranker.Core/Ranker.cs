using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indexing.Core;

namespace Ranking.Core
{
    public class Ranker
    {
        public Ranker()
        {
        }

        public double GetIdf(int docCount, int docFrequency)
        {
            return Math.Log10(docCount / (double) docFrequency);
        }

        public double GetNormalizedTf(int termFrequency)
        {
            if (termFrequency == 0)
                return 0;

            return 1 + Math.Log10(termFrequency);
        }

        public double GetTfIdf(int termFrequency, int docCount, int docFrequency)
        {
            return GetNormalizedTf(termFrequency) * GetIdf(docCount, docFrequency);
        }

        public IEnumerable<KeyValuePair<int, double>> Rank(IEnumerable<string> queryTerms, SortedDictionary<string, Term> terms, int documentCount)
        {
            SortedList<int, double> scores = new SortedList<int, double>();
            foreach (var queryTerm in queryTerms)
            {
                if (!terms.ContainsKey(queryTerm))
                    return null;

                Term currentTerm = terms[queryTerm];
                int documentFrequency = currentTerm.Frequency;
                foreach (var posting in currentTerm.Postings)
                {
                    int docId = posting.Key;
                    int termFrequency = posting.Value;
                    double weight = GetTfIdf(termFrequency, documentCount, documentFrequency);
                    if (scores.ContainsKey(docId))
                    {
                        scores[docId] += weight;                        
                    }
                    else
                    {
                        scores.Add(docId, weight);
                    }
                }
            }

            return scores.OrderByDescending(kvp => kvp.Value);
        }
    }
}
