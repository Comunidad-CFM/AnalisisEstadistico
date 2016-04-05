﻿using AnalisisEstadistico.Model;
using Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AnalisisEstadistico.Modulos
{
    public class FB
    {
        public string accessToken;
        public string allMessages;
        public string[] posts;
        public double[] langPercents;
        public double[] langCount;
        public List<Post> postList;

        public FB(string token)
        {
            this.accessToken = token;
            this.postList = new List<Post>();
            this.allMessages = "";
        }

        public void splitPosts()
        {
            this.allMessages = this.allMessages.Substring(1, this.allMessages.Length - 2);
            this.posts = Regex.Split(this.allMessages, "}{");
            string eachPost = "";

            foreach (string post in this.posts)
            {
                eachPost += post + "\n\n";
            }

            this.allMessages = eachPost;
        }

        public void getPosts()
        {
            var client = new FacebookClient(this.accessToken);
            var result2 = client.Get("/me/feed?limit=10000").ToString();
            JObject obj = JObject.Parse(result2);
            JToken jUser = obj["data"];
            int numb = jUser.Count();
            string data;
            JObject post;

            for (int i = 0; i < numb; i++)
            {
                data = obj["data"][i].ToString();
                post = JObject.Parse(data);
                var msgProperty = post.Property("message");

                if (msgProperty != null)
                    this.allMessages += "{ " + msgProperty.Value.ToString() + " }";
            }

            splitPosts();
        }

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

            int totalPosts = this.postList.Count;

            foreach (Post post in postList)
            {
                if (post.lang == "Spanish")
                {
                    contSpanish++;
                }
                else if (post.lang == "English")
                {
                    contEnglish++;
                }
                else if (post.lang == "German")
                {
                    contGerman++;
                }
                else if (post.lang == "Dutch")
                {
                    contDutch++;
                }
                else
                {
                    contUnknown++;
                }
            }

            percentSpanish = (contSpanish * 100) / totalPosts;
            percentEnglish = (contEnglish * 100) / totalPosts;
            percentGerman = (contGerman * 100) / totalPosts;
            percentDutch = (contDutch * 100) / totalPosts;
            percentUnknown = (contUnknown * 100) / totalPosts;

            this.langPercents = new double[] { percentSpanish, percentEnglish, percentGerman, percentDutch, percentUnknown };
            this.langCount = new double[] { contSpanish, contEnglish, contGerman, contDutch, contUnknown };
        }

     
        public void postsAnalysis()
        {
            Language language;
            Post post;

            foreach (string eachPost in this.posts)
            {
                language = new Language();
                post = new Post();
                language.text = eachPost;
                post.msg = eachPost;
                post.lang = language.languageAnalisys();

                this.postList.Add(post);
            }
        }
    }
}