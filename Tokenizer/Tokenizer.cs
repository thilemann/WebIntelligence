using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using HtmlAgilityPack;
using AdvancedAgilityPack;
using System.Text.RegularExpressions;

namespace Indexing.Token
{
    public class Tokenizer
    {
        private static string[] STOP_WORDS = new[] { "a", "about", "above", "after", "again", "against", "all", "am", "an", "and", "any", "are", "aren't", "as", "at", "be", "because", "been", "before", "being", "below", "between", "both", "but", "by", "can't", "cannot", "could", "couldn't", "did", "didn't", "do", "does", "doesn't", "doing", "don't", "down", "during", "each", "few", "for", "from", "further", "had", "hadn't", "has", "hasn't", "have", "haven't", "having", "he", "he'd", "he'll", "he's", "her", "here", "here's", "hers", "herself", "him", "himself", "his", "how", "how's", "i", "i'd", "i'll", "i'm", "i've", "if", "in", "into", "is", "isn't", "it", "it's", "its", "itself", "let's", "me", "more", "most", "mustn't", "my", "myself", "no", "nor", "not", "of", "off", "on", "once", "only", "or", "other", "ought", "our", "ours	ourselves", "out", "over", "own", "same", "shan't", "she", "she'd", "she'll", "she's", "should", "shouldn't", "so", "some", "such", "than", "that", "that's", "the", "their", "theirs", "them", "themselves", "then", "there", "there's", "these", "they", "they'd", "they'll", "they're", "they've", "this", "those", "through", "to", "too", "under", "until", "up", "very", "was", "wasn't", "we", "we'd", "we'll", "we're", "we've", "were", "weren't", "what", "what's", "when", "when's", "where", "where's", "which", "while", "who", "who's", "whom", "why", "why's", "with", "won't", "would", "wouldn't", "you", "you'd", "you'll", "you're", "you've", "your", "yours", "yourself", "yourselves" };

        public Tokenizer()
        {

        }

        public IEnumerable<string> Tokenize(string fileContent)
        {
            string cleanContent = stripHtml(fileContent);

            List<string> tokens = cleanContent.Split(null).ToList();
            tokens.RemoveAll(x => x.Equals(String.Empty));

            return tokens.Except(STOP_WORDS);
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
