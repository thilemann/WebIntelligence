using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WebCrawler
{
    class SimpleUrlFrontier : IUrlFrontier
    {
        private Queue<Uri> queue;
        private Dictionary<string, object> addedPages;

        public SimpleUrlFrontier(string seeds)
        {
            addedPages = new Dictionary<string, object>();
            queue = Helper.GetSeeds(seeds);
        }

        public Uri GetUri()
        {
            return queue.Dequeue();
        }

        public void AddUri(Uri uri)
        {
            SafeAddUri(uri);
        }

        private void SafeAddUri(Uri uri)
        {

            if (!addedPages.ContainsKey(uri.AbsoluteUri))
            {
                queue.Enqueue(uri);
                addedPages.Add(uri.AbsoluteUri, null);
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
            return queue.Count == 0;
        }
    }


}
