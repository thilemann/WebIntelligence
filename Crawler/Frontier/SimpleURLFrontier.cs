using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WebCrawler.Crawl
{
    class SimpleUrlFrontier : IUrlFrontier
    {
        //private readonly ConcurrentQueue<Uri> Queue;
        private readonly ConcurrentDictionary<string, object> _addedPages;

        public SimpleUrlFrontier(string seeds)
        {
            _addedPages = new ConcurrentDictionary<string, object>();
            Queue = Helper.GetSeeds(seeds);
        }

        public BlockingCollection<Uri> Queue { get; set; }

        public Uri GetUri()
        {
            Uri uri = null;
            //Queue.TryDequeue(out uri);
            return uri;
        }

        public void AddUri(Uri uri)
        {
            SafeAddUri(uri);
        }

        private void SafeAddUri(Uri uri)
        {
            if (!_addedPages.ContainsKey(uri.AbsoluteUri))
            {
                Queue.Add(uri);
                _addedPages.TryAdd(uri.AbsoluteUri, null);
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
            return Queue.Count == 0;
        }
    }


}
