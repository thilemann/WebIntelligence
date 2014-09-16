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
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
