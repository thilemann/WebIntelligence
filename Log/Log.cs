using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Logger
{
    public class Log
    {
        private const string LOGFILE_PATH = "crawler.log";
        private const string TIMESTAMP_PATTERN = "dd-MM-yyyy SS:mm:ss";

        private static Log _instance;
        public static Log Instance { 
            get
            {
                return _instance ?? (_instance = new Log());
            }
        }

        private Log()
        {
            Stream stream = new FileStream(LOGFILE_PATH, FileMode.OpenOrCreate);
            Debug.Listeners.Add(new TextWriterTraceListener(stream));
        }

        public void Write(LogLevel level, string msg)
        {
            Debug.WriteLine("[{0}] {1}\t{2}", DateTime.Now.ToString(TIMESTAMP_PATTERN), level, msg);
        }
    }
}
