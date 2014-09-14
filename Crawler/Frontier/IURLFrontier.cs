using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Crawl
{
    interface IUrlFrontier
    {
        BlockingCollection<Uri> Queue { get; set; }

        Uri GetUri();

        void AddUri(Uri uri);

        void AddUriRange(List<Uri> range);

        bool IsEmpty();
    }
}
