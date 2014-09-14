
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
    public class RobotsTxtParser
    {
        private const string ROBOT_TXT = "robots.txt";

        private static Regex agentRegex = new Regex(@"User-agent:\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private static Regex disallowRegex = new Regex(@"Disallow:\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private static Regex allowRegex = new Regex(@"Allow:\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        private static Regex sitemapRegex = new Regex(@"Sitemap:\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        
        public RobotsTxt Parse(string domainUrl)
        {

            RobotsTxt robotTxt = new RobotsTxt(domainUrl);
            Uri uri = new Uri(robotTxt.Domain, ROBOT_TXT);
            Politeness currentPoliteness = null;

            var reader = GetStream(uri);
            if (reader != null)
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var agentMatch = agentRegex.Match(line);

                    if (agentMatch.Success)
                    {
                        string agent = agentMatch.Groups[1].Value;

                        if (!robotTxt.HasAgent(agent))
                        {
                            currentPoliteness = new Politeness();
                            robotTxt.PutPoliteness(agent, currentPoliteness);
                        }
                        else
                            currentPoliteness = robotTxt.GetPoliteness(agent);
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

                    if (!sitemapMatch.Success) continue;
                    if (currentPoliteness != null)
                        robotTxt.Sitemaps.Add(sitemapMatch.Groups[1].Value);
                }
            }

            return robotTxt;
        }

        private StreamReader GetStream(Uri uri)
        {
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                // Sends the HttpWebRequest and waits for the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Gets the stream associated with the response.
                Stream stream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                Encoding encoding = Encoding.GetEncoding("utf-8");
                reader = new StreamReader(stream, encoding);
            }
            catch (WebException e)
            {
            }
            return reader;
        }

    }
}
