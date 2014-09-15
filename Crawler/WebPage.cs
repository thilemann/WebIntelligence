using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using WebCrawler.Logger;

namespace WebCrawler.Crawl
{
    public class WebPage
    {
        private readonly HtmlDocument _html;
        private readonly Log _logger;
        private Uri _uri;

        public IPAddress Address { get; private set; }

        public Uri Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        public bool IsLoaded { get; private set; }

        public WebPage(Uri uri)
        {
            _logger = Log.Instance;
            _uri = uri;
            _html = new HtmlDocument();
            try
            {
                Address = Helper.ResolveDNS(uri.DnsSafeHost);
            }
            catch (Exception e)
            {
                Address = new IPAddress(new byte[] { 0, 0, 0, 0 });
                _logger.Write(LogLevel.Error, "WebPage: DNS Resolving failed");
                _logger.Write(LogLevel.Error, uri.DnsSafeHost);
                _logger.Write(LogLevel.Error, e.ToString());
            }
            IsLoaded = false;
        }

        public void LoadPage()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_uri);

            // Sends the HttpWebRequest and waits for the response.
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                _logger.Write(LogLevel.Error, "WebPage: Could not load page");
                _logger.Write(LogLevel.Error, _uri.AbsoluteUri);
                _logger.Write(LogLevel.Error, e.ToString());
                return;
            }

            // Gets the stream associated with the response.
            Stream stream = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("utf-8");

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader reader = new StreamReader(stream, encoding);

            _html.LoadHtml(reader.ReadToEnd());

            IsLoaded = true;
        }

        public void SavePage(string filePath)
        {
            _html.Save(filePath, Encoding.UTF8);
        }

        public List<Uri> GetAnchors()
        {
            HtmlNodeCollection anchors = _html.DocumentNode.SelectNodes("//a");
            List<Uri> hrefs = new List<Uri>();

            if (anchors == null)
                return hrefs;

            foreach (var anchor in anchors)
            {
                if (!anchor.Attributes.Contains("href"))
                    continue;

                string path = anchor.Attributes["href"].Value;
                try
                {
                    if (path.Contains("javascript:"))
                        continue;

                    hrefs.Add(UriNormalizer.Normalize(_uri.DnsSafeHost, path));
                }
                catch (Exception e)
                {
                    _logger.Write(LogLevel.Error, "WebPage: Could not Normalize url or is not valid for an Uri instance");
                    _logger.Write(LogLevel.Error, path);
                    _logger.Write(LogLevel.Error, e.ToString());
                }
            }
            return hrefs;
        }
        
    }
}
