using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebCrawler
{
    public class Store
    {
        private string outputPath;
        private Dictionary<string, string> map;

        public Store()
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "pages"));
            outputPath = directoryInfo.ToString();
            map = new Dictionary<string, string>();
        }

        public void WriteFile(WebPage page)
        {
            if (map.ContainsKey(page.Uri.AbsoluteUri))
                return;

            Guid guid = Guid.NewGuid();
            string fileName = guid.ToString() + ".html";

            page.SavePage(Path.Combine(outputPath, fileName));

            map.Add(page.Uri.AbsoluteUri, fileName);
        }

        public void WriteFileMap()
        {
            using(StreamWriter writer = new StreamWriter(File.Open(Path.Combine(outputPath, "_filemap.csv"), FileMode.OpenOrCreate)))
            {
                foreach (var pair in map)
                {
                    writer.WriteLine(pair.Key.ToString() + ";" + pair.Value);
                }
            }

        }

        public void LoadFileMap()
        {
            using (StreamReader reader = new StreamReader(File.Open(Path.Combine(outputPath, "_filemap.csv"), FileMode.OpenOrCreate)))
            {
                while (!reader.EndOfStream)
                {
                    string[] array = reader.ReadLine().Split(';');

                    map.Add(array[0], array[1]);
                }
            }

        }



    }
}
