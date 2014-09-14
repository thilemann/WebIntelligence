using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawler = new Crawler("Ressources\\Seeds.txt");
            Console.WriteLine("Starting...");
            DateTime startTime = DateTime.Now;
            crawler.Start();
            DateTime endTime = DateTime.Now;
            Console.WriteLine("Finished crawling {0} pages in {1}", crawler.CrawledPages, endTime.Subtract(startTime));

            //Uri uri = new Uri("http://www.kabelchristensen.dk/Sitemap");
            //Sitemap smap = new Sitemap(uri);

            //foreach (var url in smap.Contents)
            //{
            //    Console.WriteLine(url.Location + "\t\t\t" + url.LastModified);
            //}
            Console.ReadLine();
        }
    }
}
