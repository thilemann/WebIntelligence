using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Crawl
{
    public class Helper
    {
        public static Queue<Uri> GetSeeds(string seedsPath)
        {
            string[] seeds = File.ReadAllLines(seedsPath);
            Queue<Uri> queue = new Queue<Uri>();

            foreach (var seed in seeds)
            {
                if (string.IsNullOrEmpty(seed))
                {
                    continue;
                }

                try
                {
                    queue.Enqueue(new Uri(seed));
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
