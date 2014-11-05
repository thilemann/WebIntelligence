using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentiment;

namespace SocialMediaAnalysis
{
    class CommunityAnalyzer
    {
        private List<Community> _communities;
        private NaiveBayesClassifier sentimentClassifier;

        public CommunityAnalyzer(List<Community> communities)
        {
            _communities = communities;
            sentimentClassifier = new NaiveBayesClassifier();
        }

        public void Analyze()
        {
            
        }

        private string AnalyseCommunity(Community community)
        {
            List<User> users = community.Users;
            foreach (var user in users)
            {
                double score = 0;
                List<string> friends = user.Friends;
                foreach (var friend in friends)
                {
                    
                }
            }
            return null;
        }

        private User findUser(Community startCommunity, string name)
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
    }
}
