using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indexing.Linguistics;
using Indexing.Token;

namespace Indexing.Core
{
    public class Indexer
    {
        private readonly Tokenizer _tokenizer;

        private readonly Dictionary<int, string> _postingMap;
        private readonly SortedDictionary<string, Term> _terms;

        public Indexer()
        {
            _tokenizer = new Tokenizer();
            _postingMap = new Dictionary<int, string>();
            _terms = new SortedDictionary<string, Term>();
        }

        public void Index(string uri, string fileContent)
        {
            int postingId = uri.GetHashCode();
            _postingMap.Add(postingId, uri);
            IEnumerable<string> unStemmedTokens = _tokenizer.Tokenize(fileContent);
            foreach (var token in unStemmedTokens)
            {
                Stemmer stemmer = new Stemmer();
                stemmer.Stem(token);
                string stemmedToken = stemmer.ToString();


                Term term;
                if (_terms.ContainsKey(stemmedToken))
                {
                    if (_terms[stemmedToken].ContainsPosting(postingId))
                    {
                        continue;
                    }
                    term = _terms[stemmedToken];
                    term.AddPosting(postingId);
                }
                else
                {
                    term = new Term(stemmedToken);
                    term.AddPosting(postingId);
                    _terms.Add(stemmedToken, term);
                }
            }
        }
    }
}
