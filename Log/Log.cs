using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebCrawler.Logger
{
    public class Log
    {
        private const string LOGFILE_PATH = "crawler.log";
        private const string TIMESTAMP_PATTERN = "dd-MM-yyyy HH:mm:ss";
        private static Mutex logMutex = new Mutex();

        private static Log _instance;
        public static Log Instance { 
            get
            {
                logMutex.WaitOne();
                if (_instance == null)
                    _instance = new Log();
                logMutex.ReleaseMutex();

                return _instance;
            }
        }

        private Log()
        {
            Stream stream = new FileStream(LOGFILE_PATH, FileMode.OpenOrCreate);
            Debug.Listeners.Add(new TextWriterTraceListener(stream));
        }

        public void Write(LogLevel level, string msg)
        {
            logMutex.WaitOne();
            Debug.WriteLine("[{0}] {1}\t{2}", DateTime.Now.ToString(TIMESTAMP_PATTERN), level, msg);
            logMutex.ReleaseMutex();
        }
    }
}
