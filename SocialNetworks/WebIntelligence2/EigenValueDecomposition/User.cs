using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace EigenValueDecomposition
{
    /*
     *  user: lottie
        friends:	Jeramy	rosa	chloe	diahann	Gregg	laina	tonye	milena	joelle	Trevor	haily	Emanuel	cilka	rheta	alysa
        summary: *
        text: *
     */

    class User
    {
        public User(string name, List<string> friends, string summary, string review)
        {
            Name = name;
            Friends = friends;
            Summary = summary;
            Review = review;
        }

        public string Name { get; set; }

        public List<string> Friends { get; set; }

        public string Summary { get; set; }

        public string Review { get; set; }

        public bool HasFriend(string name)
        {
            return Friends.Contains(name);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Name: " + Name);
            sb.AppendLine("Friends: " + string.Join("\t", Friends));
            sb.AppendLine("Summary: " + Summary);
            sb.AppendLine("Review: " + Review);
            return sb.ToString();
        }
    }
}
