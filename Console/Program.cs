using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Crawl;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Crawler crawler = new Crawler("Ressources\\Seeds.txt");
            Console.WriteLine("Starting...");
            crawler.Start(1000);
            Console.WriteLine("Finished crawling...");

            //Uri uri = new Uri("http://www.kabelchristensen.dk/Sitemap");
            //Sitemap smap = new Sitemap(uri);

            //foreach (var url in smap.Contents)
            //{
            //    Console.WriteLine(url.Location + "\t\t\t" + url.LastModified);
            //}
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
