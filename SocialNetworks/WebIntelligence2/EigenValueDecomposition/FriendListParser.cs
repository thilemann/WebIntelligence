using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Accord.Math;

namespace EigenValueDecomposition
{
    class FriendListParser
    {
        private const string UserPattern = @"user:\s(.+)";
        private const string FriendsPattern = @"friends:\t(.+)";
        private const string SummaryPattern = @"summary:\s(.+)";
        private const string ReviewPattern = @"review:\s(.+)";

        private readonly string _filePath;

        private Community _community;
        public Community Community
        {
            get { return _community; }
        }

        public FriendListParser(string filePath)
        {
            this._filePath = filePath;
            _community = new Community();
        }


        public void Parse()
        {
            StreamReader reader = new StreamReader(_filePath);
            Regex userRegex = new Regex(UserPattern, RegexOptions.IgnoreCase);
            Regex friendsRegex = new Regex(FriendsPattern, RegexOptions.IgnoreCase);
            Regex summaryRegex = new Regex(SummaryPattern, RegexOptions.IgnoreCase);
            Regex reviewRegex = new Regex(ReviewPattern, RegexOptions.IgnoreCase);

            string name = null;
            string[] friends = null;
            string summary = null;
            string review = null;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line == null)
                    continue;

                if (userRegex.IsMatch(line))
                {
                    name = userRegex.Match(line).Groups[1].Value;
                }
                else if (friendsRegex.IsMatch(line))
                {
                    friends = friendsRegex.Match(line).Groups[1].Value.Split('\t');
                }
                else if (summaryRegex.IsMatch(line))
                {
                    summary = summaryRegex.Match(line).Groups[1].Value;
                }
                else if (reviewRegex.IsMatch(line))
                {
                    review = reviewRegex.Match(line).Groups[1].Value;
                    _community.AddUser(new User(name, (friends == null) ? new List<string>() : friends.ToList(), summary, review));
                    name = null;
                    friends = null;
                    summary = null;
                    review = null;
                }
            }
        }

    }
}
