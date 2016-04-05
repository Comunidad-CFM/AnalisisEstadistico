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
        public Sentiment sentiment;
        public List<Tweet> tweetList;
        public string path;
        public double[] langPercents;
        public double[] langCount;
        public List<string> contentJson;
        public List<string> eachTweetUser;
        public List<string> differentUsers;

        public Twitter(string path)
        {
            this.tweetList = new List<Tweet>();
            this.contentJson = new List<string>();
            this.path = path;
            this.differentUsers = new List<string>();
            this.getJson();
        }

        public Twitter()
        {
            this.eachTweetUser = new List<string>();
            this.tweetList = new List<Tweet>();
            this.differentUsers = new List<string>();
        }
    
        /// <summary>
        /// Función para obtener los tweets de un usuario en una lista
        /// </summary>
        /// <param name="content">Corresponde a una cadena que indica el nombre de usuario y la cantidad máxima de twees que desea obtener</param>
        /// <returns></returns>
        public string searchTweets(string content) 
        {
            string[] info = content.Split(' ');// separa el nombre de la cantidad de tweets
            string contentTweets = "";
            var service = new TwitterService("C98uX0MU7n24kXYROPs1YfZGd", "nDEBrbJXszSZKfrOmmDfAm4NNrvDsfqkE5BwvsXsdFVZKJMdQg");

            //AuthenticateWith("Access Token", "AccessTokenSecret");
            service.AuthenticateWith("711043579699982336-scPSu5YliFK6yov7Jf5aQOLrtklaQFU", "ZiwI7zz8oAX37Ht7jLJ0rjlzaT44CQdsyzjarz1xTRmOC");

            //ScreenName="nombre del usuario", Count=numero de Tweets
            IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = info[0], Count = Int32.Parse(info[1]), });
            var tweetsList = tweets.ToList();
            
            for (int i = 0; i < tweetsList.Count; i++)
            {
                contentTweets += tweetsList[i].Text + "\n\n";
                this.eachTweetUser.Add(tweetsList[i].Text);
            }

            this.tweets = contentTweets;
            return contentTweets;
        }

        /// <summary>
        /// Analiza los tweets de un usuario específico.
        /// </summary>
        public void tweetsAnalysisForUser()
        {
            Tweet tweet;

            foreach (string eachTweet in this.eachTweetUser)
            {
                tweet = new Tweet();
                this.language = new Language();
                this.language.text = eachTweet;
                tweet.lang = this.language.languageAnalisys();
                tweet.msg = eachTweet;

                if(tweet.lang.Equals("Spanish"))
                {
                    this.sentiment = new Sentiment(eachTweet);
                    tweet.sentiment = this.sentiment.sentimentAnalysis();
                }

                this.tweetList.Add(tweet);
            }
        }

        // Tweets masivos
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

        public bool existUser(string user) 
        {
            foreach (string eachUser in this.differentUsers)
            {
                if (eachUser.Equals(user))
                {
                    return true;
                }
            }

            return false;
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

            foreach (Tweet tweet in this.tweetList)
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

                if (!existUser(tweet.user))
                {
                    this.differentUsers.Add(tweet.user);
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
                        this.language = new Language();
                        this.language.text = token.SelectToken("text").ToString();
                        tweet.lang = this.language.languageAnalisys();
                        tweet.user = token.SelectToken("user").SelectToken("name").ToString();
                        tweet.msg = token.SelectToken("text").ToString();

                        if (tweet.lang.Equals("Unknown"))
                        {
                            tweet.lang = token.SelectToken("user").SelectToken("lang").ToString();
                        }

                        if (tweet.lang.Equals("Spanish"))
                        {
                            this.sentiment = new Sentiment(tweet.msg);
                            tweet.sentiment = this.sentiment.sentimentAnalysis();
                        }

                        this.tweetList.Add(tweet);
                    }
                    catch { }
                }
            }
        }
    }
}