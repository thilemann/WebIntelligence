using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler
{
    class UriNormalizer
    {
        private string url;
        private Regex relativeRegex = new Regex(@"/?.*");
        public bool IsRelative { get; private set; }

        public UriNormalizer(string url)
        {
            this.url = url;
        }

        public Uri Normalize()
        {
            url = url.ToLower();

            return null;
        }
    }
}
