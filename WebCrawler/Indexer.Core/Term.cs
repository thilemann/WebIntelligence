using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexing.Core
{
    public class Term
    {
        private readonly SortedList<int, int> _postings;

        public SortedList<int, int> Postings { get { return _postings; } }

        public int Frequency {
            get
            {
                return _postings.Count;
            }
        }

        public Term(string term)
        {
            _postings = new SortedList<int, int>();
        }

        public bool ContainsPosting(int id)
        {
            return _postings.ContainsKey(id);
        }

        public void AddPosting(int id)
        {
            if (_postings.ContainsKey(id))
            {
                _postings[id] = _postings[id] + 1;
            }
            else
	        {
                _postings.Add(id, 1);
	        }
        }

        public int GetTermFrequency(int id)
        {
            if (_postings.ContainsKey(id))
                return _postings[id];
            else
                return 0;
        }
    }
}
