using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WebCrawler.Core
{
    public class SimpleUrlFrontier : IUrlFrontier
    {
        public BlockingCollection<Uri> _queue { get; set; }
        private readonly ConcurrentDictionary<int, object> _addedPages;

        public SimpleUrlFrontier(string seeds)
        {
            _addedPages = new ConcurrentDictionary<int, object>();
            _queue = Helper.GetSeeds(seeds);
        }

        public virtual IEnumerable<Uri> GetUris()
        {
            return _queue.GetConsumingEnumerable();
        }

        public virtual void CompleteAdding()
        {
            _queue.CompleteAdding();
        }

        public void AddUri(Uri uri)
        {
            SafeAddUri(uri);
        }

        private void SafeAddUri(Uri uri)
        {
            if (!_addedPages.ContainsKey(uri.AbsoluteUri.GetHashCode()))
            {
                _queue.Add(uri);
                _addedPages.TryAdd(uri.AbsoluteUri.GetHashCode(), null);
            }
        }

        public bool AddUriRange(List<Uri> range)
        {
            if (_queue.IsAddingCompleted)
                return false;

            foreach (var uri in range)
            {
                SafeAddUri(uri);
            }

            return true;
        }

        public bool IsEmpty()
        {
            return _queue.Count == 0;
        }
    }


}
