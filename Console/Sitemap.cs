using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net;

namespace WebCrawler
{
    public class Sitemap
    {
        public Sitemap(Uri uri)
        {
            this.uri = uri;
        }

        private Uri uri;

        public Uri Uri
        {
            get { return uri; }
        }

        private XmlDocument xmlSitemap;

        public XmlDocument XmlSitemap
        {
            get { return xmlSitemap; }
        }

        private List<SitemapUrl> contents;

        public List<SitemapUrl> Contents
        {
            get 
            {
                if (this.contents != null)
                {
                    return contents;
                }
                else
                {
                    return Load();
                }
            }
        }

        private ChangeFreqEnum changeFreq;

        public ChangeFreqEnum ChangeFreq
        {
            get { return changeFreq; }
        }

        private double priority;

        public double Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        

        private List<SitemapUrl> Load()
        {
            List<SitemapUrl> list = new List<SitemapUrl>();
            xmlSitemap = new XmlDocument();
            xmlSitemap.Load(uri.AbsoluteUri);
            // TODO: Inefficient to resolve uri twice!
            using (XmlReader reader = XmlReader.Create(uri.AbsoluteUri))
            {
                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    // TODO: Use Linq to XML to parse instead and add changefrequency and priority
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name.ToLower() == "url"))
                    {
                        reader.ReadToDescendant("loc");

                        string link = null;
                        string lastmod = null;

                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name.ToLower() == "loc"))
                        {
                            try
                            {
                                link = reader.ReadElementContentAsString();
                            }
                            catch (Exception) { continue; }
                        }

                        reader.ReadToNextSibling("lastmod");

                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name.ToLower() == "lastmod"))
                        {
                            try
                            {
                                lastmod = reader.ReadElementContentAsString();
                            }
                            catch (Exception) { continue; }
                        }

                        if (link != null && lastmod != null)
                        {
                            list.Add(new SitemapUrl(new Uri(link), DateTime.Parse(lastmod)));
                        }
                    }
                }
            }
            return list;
        }

        public ChangeFreqEnum GetChangeFrequency(string value)
        {
            switch (value.ToLower().Trim())
            {
                case "always":
                    return Sitemap.ChangeFreqEnum.Always;
                case "hourly":
                    return Sitemap.ChangeFreqEnum.Hourly;
                case "daily":
                    return Sitemap.ChangeFreqEnum.Daily;
                case "weekly":
                    return Sitemap.ChangeFreqEnum.Weekly;
                case "monthly":
                    return Sitemap.ChangeFreqEnum.Monthly;
                case "yearly":
                    return Sitemap.ChangeFreqEnum.Yearly;
            }
            return Sitemap.ChangeFreqEnum.Never;
        }

        public class SitemapUrl
        {
            public SitemapUrl(Uri location, DateTime lastmod)
            {
                this.Location = location;
                this.LastModified = lastmod;
            }

            public Uri Location { get; set; }
            public DateTime LastModified { get; set; }
        }

        public enum ChangeFreqEnum
        {
            Always,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Yearly,
            Never
        }
        
    }
}
