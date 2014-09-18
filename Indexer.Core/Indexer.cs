using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indexer.Token;
using Indexer.Linguistics;

namespace Indexer.Core
{
    class Indexer
    {
        private Tokenizer tokenizer;

        private Dictionary<int, string> postingMap;
        private SortedDictionary<string, Term> terms;

        public Indexer()
        {
            tokenizer = new Tokenizer();
            postingMap = new Dictionary<int, string>();
            terms = new SortedDictionary<string, Term>();
        }

        public void Start(string uri, string fileContent)
        {
            int postingId = uri.GetHashCode();
            postingMap.Add(postingId, uri);
            List<string> unStemmedTokens = tokenizer.Tokenize(fileContent);
            foreach (var token in unStemmedTokens)
            {
                Stemmer stemmer  = new Stemmer();
                stemmer.Stem(token);
                string stemmedToken = stemmer.ToString();
                
                Term term = new Term();
                term.AddPosting(postingId);
                terms.Add(stemmedToken, term);
            }
            unStemmedTokens = null;
        }
    }
}
