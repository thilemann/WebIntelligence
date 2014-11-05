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
                if (!string.Equals(user.Text, "*"))
                {
                    List<string> friends = user.Friends;
                    foreach (var friendName in friends)
                    {
                        User friend = FindUser(community, friendName);
                        if (friend == null)
                            continue;

                        string review = string.Join(" ", friend.FullReview);

                        scorePos += _sentimentClassifier.Score(review, Label.Pos);
                        scoreNeg += _sentimentClassifier.Score(review, Label.Neg);
                        Label score = DetermineScore(scorePos, scoreNeg);
                        WriteDecision(user.Name, score);
                    }
                }
                else
                {
                    scorePos += _sentimentClassifier.Score(user.FullReview, Label.Pos);
                    scoreNeg += _sentimentClassifier.Score(user.FullReview, Label.Neg);
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
            int rating = 1;
            if (score == Label.Pos)
                rating = 5;

            writer.WriteLine(string.Join("\t", name, rating, "*"));
        }

        private Label DetermineScore(double scorePos, double scoreNeg)
        {
            double diff = scorePos - scoreNeg;
            Label result;
            if (diff < 0.4)
                result = Label.Neg;
            else if (diff >= 0.6)
                result = Label.Pos;
            else
                result = Label.Neutral;

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
        }
    }
}
