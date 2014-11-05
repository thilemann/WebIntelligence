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
            ////const string testString_1 = "<:) >:) :) :-) :-D :-d :P :d :D :-) " +
            ////                          "test :hej :-lort (-: (: et ord her, flere ord!! FUCK lort";
            ////const string testString_2 = "et ord... her, flere ord!! FUCK lort";

            ////Tokenize(testString_2, true);            

            //ReviewListParser parser = new ReviewListParser("Resources/SentimentTrainingData.txt");
            //parser.Parse();

            //NaiveBayesClassifier classifier = new NaiveBayesClassifier(parser.Reviews);

            //Console.WriteLine("\nPres any key to quit!");
            //Console.ReadLine();

            Tokenizer t = new Tokenizer();

            Console.WriteLine("heeeeeeeeeeeeeeeeeeeeeeeeeeeyyaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Console.WriteLine(t.MatchLengthening("heeeeeeeeeeeeeeeeeeeeeeeeeeeyyaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"));
        }

        
    }
}
