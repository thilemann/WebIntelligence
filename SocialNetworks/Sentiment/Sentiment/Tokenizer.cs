﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Sentiment
{
    class Tokenizer
    {
        private const string negationStartPattern =
            @"never|no|nothing|nowhere|noone|none|not|havent|hasnt|hadnt|cant|couldnt|shouldnt|wont|wouldnt|dont|doesnt|didnt|isnt|arent|aint|n't";
        private const string negationStopPattern =
            @"[.:;!?]";
        private const string NegSuffix = "_NEG";

        private MatchCollection MatchEmoticons(string s, out string result)
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

        private MatchCollection MatchWords(string s)
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

        private bool IsShouting(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!Char.IsUpper(s[i]))
                {
                    return false;
                }
            }

        return true;
        }

        private string MatchGrawlix(string s)
        {
            string swearing = Regex.Escape("!#¤%&?@£$€^~+*()");
            const string excludeChars = "(?![\\!]{1,}|[\\?]{1,}|[\\.]{1,})";
            string pattern = @"(?!\s)*(" + excludeChars + "[" + swearing + "]){3,}";

            Regex regex = new Regex(pattern);
            return regex.Replace(s, "_GRAWLIX");
        }

        private string MatchMaskedCursing(string s)
        {
            string maskingChars = Regex.Escape("*");
            string pattern = @"(?!\s)*((\w)*[" + maskingChars + @"]+(\w)*)+(?!\s)*";

            Regex regex = new Regex(pattern);
            return regex.Replace(s, "_CURSE");
        }

        private string Lengthening(string s)
        {
            int count = 0;
            char letter = s[s.Length - 1];

            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (count < 3)
                {
                    if (letter == s[i])
                    {
                        count += 1;
                    }
                    else if (letter != s[i])
                    {
                        letter = s[i];
                        count = 1;
                    }
                }
                else if (count >= 3)
                {
                    if (letter == s[i])
                    {
                        s = s.Remove(i, 1);
                        count += 1;
                    }
                    if (letter != s[i])
                    {
                        letter = s[i];
                        count = 1;
                    }
                }
            }

            return s;
        }

        private bool StartNegation(string token)
        {
            return Regex.IsMatch(token, negationStartPattern);
        }

        private bool StopNegation(String token)
        {
            return Regex.IsMatch(token, negationStopPattern);
        }

        public List<string> Tokenize(string s)
        {
            List<string> tokensList = new List<string>();
            string result;
            MatchCollection emoticons = MatchEmoticons(s, out result);
            foreach (Match emoticonMatch in emoticons) // Add emoticons
            {
                tokensList.Add(emoticonMatch.Value);
            }  

            result = MatchMaskedCursing(result);
            result = MatchGrawlix(result);
 
            MatchCollection matchWords = MatchWords(result);
            bool isNegating = false;
            foreach (Match match in matchWords)
            {
                string word = match.Value;
                if (!IsShouting(word))
                    word = word.ToLower();

                word = Lengthening(word);

                if (!isNegating && StartNegation(word))
                {
                    tokensList.Add(word);
                    isNegating = true;
                }
                else if (isNegating && StopNegation(word))
                {
                    tokensList.Add(word);
                    isNegating = false;
                }
                else
                {
                    if (isNegating)
                        word += NegSuffix;
                    
                    tokensList.Add(word);
                }
            }
            return tokensList;
        }
    }
}
