using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexer.Core
{
    class Term
    {
        SortedList Postings;

        public int Frequency {
            get
            {
                return Postings.Count;
            }
        }

        public Term()
        {
            Postings = new SortedList();
        }

        public void AddPosting(int id)
        {
            Postings.Add(id, id);
        }
    }
}
