using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class Helpers
    {
        public static StreamReader GetStream(Uri uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            // Sends the HttpWebRequest and waits for the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Gets the stream associated with the response.
            Stream stream = response.GetResponseStream();
            Encoding encoding = System.Text.Encoding.GetEncoding("utf-8");

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader reader = new StreamReader(stream, encoding);
            return reader;
        }
    }
}
