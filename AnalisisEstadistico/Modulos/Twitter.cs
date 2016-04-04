using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TweetSharp;

namespace AnalisisEstadistico.Modulos
{
    public class Twitter
    {
        public string tweets;
        public Language language;
        public List<Tweet> tweetList;
        public string path;
        public double[] langPercents;
        public double[] langCount;
        public List<string> contentJson;

        public Twitter(string path)
        {
            this.language = new Language();
            this.tweetList = new List<Tweet>();
            this.contentJson = new List<string>();
            this.path = path;
            this.getJson();
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

        protected string readJson(string ruta)
        {
            string content = "";
            content = File.ReadAllText(ruta, Encoding.UTF8);

            content = Regex.Replace(content, "<.*?>", string.Empty);
            return content.ToLower().Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Replace("&", "");
        }

        public void getJson()
        {
            try
            {
                DirectoryInfo Dir = new DirectoryInfo(this.path);
                FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.AllDirectories);
                
                foreach (FileInfo file in FileList)
                {
                    if (file.Extension == ".json")
                    {
                        this.contentJson.Add(readJson(file.FullName));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Función para determinar la cantidad y porcentaje de referencias en un  determinado idioma
        /// </summary>
        public void generalAnalysis()
        {
            // cantidad de mensajes en un determinado idioma
            double contSpanish = 0,
                   contEnglish = 0,
                   contGerman = 0,
                   contDutch = 0,
                   contUnknown = 0;

            // porcentaje de mensajes en un idioma determinado
            double percentSpanish,
                   percentEnglish,
                   percentGerman,
                   percentDutch,
                   percentUnknown;

            int totalTweets = this.tweetList.Count;

            foreach (Tweet tweet in tweetList)
            {
                if (tweet.lang == "Spanish" || tweet.lang == "es")
                {
                    contSpanish++;
                }
                else if (tweet.lang == "English" || tweet.lang == "en")
                {
                    contEnglish++;
                }
                else if (tweet.lang == "German" || tweet.lang == "de")
                {
                    contGerman++;
                }
                else if (tweet.lang == "Dutch" || tweet.lang == "nl")
                {
                    contDutch++;
                }
                else
                {
                    contUnknown++;
                }
            }

            percentSpanish = (contSpanish * 100) / totalTweets;
            percentEnglish = (contEnglish * 100) / totalTweets;
            percentGerman = (contGerman * 100) / totalTweets;
            percentDutch = (contDutch * 100) / totalTweets;
            percentUnknown = (contUnknown * 100) / totalTweets;

            this.langPercents = new double[] { percentSpanish, percentEnglish, percentGerman, percentDutch, percentUnknown };
            this.langCount = new double[] { contSpanish, contEnglish, contGerman, contDutch, contUnknown };

        }

        public void tweetsAnalysis()
        {
            JToken token;
            Tweet tweet;
            string[] infoJson;

            foreach (string eachJson in this.contentJson)
            {
                infoJson = eachJson.Split('\n');

                foreach (string subJson in infoJson)
                {
                    try
                    {
                        tweet = new Tweet();
                        token = JObject.Parse(subJson);
                        this.language.text = token.SelectToken("text").ToString();
                        tweet.lang = this.language.languageAnalisys();
                        tweet.user = token.SelectToken("user").SelectToken("name").ToString();
                        tweet.msg = token.SelectToken("text").ToString();

                        if (tweet.lang.Equals("Unknown"))
                        {
                            tweet.lang = token.SelectToken("user").SelectToken("lang").ToString();
                        }

                        this.tweetList.Add(tweet);
                    }
                    catch { }
                }
            }
        }
    }
}