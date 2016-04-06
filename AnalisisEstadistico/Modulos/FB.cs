using AnalisisEstadistico.Model;
using Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AnalisisEstadistico.Modulos
{
    /// <summary>
    /// Clase para realizar el análisis de facebook
    /// Atributos:
    /// accessToken: Token de acceso para realizar la petición a facebook
    /// posts: arreglo con los post recogidos
    /// langPercents: arreglo con los porcentajes correspondientes a cada lenguaje
    /// langCount: arreglo con los contadores de resultados positivos para cada lenguaje
    /// postList: lista de tipo post, utilizada para almacenar la información pertinente a cada publicación
    /// </summary>
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

        /// <summary>
        /// Separa los post de un determinado usuario y los agrega a una lista.
        /// </summary>
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

        /// <summary>
        /// Obtiene los post de un determinado usuario.
        /// </summary>
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

        /// <summary>
        /// Determina la cantidad y porcentaje de referencias en un determinado idioma en los posts.
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

            int totalPosts = this.postList.Count;

            foreach (Post post in postList)
            {
                if (post.lang == "Español")
                {
                    contSpanish++;
                }
                else if (post.lang == "Inglés")
                {
                    contEnglish++;
                }
                else if (post.lang == "Alemán")
                {
                    contGerman++;
                }
                else if (post.lang == "Holandés")
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

        /// <summary>
        /// Se encarga de llamar a las funciones que realizan el analisis de los posts.
        /// </summary>
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