using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    interface IPrioritizer
    {
        Uri GetUri();

        void AddUri(Uri uri);

        void AddUriRange(List<Uri> range);

        bool IsEmpty();
    }
}
