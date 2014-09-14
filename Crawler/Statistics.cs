using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Crawl
{
    class Statistics
    {
        private const int NO_LIMIT = -1;

        private readonly int _pageLimit;
        private readonly DateTime _startTime;

        public int PagesCrawled { get; set; }

        public Statistics()
        {
            PagesCrawled = 0;
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
                Console.Write("Pages crawled: {0}", PagesCrawled);
            else
                Console.WriteLine("Pages crawled: {0} / {1}", PagesCrawled, _pageLimit);

            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Time elapsed: {0}", DateTime.Now.Subtract(_startTime).ToString(@"hh\:mm\:ss"));
        }
    }
}
