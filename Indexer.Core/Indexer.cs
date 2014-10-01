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

        public int TotalDocuments { get { return _postingMap.Count(); } }
        public SortedDictionary<string, Term> Terms { get { return _terms; } }

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
                    term = _terms[stemmedToken];
                }
                else
                {
                    term = new Term(stemmedToken);
                    _terms.Add(stemmedToken, term);
                }
                term.AddPosting(postingId);
            }
        }

        public string GetUrlFromHash(int hashCode)
        {
            if (_postingMap.ContainsKey(hashCode))
                return _postingMap[hashCode];
            else
                return null;
        }
    }
}
