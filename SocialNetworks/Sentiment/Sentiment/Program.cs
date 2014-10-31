using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sentiment
{
    class Program
    {   
        public static void Main()
        {
            const string testString_1 = "<:) >:) :) :-) :-D :-d :P :d :D :-) " +
                                      "test :hej :-lort (-: (: et ord her, flere ord!! FUCK lort";
            const string testString_2 = "et ord... her, flere ord!! FUCK lort";

            Tokenize(testString_2, true);            

            Console.WriteLine("\nPres any key to quit!");
            Console.ReadLine();
        }

        public static MatchCollection MatchEmoticons(string s, out string result)
        {
            string pattern = "";
            pattern = @"[<>]?";                         // hat
            pattern += @"[:;=8]";                       // eyes
            pattern += @"[\-o\*\']?";                   // optional nose
            pattern += @"[\)\]\(\[dDpP/\:\}\{@\|\\]";   // mouth
            pattern += @"|";                            // OR 
            pattern += @"[\)\]\(\[dDpP/\:\}\{@\|\\]";   // mouth
            pattern += @"[\-o\*\']?";                   // optional nose
            pattern += @"[:;=8]";                       // eyes
            pattern += @"[<>]?";                        // hat

            Regex regex = new Regex(pattern);
            result = regex.Replace(s, "");
            return regex.Matches(s);        
        }

        public static MatchCollection MatchWords(string s)
        {
            string pattern = "";
            pattern = @"(?:[a-z][a-z'\-_]+[a-z])";          // Words with apostrophes or dashes.
            pattern += @"|";                                // OR
            pattern += @"(?:[+\-]?\d+[,/.:-]\d+[+\-]?)";    // Numbers, including fractions, decimals.
            pattern += @"|";                                // OR
            pattern += @"(?:[\w_]+)";                       // Words without apostrophes or dashes.
            pattern += @"|";                                // OR
            pattern += @"(?:\.(?:\s*\.){1,})";              // Ellipsis dots.
            pattern += @"|";                                // OR
            pattern += @"(?:\S)";                           // Everything else that isn't whitespace.

            Regex regex = null;

            regex = new Regex(pattern, RegexOptions.None);
            return regex.Matches(s);  
        }

        public static MatchCollection MatchShouting(string s)
        {
            throw new NotImplementedException();
        }

        public static MatchCollection MatchMaskedCursing(string s)
        {
            throw new NotImplementedException();
        }

        public static MatchCollection MatchLengthening(string s)
        {
            throw new NotImplementedException();
        }

        public static void Tokenize(string s, bool preserveCase)
        {
            string result;
            MatchCollection emoticons = MatchEmoticons(s, out result);
            
            MatchCollection words = null;
            words = MatchWords(result ?? s); // if no emoticons are matched, use input string
        }
    }
}
