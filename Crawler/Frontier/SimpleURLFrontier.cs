using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WebCrawler.Crawl
{
    class SimpleUrlFrontier : IUrlFrontier
    {
        private readonly Queue<Uri> _queue;
        private readonly Dictionary<string, object> _addedPages;

        public SimpleUrlFrontier(string seeds)
        {
            _addedPages = new Dictionary<string, object>();
            _queue = Helper.GetSeeds(seeds);
        }

        public Uri GetUri()
        {
            return _queue.Dequeue();
        }

        public void AddUri(Uri uri)
        {
            SafeAddUri(uri);
        }

        private void SafeAddUri(Uri uri)
        {

            if (!_addedPages.ContainsKey(uri.AbsoluteUri))
            {
                _queue.Enqueue(uri);
                _addedPages.Add(uri.AbsoluteUri, null);
            }
        }

        public void AddUriRange(List<Uri> range)
        {
            foreach (var uri in range)
            {
                SafeAddUri(uri);
            }
        }

        public bool IsEmpty()
        {
            return _queue.Count == 0;
        }
    }


}
