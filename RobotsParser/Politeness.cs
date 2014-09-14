using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.RobotsTxtParser
{
    public class Politeness
    {
        private List<string> allows = new List<string>();

        public List<string> Allows
        {
            get { return allows; }
            set { allows = value; }
        }

        private List<string> disallows = new List<string>();

        public List<string> Disallows
        {
            get { return disallows; }
            set { disallows = value; }
        }
    }
}
