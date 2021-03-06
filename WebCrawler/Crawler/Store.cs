﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace WebCrawler.Core
{
    public class Store
    {
        private readonly string _outputPath;
        private ConcurrentDictionary<string, string> _fileToDomainMap;

        public string OutputPath
        {
            get { return _outputPath; }
        }

        public Store()
        {
            _outputPath = Logger.Log.outputFolder;
            _fileToDomainMap = new ConcurrentDictionary<string,string>();
        }

        public void WriteFile(WebPage page)
        {
            if (_fileToDomainMap.ContainsKey(page.Uri.AbsoluteUri))
            {
                return;
            }

            Guid guid = Guid.NewGuid();
            string fileName = guid.ToString() + ".html";

            page.SavePage(Path.Combine(_outputPath, fileName));

            _fileToDomainMap.TryAdd(page.Uri.AbsoluteUri, fileName);
        }

        public void WriteFileMap()
        {
            using(StreamWriter writer = new StreamWriter(File.Open(Path.Combine(_outputPath, "_filemap.csv"), FileMode.OpenOrCreate)))
            {
                foreach (var pair in _fileToDomainMap)
                {
                    writer.WriteLine(pair.Key + "¤" + pair.Value);
                }
            }
            _fileToDomainMap = null;
        }

        public Dictionary<string, string> LoadFileMap()
        {
            using (StreamReader reader = new StreamReader(File.Open(Path.Combine(_outputPath, "_filemap.csv"), FileMode.OpenOrCreate)))
            {
                while (!reader.EndOfStream)
                {
                    string[] array = reader.ReadLine().Split('¤');

                    _fileToDomainMap.TryAdd(array[0], array[1]);
                }
            }
            return new Dictionary<string, string>(_fileToDomainMap);
        }
    }
}
