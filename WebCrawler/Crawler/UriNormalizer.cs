using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawler.Core
{
    static class UriNormalizer
    {
        private const string HTTP = "http://";
        private const string HTTPS = "https://";

        private static string EnsureProtocol(string url)
        {
            if (!url.StartsWith(HTTP) && !url.StartsWith(HTTPS))
                url = string.Format("{0}{1}", HTTP, url);

            return url;
        }

        public static Uri Normalize(string currentDomain, string url)
        {
            Uri uri;
            url = url.Trim();
            // Check if url is an internal anchor, if so we return the current domain
            if (url.StartsWith("#"))
            {
                uri = new Uri(EnsureProtocol(currentDomain));
                return uri;
            }

            url = RemovePartOfUrl(url);

            // Test if url is relative, if so we combine it with the domain
            if (Uri.TryCreate(url, UriKind.Relative, out uri))
            {
                uri = new Uri(new Uri(EnsureProtocol(currentDomain)), url);
            }
            else
            {
                uri = new Uri(EnsureProtocol(url));
            }

            return uri;
        }

        private static string RemovePartOfUrl(string url)
        {
            if (url.Contains("?"))
            {
                int index = url.IndexOf('?');
                url = url.Remove(index);
            }

            if (url.Contains("#"))
            {
                int index = url.IndexOf('#');
                url = url.Remove(index);
            }
            return url;
        }
    }
}
