using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace SocialMediaAnalysis
{
    class Program
    {

        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            FriendListParser parser = new FriendListParser("Resources/friendships.reviews.txt");
            //FriendListParser parser = new FriendListParser("Resources/TextFile1.txt");
            parser.Parse();

            //PrintCommunity(parser.Community);

            CommunityIdentifier communityIdentifier = new CommunityIdentifier(parser.Community);
            List<Community> subCommunities = communityIdentifier.Identify();
            Console.WriteLine("Identied communities: " + subCommunities.Count);
            Console.WriteLine("\nSub communities:");
            foreach (var community in subCommunities)
            {
                Console.WriteLine(community.Size);
            }

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
