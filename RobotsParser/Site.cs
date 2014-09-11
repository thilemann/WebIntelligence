using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class Site
    {
        public Site(Uri domain)
        {
            Domain = domain;
        }

        public Uri Domain { get; set; }

        private Dictionary<string, Politeness> politenesses = new Dictionary<string, Politeness>();

        //TODO: For testing
        public Dictionary<string, Politeness> Politenesses
        {
            get { return politenesses; }
            set { politenesses = value; }
        }

        public Politeness GetPoliteness(string userAgent)
        {
            return politenesses[userAgent];
        }

        public void PutPoliteness(string userAgent, Politeness politeness)
        {
            politenesses.Add(userAgent, politeness);
        }

        private List<string> sitemaps = new List<string>();

        public List<string> Sitemaps
        {
            get { return sitemaps; }
            set { sitemaps = value; }
        }

        public bool HasAgent(string agent)
        {
            return politenesses.ContainsKey(agent);
        }
    }
}
