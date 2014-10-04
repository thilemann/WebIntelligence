using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using WebCrawler.Core;
using Indexing.Core;
using Indexing.Token;
using Indexing.Linguistics;
using Ranking.Core;


namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Crawler crawler = new Crawler("Ressources\\Seeds.txt");

            //Console.WriteLine("Starting...");
            //crawler.Start(50);
            //Console.WriteLine("Finished crawling...");
            //Console.WriteLine("Press any key to start indexing...");
            //Console.ReadLine();

            //Store fileStore = new Store();
            //Indexer indexer = new Indexer();
            //Dictionary<string, string> filesMap = fileStore.LoadFileMap();
            //Console.WriteLine("Indexing started");
            //DateTime start = DateTime.Now;
            //int filesIndexed = 0;
            //foreach (var file in filesMap)
            //{
            //    string fileContent = File.ReadAllText(Path.Combine(fileStore.OutputPath, file.Value));
            //    indexer.Index(file.Key, fileContent);
            //    Console.SetCursorPosition(0, 8);
            //    Console.WriteLine("Pages indexed {0} / {1}", ++filesIndexed, filesMap.Count);
            //}
            //DateTime end = DateTime.Now;
            //TimeSpan timeElapsed = end.Subtract(start);
            //double pagesPerSec = filesMap.Count / timeElapsed.TotalSeconds;
            //Console.WriteLine("Indexed {0} pages per second", pagesPerSec);
            //Console.WriteLine("Indexing finished in {0}", timeElapsed.ToString(@"hh\:mm\:ss"));

            //Ranker ranker = new Ranker();
            //string searchQuery = "Abu Qatada Acquitted on Terrorism Charge";

            //Tokenizer tokenizer = new Tokenizer();
            //List<string> queryTerms = tokenizer.TokenizeQuery(searchQuery).ToList();
            //for (int i = 0; i < queryTerms.Count(); i++)
            //{
            //    Stemmer stemmer = new Stemmer();
            //    stemmer.Stem(queryTerms[i]);
            //    queryTerms[i] = stemmer.ToString();
            //}


            //IEnumerable<KeyValuePair<int, double>> scores = ranker.Rank(queryTerms, indexer.Terms, indexer.TotalDocuments);

            //int resultCount = 1;
            //foreach (var item in scores)
            //{
            //    string url = indexer.GetUrlFromHash(item.Key);
            //    if (url == null)
            //        continue;
            //    Console.WriteLine("{0}: {1}", resultCount++, url);
            //    if (resultCount > 10)
            //        break;
            //}

            PageRank pageRank = new PageRank();
            float[] vec = pageRank.DoRank();
            for (int i = 0; i < vec.Length; i++)
            {
                Console.Write(vec[i] + " ");
            }
            Console.Write("\n");  

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
