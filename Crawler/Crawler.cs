using System;
using System.Collections.Concurrent;
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

namespace WebCrawler.Core
{
    public class Crawler : IDisposable
    {
        private const int TIME_BETWEEN_VISITS = 1; // Seconds - minimum time between each visit

        private Timer _statisticallyTimer;
        private Statistics _statistics;
        private CancellationTokenSource _cts;

        private readonly Log _logger;
        private readonly Store _store;
        private readonly IUrlFrontier _urlFrontier;
        private readonly ConcurrentDictionary<int, long> _visitedServers;
        private readonly ConcurrentDictionary<int, int> _domainToIpMap;
        private readonly Parser _parser;
        private readonly ConcurrentDictionary<int, RobotsTxt> _robotsTxts;

        public Crawler(string seeds)
        {
            _logger = Log.Instance;
            _visitedServers = new ConcurrentDictionary<int, long>();
            _domainToIpMap = new ConcurrentDictionary<int, int>();
            _urlFrontier = new SimpleUrlFrontier(seeds);
            _store = new Store();
            _parser = new Parser();
            _robotsTxts = new ConcurrentDictionary<int, RobotsTxt>();
            _cts = new CancellationTokenSource();
        }

        public void Start()
        {
            Start(Statistics.NO_LIMIT);
        }

        public void Start(int limit)
        {
            _statistics = new Statistics(limit);
            TimerCallback callback = _statistics.ReportStatistics;
            _statisticallyTimer = new Timer(callback, null, 1000, 1000);
            Console.WriteLine("Starting Threads");
            var background = Task.Factory.StartNew(Process);

            switch (limit)
            {
                case Statistics.NO_LIMIT:
                    if (Console.ReadKey().KeyChar == 'c')
                    {
                        _cts.Cancel();
                    }
                    break;
                default:
                    while (_statistics.PagesCrawled < limit) {} // Spin around while we haven't reached our limit
                    _cts.Cancel();
                    break;
            }

            _urlFrontier.CompleteAdding();

            try
            {
                background.Wait();
            }
            catch (AggregateException e)
            {
                foreach (var innerException in e.InnerExceptions)
                {
                    if (innerException is TaskCanceledException)
                    {
                        _logger.Write(LogLevel.Info, string.Format("TaskCanceledException: Task {0}",
                            ((TaskCanceledException)innerException).Task.Id));
                    }
                    else
                    {
                        _logger.Write(LogLevel.Error, innerException.ToString());
                    }
                }
            }

            _statisticallyTimer.Dispose();
            _store.WriteFileMap();
        }

        private void Process()
        {
            var exceptions = new ConcurrentQueue<Exception>();

            ParallelOptions options = new ParallelOptions();
            options.CancellationToken = _cts.Token;
            try
            {
                Parallel.ForEach(_urlFrontier.GetUris(), options, (uri) =>
                {
                    try
                    {
                        ProcessUri(options, uri);
                    }
                    catch (Exception e) { exceptions.Enqueue(e); }
                });
            }
            catch (AggregateException e)
            {
                LogExceptions(e.InnerExceptions);
            }

            LogExceptions(exceptions);
        }

        private void LogExceptions(IEnumerable<Exception> exceptions)
        {
            // Throw the exceptions here after the loop completes. 
            var enumerable = exceptions as IList<Exception> ?? exceptions.ToList();
            if (enumerable.Any())
            {
                foreach (var e in enumerable)
                {
                    _logger.Write(LogLevel.Error, e.InnerException.ToString());
                }
            }
        }

        private void ProcessUri(ParallelOptions options, Uri uri)
        {
            if (uri == null)
                return;

            WebPage webpage = new WebPage(uri);

            // Is it safe to visit the webpage?
            if (!EnsurePoliteVisit(webpage)) return;

            // Visit webpage
            webpage.LoadPage();

            // make sure to update or add time for visit to the dictionary
            UpdateTimestamp(webpage);

            // Add webpage anchors to the queue
            if (!webpage.IsLoaded)
                return;

            if (options.CancellationToken.IsCancellationRequested)
            {
                Thread.CurrentThread.Abort();
            }
            _store.WriteFile(webpage);
            _statistics.IncrementPagesCrawled();

            // Add extracted anchors to the queue
            _urlFrontier.AddUriRange(webpage.GetAnchors());
        }
        
        private bool EnsurePoliteVisit(WebPage webpage)
        {
            // Check robotstxt to see if we can visit the page
            string hostname = webpage.Uri.DnsSafeHost;
            RobotsTxt robotsTxt;
            if (!_robotsTxts.ContainsKey(hostname.GetHashCode()))
            {
                robotsTxt = _parser.Parse(hostname);
                if (robotsTxt == null) // we could not get the robotstxt therefore we cannot visit and we requeue the Uri
                {
                    _urlFrontier.AddUri(webpage.Uri);
                    return false;
                }

                _robotsTxts.TryAdd(hostname.GetHashCode(), robotsTxt);
            }
            else
            {
                robotsTxt = _robotsTxts[hostname.GetHashCode()];
            }

            if (!robotsTxt.CanVisit("*", webpage.Uri)) // We are not allowed to visit this page
                return false;

            // We are allowed to visit, ensure we are waiting enough time before next request
            int address = webpage.Address;
            if (_visitedServers.ContainsKey(address))
            {
                DateTime now = DateTime.Now;
                DateTime safeVisit = new DateTime(_visitedServers[address]);

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
                _visitedServers[webpage.Address] = nextValidVisitTime.Ticks;
            }
            else
            {
                _visitedServers.TryAdd(webpage.Address, nextValidVisitTime.Ticks);
            }
        }

        public void Dispose()
        {
        }
    }
}
