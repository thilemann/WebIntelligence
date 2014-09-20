using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexing.Core
{
    class Term
    {
        private readonly SortedList _postings;

        public int Frequency {
            get
            {
                return _postings.Count;
            }
        }

        public Term(string term)
        {
            _postings = new SortedList();
        }

        public bool ContainsPosting(int id)
        {
            return _postings.ContainsKey(id);
        }

        public void AddPosting(int id)
        {
            _postings.Add(id, id);
        }
    }
}
