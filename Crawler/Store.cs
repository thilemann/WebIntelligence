using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace WebCrawler.Crawl
{
    public class Store
    {
        private readonly string _outputPath;
        private readonly Dictionary<string, string> _fileToDomainMap;
        private Mutex storeMutex;

        public Store()
        {
            storeMutex = new Mutex();
            DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "pages"));
            _outputPath = directoryInfo.ToString();
            _fileToDomainMap = new Dictionary<string, string>();
        }

        public void WriteFile(WebPage page)
        {
            storeMutex.WaitOne();
            if (_fileToDomainMap.ContainsKey(page.Uri.AbsoluteUri))
                return;

            Guid guid = Guid.NewGuid();
            string fileName = guid.ToString() + ".html";

            page.SavePage(Path.Combine(_outputPath, fileName));

            _fileToDomainMap.Add(page.Uri.AbsoluteUri, fileName);
            storeMutex.ReleaseMutex();
        }

        public void WriteFileMap()
        {
            using(StreamWriter writer = new StreamWriter(File.Open(Path.Combine(_outputPath, "_filemap.csv"), FileMode.OpenOrCreate)))
            {
                foreach (var pair in _fileToDomainMap)
                {
                    writer.WriteLine(pair.Key + ";" + pair.Value);
                }
            }
        }

        public void LoadFileMap()
        {
            using (StreamReader reader = new StreamReader(File.Open(Path.Combine(_outputPath, "_filemap.csv"), FileMode.OpenOrCreate)))
            {
                while (!reader.EndOfStream)
                {
                    string[] array = reader.ReadLine().Split(';');

                    _fileToDomainMap.Add(array[0], array[1]);
                }
            }

        }



    }
}
