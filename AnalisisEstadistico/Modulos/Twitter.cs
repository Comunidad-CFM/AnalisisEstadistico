using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TweetSharp;

namespace AnalisisEstadistico.Modulos
{
    public class Twitter
    {
        public string tweets;

        public Twitter() 
        { 
        }

        public string searchTweets(string content) 
        {
            string[] info = content.Split(' ');
            string contentTweets = "";
            var service = new TwitterService("C98uX0MU7n24kXYROPs1YfZGd", "nDEBrbJXszSZKfrOmmDfAm4NNrvDsfqkE5BwvsXsdFVZKJMdQg");

            //AuthenticateWith("Access Token", "AccessTokenSecret");
            service.AuthenticateWith("711043579699982336-scPSu5YliFK6yov7Jf5aQOLrtklaQFU", "ZiwI7zz8oAX37Ht7jLJ0rjlzaT44CQdsyzjarz1xTRmOC");

            //ScreenName="screeen name not username", Count=Number of Tweets / www.Twitter.com/mcansozeri. 
            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = info[0], Count = Int32.Parse(info[1]), });
            var tweetsList = tweets.ToList();
            
            for (int i = 0; i < tweetsList.Count; i++)
            {
                contentTweets += tweetsList[i].Text + "\n\n";
            }

            this.tweets = contentTweets;
            return contentTweets;
        }

        public string tweetsAnalysis()
        {
            return "tweetsAnalysis";
        }
    }
}