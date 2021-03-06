﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaAnalysis
{
    /*
     *  user: lottie
        friends:	Jeramy	rosa	chloe	diahann	Gregg	laina	tonye	milena	joelle	Trevor	haily	Emanuel	cilka	rheta	alysa
        summary: *
        text: *
     */

    class User
    {
        public User(string name, List<string> friends, string summary, string text)
        {
            Name = name;
            Friends = friends;
            Summary = summary;
            Text = text;
        }

        public Guid CommunityId { get; set; }

        public string Name { get; set; }

        public List<string> Friends { get; set; }

        public string Summary { get; set; }

        public string Text { get; set; }

        public string FullReview
        {
            get { return string.Join(" ", Summary, Text); }
        }

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
            sb.AppendLine("Text: " + Text);
            return sb.ToString();
        }
    }
}
