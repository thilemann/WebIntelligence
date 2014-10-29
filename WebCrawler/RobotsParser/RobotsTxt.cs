using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebCrawler.Logger;

namespace WebCrawler.RobotsTxtParser
{
    public class RobotsTxt
    {
        private const string HTTP = "http://";
        private const string HTTPS = "https://";

        private readonly Dictionary<string, Politeness> _politenesses;
        private Log _logger;

        public List<string> Sitemaps { get; set; }
        public Uri Domain { get; set; }

        public RobotsTxt(string domain)
        {
            _politenesses = new Dictionary<string, Politeness>();
            Sitemaps = new List<string>();
            _logger = Log.Instance;

            if (!domain.StartsWith(HTTP) && !domain.StartsWith(HTTPS))
                domain = string.Format("{0}{1}", HTTP, domain);

            Domain = new Uri(domain);
        }

        public void PutPoliteness(string agent, Politeness politeness)
        {
            _politenesses.Add(agent, politeness);
        }

        public Politeness GetPoliteness(string agent)
        {
            return _politenesses[agent];
        }

        public bool HasAgent(string agent)
        {
            return _politenesses.ContainsKey(agent);
        }

        public bool CanVisit(string agent, Uri uri)
        {
            bool result = true;

            if (!_politenesses.ContainsKey(agent))
                return result;

            string url = uri.AbsoluteUri;

            try
            {
                foreach (var politeness in _politenesses[agent].Disallows)
                {
                    string pattern = string.Format(".*{0}.*", politeness.Replace("*", ".*").Replace("$", ".*"));
                    Regex regEx = new Regex(pattern);
                    if (regEx.Match(url).Success)
                    {
                        result = false;
                        break;
                    }
                }

                foreach (var politeness in _politenesses[agent].Allows)
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
            catch (Exception e)
            {
                _logger.Write(LogLevel.Error, string.Format("RobotsTxt.cs: Could not determine whether '{0}' could be visited - Skipping.", uri.AbsoluteUri));
                _logger.Write(LogLevel.Error, e.ToString());
                result = false;
            }

            return result;
        }
    }
}
