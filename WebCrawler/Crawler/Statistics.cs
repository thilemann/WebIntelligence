using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebCrawler.Core
{
    class Statistics
    {
        public const int NO_LIMIT = -1;

        private readonly int _pageLimit;
        private readonly DateTime _startTime;
        private int _pagesCrawled;

        public int PagesCrawled
        {
            get { return _pagesCrawled; }
        }

        public void IncrementPagesCrawled()
        {
            Interlocked.Increment(ref _pagesCrawled);
        }

        public TimeSpan TimeElapsed
        {
            get { return DateTime.Now.Subtract(_startTime); }
        }

        public double PagesPrSecond
        {
            get { return _pagesCrawled / TimeElapsed.TotalSeconds; }
        }
        
        public Statistics()
        {
            _pagesCrawled = 0;
            _startTime = DateTime.Now;
            _pageLimit = NO_LIMIT;
        }

        public Statistics(int limit) : this()
        {
            _pageLimit = limit;
        }

        public void ReportStatistics(object state)
        {
            Console.SetCursorPosition(0, 1);
            if (_pageLimit == NO_LIMIT)
                Console.Write("Pages crawled: {0}", _pagesCrawled);
            else
                Console.WriteLine("Pages crawled: {0} / {1}", _pagesCrawled, _pageLimit);

            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Time elapsed: {0}", TimeElapsed.ToString(@"hh\:mm\:ss"));

            Console.SetCursorPosition(0, 3);
            Console.WriteLine("Crawled pages pr second: {0}", PagesPrSecond);
        }
    }
}
