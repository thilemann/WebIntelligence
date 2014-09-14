using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class RobotsTxt
    {
        private const string HTTP = "http://";
        private const string HTTPS = "https://";

        private Dictionary<string, Politeness> politenesses;

        public List<string> Sitemaps { get; set; }
        public Uri Domain { get; set; }

        public RobotsTxt(string domain)
        {
            politenesses = new Dictionary<string, Politeness>();
            Sitemaps = new List<string>();

            if (!domain.StartsWith(HTTP) && !domain.StartsWith(HTTPS))
                domain = string.Format("{0}{1}", HTTP, domain);

            Domain = new Uri(domain);
        }

        public void PutPoliteness(string agent, Politeness politeness)
        {
            politenesses.Add(agent, politeness);
        }

        public Politeness GetPoliteness(string agent)
        {
            return politenesses[agent];
        }

        public bool HasAgent(string agent)
        {
            return politenesses.ContainsKey(agent);
        }

        public bool CanVisit(string agent, Uri uri)
        {
            bool result = true;

            if (!politenesses.ContainsKey(agent))
                return result;

            string url = uri.AbsoluteUri;

            try
            {
                foreach (var politeness in politenesses[agent].Disallows)
                {
                    string pattern = string.Format(".*{0}.*", politeness.Replace("*", ".*").Replace("$", ".*"));
                    Regex regEx = new Regex(pattern);
                    if (regEx.Match(url).Success)
                    {
                        result = false;
                        break;
                    }
                }

                foreach (var politeness in politenesses[agent].Allows)
                {
                    string pattern = Regex.Escape(politeness);
                    pattern = string.Format(".*{0}.*", politeness.Replace("\\*", ".*"));
                    Regex regEx = new Regex(pattern);
                    if (regEx.Match(url).Success)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}
