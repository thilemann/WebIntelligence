using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Threading;
using WebCrawler.Logger;
using WebCrawler.RobotsTxtParser;

namespace WebCrawler.Crawl
{
    public class Crawler
    {
        private const int TIME_BETWEEN_VISITS = 1; // Seconds - minimum time between each visit

        private Timer _statisticallyTimer;
        private Statistics _statistics;
        private readonly Log _logger;

        private readonly Store _store;
        private readonly IUrlFrontier _urlFrontier;
        private readonly Dictionary<IPAddress, DateTime> _visitedServers;
        private readonly Parser _parser;
        private readonly Dictionary<string, RobotsTxt> _robotsTxts;

        public Crawler(string seeds)
        {
            _logger = Log.Instance;
            _visitedServers = new Dictionary<IPAddress, DateTime>();
            _urlFrontier = new SimpleUrlFrontier(seeds);
            _store = new Store();
            _parser = new Parser();
            _robotsTxts = new Dictionary<string, RobotsTxt>();
        }

        public void Start(int limit = 1000)
        {
            _logger.Write(LogLevel.Info, "Crawl started");
            _statistics = new Statistics(limit);
            TimerCallback callback = _statistics.ReportStatistics;
            _statisticallyTimer = new Timer(callback, null, 1000, 1000);

            while (!_urlFrontier.IsEmpty() && _statistics.PagesCrawled < limit)
            {
                WebPage webpage = new WebPage(_urlFrontier.GetUri());

                // Is it safe to visit the webpage?
                if (!EnsurePoliteVisit(webpage)) continue;

                // Visit webpage
                webpage.LoadPage();

                // make sure to update or add time for visit to the dictionary
                UpdateTimestamp(webpage);

                // Add webpage anchors to the queue
                if (!webpage.IsLoaded)
                    continue;

                _store.WriteFile(webpage);

                // Add extracted anchors to the queue
                _urlFrontier.AddUriRange(webpage.GetAnchors());

                _statistics.PagesCrawled++;
            }

            _store.WriteFileMap();
            _statisticallyTimer.Dispose();
            _logger.Write(LogLevel.Info, "Crawl Finished");
            _logger.Write(LogLevel.Info, "");
        }

        private bool EnsurePoliteVisit(WebPage webpage)
        {
            // Check robotstxt to see if we can visit the page
            string hostname = webpage.Uri.DnsSafeHost;
            RobotsTxt robotsTxt;
            if (!_robotsTxts.ContainsKey(hostname))
            {
                robotsTxt = _parser.Parse(hostname);
                _robotsTxts.Add(hostname, robotsTxt);
            }
            else
            {
                robotsTxt = _robotsTxts[hostname];
            }

            if (!robotsTxt.CanVisit("*", webpage.Uri)) // We are not allowed to visit this page
                return false;

            // We are allowed to visit, ensure we are waiting enough time before next request
            IPAddress address = webpage.Address;
            if (_visitedServers.ContainsKey(address))
            {
                DateTime now = DateTime.Now;
                DateTime safeVisit = _visitedServers[address];

                int delay = (int) safeVisit.Subtract(now).TotalMilliseconds;

                if (delay > 0)
                {
                    Thread.Sleep(delay);
                }
            }
            return true;
        }

        private void UpdateTimestamp(WebPage webpage)
        {
            DateTime nextValidVisitTime = DateTime.Now.AddSeconds(TIME_BETWEEN_VISITS);
            if (_visitedServers.ContainsKey(webpage.Address))
            {
                _visitedServers[webpage.Address] = nextValidVisitTime;
            }
            else
            {
                _visitedServers.Add(webpage.Address, nextValidVisitTime);
            }
        }
        
    }
}
