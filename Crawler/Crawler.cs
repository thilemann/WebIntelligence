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
        private IPrioritizer prioritizer;
        private WebPage webpage;
        private Dictionary<IPAddress, DateTime> visitedServers;

        public Crawler(string seeds)
        {
            visitedServers = new Dictionary<IPAddress, DateTime>();
            prioritizer = new SimplePrioritizer(seeds);
            store = new Store();
        }

        public void Start(int limit = 5)
        {
            Console.WriteLine("Starting...");
            int count = 0;
            while (!prioritizer.IsEmpty() && count < limit)
            {
                webpage = new WebPage(prioritizer.GetUri());

                // Is it safe to visit the webpage?
                int delay = DelayVisit(webpage.Address);
                if (delay < SLEEP)
                {
                    Console.WriteLine("Waiting for visit: {0} ms", SLEEP - delay);
                    //Thread.Sleep(SLEEP - delay);
                }

                // Visit webpage
                webpage.LoadPage();
                // Add webpage anchors to the queue
                if (!webpage.IsLoaded)
                    continue;

                store.WriteFile(webpage);

                prioritizer.AddUriRange(webpage.GetAnchors());

                // make sure to update or add time for visit to the dictionary
                if (visitedServers.ContainsKey(webpage.Address))
                {
                    visitedServers[webpage.Address] = DateTime.Now;
                }
                else
                {
                    visitedServers.Add(webpage.Address, DateTime.Now);
                }
                count++;
            }

            store.WriteFileMap();
        }

        private int DelayVisit(IPAddress address)
        {
            if (visitedServers.ContainsKey(address))
            {
                DateTime now = DateTime.Now;
                DateTime then = visitedServers[address];

                return (int) now.Subtract(then).TotalMilliseconds;

            }
            return SLEEP;
        }
        
    }
}
