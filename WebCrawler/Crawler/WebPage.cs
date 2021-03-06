﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using WebCrawler.Logger;

namespace WebCrawler.Core
{
    public class WebPage
    {
        private readonly HtmlDocument _html;
        private readonly Log _logger;
        private Uri _uri;
        private int _addressHash;

        public int Address
        {
            get
            {
                return _addressHash;
            }
        }

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
                _addressHash = Helper.ResolveDNS(uri.DnsSafeHost).Address.GetHashCode();
            }
            catch (Exception e)
            {
                _addressHash = new IPAddress(new byte[] { 0, 0, 0, 0 }).Address.GetHashCode();
                _logger.Write(LogLevel.Error, "WebPage: DNS Resolving failed");
                _logger.Write(LogLevel.Error, uri.DnsSafeHost);
                _logger.Write(LogLevel.Error, e.ToString());
            }
            IsLoaded = false;
        }

        public void LoadPage()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_uri);
            request.Timeout = 5000;

            // Sends the HttpWebRequest and waits for the response.
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                // Gets the stream associated with the response.
                Stream stream = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("utf-8");

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader reader = new StreamReader(stream, encoding);

                try
                {
                    _html.LoadHtml(reader.ReadToEnd());
                    IsLoaded = true;
                }
                catch (OutOfMemoryException e)
                {
                    _logger.Write(LogLevel.Error, string.Format("WebPage.cs: Uri '{0}' was too large to load into memory", _uri.AbsoluteUri));
                    _logger.Write(LogLevel.Error, e.ToString());
                }
                catch (IOException e)
                {
                    _logger.Write(LogLevel.Error, string.Format("WebPage.cs: Uri '{0}' input could not be read", _uri.AbsoluteUri));
                    _logger.Write(LogLevel.Error, e.ToString());
                }
            }
            catch (WebException e)
            {
                _logger.Write(LogLevel.Error, "WebPage: Could not load page");
                _logger.Write(LogLevel.Error, _uri.AbsoluteUri);
                _logger.Write(LogLevel.Error, e.ToString());
            }
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
