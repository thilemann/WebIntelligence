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
        BlockingCollection<Uri> _queue { get; set; }

        IEnumerable<Uri> GetUris();

        void CompleteAdding();

        void AddUri(Uri uri);

        bool AddUriRange(List<Uri> range);

        bool IsEmpty();
    }
}
