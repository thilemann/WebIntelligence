using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentiment
{
    class NaiveTerm
    {
        private int negN = 0;
        private int posN = 0;

        public NaiveTerm(Label rating)
        {
            Add(rating);
        }

        public int N(Label rating)
        {
            if (rating == Label.Neg)
                return negN;
            else if (rating == Label.Pos)
                return posN;

            return 0;
        }

        public void Add(Label rating)
        {
            if (rating == Label.Neg)
                negN++;
            else if (rating == Label.Pos)
                posN++;
        }
    }
}
