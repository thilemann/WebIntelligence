using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Threading;

namespace WebCrawler
{
    public class Crawler
    {
        private const int SLEEP = 1000; // milliseconds
        private const int TIME_BETWEEN_VISITS = 1; // Seconds

        private Store store;
        private IUrlFrontier urlFrontier;
        private Dictionary<IPAddress, DateTime> visitedServers;

        public Crawler(string seeds)
        {
            visitedServers = new Dictionary<IPAddress, DateTime>();
            urlFrontier = new SimpleUrlFrontier(seeds);
            store = new Store();
        }

        public void Start(int limit = 5)
        {
            Console.WriteLine("Starting...");
            int count = 0;
            while (!urlFrontier.IsEmpty() && count < limit)
            {
                WebPage webpage = new WebPage(urlFrontier.GetUri());

                // Is it safe to visit the webpage?
                int delay = DelayVisit(webpage.Address);
                if (delay > 0)
                {
                    Console.WriteLine("Waiting for visit: {0} ms", delay);
                    Thread.Sleep(delay);
                }

                // Visit webpage
                webpage.LoadPage();

                // make sure to update or add time for visit to the dictionary
                DateTime nextValidVisitTime = DateTime.Now.AddSeconds(TIME_BETWEEN_VISITS);
                if (visitedServers.ContainsKey(webpage.Address))
                {
                    visitedServers[webpage.Address] = nextValidVisitTime;
                }
                else
                {
                    visitedServers.Add(webpage.Address, nextValidVisitTime);
                }

                // Add webpage anchors to the queue
                if (!webpage.IsLoaded)
                    continue;

                store.WriteFile(webpage);

                urlFrontier.AddUriRange(webpage.GetAnchors());
                count++;
            }

            store.WriteFileMap();
        }

        private int DelayVisit(IPAddress address)
        {
            if (visitedServers.ContainsKey(address))
            {
                DateTime now = DateTime.Now;
                DateTime safeVisit = visitedServers[address];

                return (int) safeVisit.Subtract(now).TotalMilliseconds;

            }
            return 0;
        }
        
    }
}
