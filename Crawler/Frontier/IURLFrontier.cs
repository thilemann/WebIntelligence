using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Crawl
{
    interface IUrlFrontier
    {
        Uri GetUri();

        void AddUri(Uri uri);

        void AddUriRange(List<Uri> range);

        bool IsEmpty();
    }
}
