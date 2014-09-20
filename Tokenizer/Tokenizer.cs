using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using HtmlAgilityPack;
using AdvancedAgilityPack;
using System.Text.RegularExpressions;

namespace Indexer.Token
{
    public class Tokenizer
    {
        public Tokenizer()
        {

        }

        public List<string> Tokenize(string fileContent)
        {
            string cleanContent = stripHtml(fileContent);

            List<string> tokens = cleanContent.Split(null).ToList();
            tokens.RemoveAll(x => x.Equals(String.Empty));

            return tokens;
        }

        private string stripHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            Sanitizer sanitizer = new Sanitizer();

            string sanitizedText = sanitizer.SanitizeHtml(html);
            sanitizedText = sanitizer.SanitizeText(sanitizedText);

            return sanitizedText;
        }

    }
}
