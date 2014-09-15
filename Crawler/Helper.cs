using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Logger;

namespace WebCrawler.Crawl
{
    public class Helper
    {
        public static BlockingCollection<Uri> GetSeeds(string seedsPath)
        {
            string[] seeds = File.ReadAllLines(seedsPath);
            BlockingCollection<Uri> queue = new BlockingCollection<Uri>();

            foreach (var seed in seeds)
            {
                if (string.IsNullOrEmpty(seed))
                {
                    continue;
                }

                try
                {
                    queue.Add(new Uri(seed));
                }
                catch (UriFormatException e)
                {
                    throw e;
                }
            }
            return queue;
        }

        public static IPAddress ResolveDNS(string hostName)
        {
            IPAddress address;
            try
            {
                address = Dns.GetHostEntry(hostName).AddressList.FirstOrDefault();
            }
            catch (SocketException e)
            {
                throw e;
            }
            return address;
        }
    }
}
