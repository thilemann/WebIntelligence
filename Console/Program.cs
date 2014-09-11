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
            //Crawler crawler = new Crawler("Seeds.txt");
            //crawler.Start();

            Uri uri = new Uri("http://www.kabelchristensen.dk/Sitemap");
            Sitemap smap = new Sitemap(uri);

            foreach (var url in smap.Contents)
            {
                Console.WriteLine(url.Location + "\t\t\t" + url.LastModified);
            }
            Console.ReadLine();
        }
    }
}
