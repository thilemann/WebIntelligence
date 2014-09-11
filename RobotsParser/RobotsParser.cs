
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class RobotsTxt
    {
        private const string ROBOT_TXT = "robots.txt";

        private static Regex agentRegex = new Regex(@"User-agent:\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private static Regex disallowRegex = new Regex(@"Disallow:\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private static Regex allowRegex = new Regex(@"Allow:\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private static Regex sitemapRegex = new Regex(@"Sitemap:\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        private Site site;

        public Site Site
        {
            get { return site; }
            set { site = value; }
        }

        public Site Parse(Uri domain)
        {
            Uri uri = new Uri(domain, ROBOT_TXT);
            
            Site site = new Site(domain);
            Politeness currentPoliteness = null;

            var reader = Helpers.GetStream(uri);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                var agentMatch = agentRegex.Match(line);

                if (agentMatch.Success)
                {
                    string agent = agentMatch.Groups[1].Value;

                    if (!site.HasAgent(agent))
                    {
                        currentPoliteness = new Politeness();
                        site.PutPoliteness(agent, currentPoliteness);
                    }
                    else
                        currentPoliteness = site.GetPoliteness(agent);
                }
                else
                {
                    var disallowMatch = disallowRegex.Match(line);
                    var allowMatch = allowRegex.Match(line);

                    if (disallowMatch.Success)
                    {
                        if (currentPoliteness != null)
                            currentPoliteness.Disallows.Add(disallowMatch.Groups[1].Value);
                    }
                    else if (allowMatch.Success)
                    {
                        if (currentPoliteness != null)
                            currentPoliteness.Allows.Add(allowMatch.Groups[1].Value);
                    }
                }


                var sitemapMatch = sitemapRegex.Match(line);

                if (sitemapMatch.Success)
                {
                    if (currentPoliteness != null)
                        site.Sitemaps.Add(sitemapMatch.Groups[1].Value);
                }
            }

            return site;
        }

    }
}
