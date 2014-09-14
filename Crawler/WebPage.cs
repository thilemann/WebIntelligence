using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace WebCrawler
{
    public class WebPage
    {
        private HtmlDocument html;
        private Uri uri;

        public IPAddress Address { get; private set; }

        public Uri Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        public bool IsLoaded { get; private set; }

        public WebPage(Uri uri)
        {
            this.uri = uri;
            html = new HtmlDocument();
            Address = Helper.ResolveDNS(uri.DnsSafeHost);
            IsLoaded = false;
        }

        public void LoadPage()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            // Sends the HttpWebRequest and waits for the response.
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }

            // Gets the stream associated with the response.
            Stream stream = response.GetResponseStream();
            Encoding encoding = System.Text.Encoding.GetEncoding("utf-8");

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader reader = new StreamReader(stream, encoding);

            html.LoadHtml(reader.ReadToEnd());

            Console.WriteLine("Visited: {0}", uri.AbsoluteUri);

            IsLoaded = true;
        }

        public void SavePage(string filePath)
        {
            html.Save(filePath, Encoding.UTF8);
        }

        public List<Uri> GetAnchors()
        {
            HtmlNodeCollection anchors = html.DocumentNode.SelectNodes("//a");
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

                    hrefs.Add(UriNormalizer.Normalize(uri.DnsSafeHost, path));
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: " + path);
                }
            }
            return hrefs;
        }
        
    }
}
