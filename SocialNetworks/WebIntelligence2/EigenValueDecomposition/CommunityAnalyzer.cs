using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentiment;

namespace SocialMediaAnalysis
{
    class CommunityAnalyzer : IDisposable
    {
        private List<Community> _communities;
        private NaiveBayesClassifier _sentimentClassifier;
        private StreamWriter writer;

        public CommunityAnalyzer(NaiveBayesClassifier sentimentClassifier, List<Community> communities)
        {
            _communities = communities;
            _sentimentClassifier = sentimentClassifier;
        }

        public void Analyze(string filePath)
        {
            writer = new StreamWriter(filePath);
            foreach (var community in _communities)
            {
                AnalyseCommunity(community);
            }
        }

        private void AnalyseCommunity(Community community)
        {
            List<User> users = community.Users;
            foreach (var user in users)
            {
                double scorePos = 0;
                double scoreNeg = 0;
                int scoreCount = 0;
                if (System.String.CompareOrdinal(user.FullReview, "* *") == 0)
                {
                    List<string> friends = user.Friends;
                    foreach (var friendName in friends)
                    {
                        User friend = FindUser(community, friendName);
                        string review = friend.FullReview;
                        if (System.String.CompareOrdinal(review, "* *") == 0) // Ensure that the friend has written a review
                            continue;

                        double pos = _sentimentClassifier.Score(review, Label.Pos);
                        double neg = _sentimentClassifier.Score(review, Label.Neg);

                        if (friend.CommunityId != user.CommunityId || System.String.CompareOrdinal(friend.Name, "kyle") == 0)
                        {
                            pos *= 10;
                            neg *= 10;
                            scoreCount += 10;
                        }
                        else
                        {
                            scoreCount++;   
                        }
                        scorePos += pos;
                        scoreNeg += neg;
                    }
                    Label score = DetermineScore(scorePos/scoreCount, scoreNeg/scoreCount);
                    WriteDecision(user.Name, score);
                }
                else
                {
                    scorePos = _sentimentClassifier.Score(user.FullReview, Label.Pos);
                    scoreNeg = _sentimentClassifier.Score(user.FullReview, Label.Neg);
                    Label score = DetermineScore(scorePos, scoreNeg);
                    WriteScore(user.Name, score);
                }
            }
        }

        private void WriteDecision(string name, Label score)
        {
            string decision = "no";
            if (score == Label.Pos)
                decision = "yes";

            writer.WriteLine(string.Join("\t", name, "*", decision));
        }

        private void WriteScore(string name, Label score)
        {
            int rating = 3;
            if (score == Label.Pos)
                rating = 5;
            else if (score == Label.Neg)
                rating = 1;

            writer.WriteLine(string.Join("\t", name, rating, "*"));
        }

        private Label DetermineScore(double scorePosAvg, double scoreNegAvg)
        {
            double diff = scorePosAvg - scoreNegAvg;
            Label result = Label.Neutral;

            if (diff > 0.1)
                result = Label.Pos;
            else if (diff < -0.1)
                result = Label.Neg;

            return result;
        }

        private User FindUser(Community startCommunity, string name)
        {
            User user = startCommunity.GetUser(name);
            if (user == null)
            {
                foreach (var community in _communities)
                {
                    if (community.Id.Equals(startCommunity.Id))
                        continue;

                    user = community.GetUser(name);
                    if (user != null)
                        break;
                }
            }
            return user;
        }

        public void Dispose()
        {
            writer.Close();
            writer.Dispose();
            foreach (var community in _communities)
            {
                community.Dispose();
                _sentimentClassifier.Dispose();
            }
        }
    }
}
