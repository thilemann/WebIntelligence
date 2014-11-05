using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Sentiment;

namespace SocialMediaAnalysis
{
    class Program
    {

        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("[START]\tTraining started");
            NaiveBayesClassifier sentimentClassifier = new NaiveBayesClassifier();
            sentimentClassifier.Train(new TestDataParser("Resources/SentimentTrainingData.txt"));
            Console.WriteLine("[END]\tTraining");
            Console.WriteLine();
            Console.WriteLine("[START]\tFriendlist parsing");
            FriendListParser parser = new FriendListParser("Resources/friendships.reviews.txt");
            parser.Parse();
            Console.WriteLine("[END]\t\tFriendlist parsing");
            Console.WriteLine();
            Console.WriteLine("[START]\t\tIdentifying communities");
            CommunityIdentifier communityIdentifier = new CommunityIdentifier(parser.Community);
            List<Community> subCommunities = communityIdentifier.Identify();
            Console.WriteLine("[END]\t\tIdentifying communities");
            Console.WriteLine();
            Console.WriteLine("[START]\t\tAnalysing communities");
            CommunityAnalyzer communityAnalyzer = new CommunityAnalyzer(sentimentClassifier, subCommunities);
            string filename = "result.csv";
            communityAnalyzer.Analyze(filename);
            Console.WriteLine("[END]\t\tAnalysing communities: Results written to {0}", filename);

            Console.WriteLine("Identied communities: " + subCommunities.Count);
            Console.WriteLine("\nSub communities:");
            foreach (var community in subCommunities)
            {
                Console.WriteLine(community.Size);
                community.Dispose();
            }
            communityAnalyzer.Dispose();
            communityIdentifier.Dispose();
            sentimentClassifier.Dispose();

            Console.WriteLine(DateTime.Now.Subtract(start).ToString("g"));
            Console.WriteLine("Finished everything. Press to exit");
            Console.ReadLine();

        }

        static void PrintCommunity(Community community)
        {
            Console.WriteLine("Users:");
            foreach (var user in community.Users)
            {
                Console.WriteLine(user.ToString());
            }
            Matrix<double> matrix = community.AdjacencyMatrix;
            Console.WriteLine("\nAdjacency Matrix:");
            for (int i = 0; i < matrix.RowCount; i++)
            {
                string line = string.Join("\t", matrix.Row(i));
                Console.WriteLine(line);
            }
        }
    }
}
