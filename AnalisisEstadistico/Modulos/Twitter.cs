using AnalisisEstadistico.Model;
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
    /// <summary>
    /// Clase para realizar el análisis en los tweets
    /// </summary>
    public class Twitter
    {
        public string tweets;// texto con los tweets almacenados todos en un string
        public Language language;// instancia para realizar el análisis
        public NaiveBayes clasificador;
        public List<Tweet> tweetList; // lista de tweets para el análisis
        public string path; // url para leer archivos con tweets masivos
        public double[] langPercents;// porcentaje de lenguajes encontrados en los msjs masivos
        public double[] langCount;// contadores de resultados positivos por lenguaje
        public List<string> contentJson;// lista de string, que contiene cada tweet representado como un json
        public List<string> eachTweetUser;// cada tweet leído para una  persona
        public List<string> differentUsers;// lista con los diferentes usuarios(sin repetir)
        public double[] catPercents;

        public Twitter(string path)// cuando son tweets masivos
        {
            this.tweetList = new List<Tweet>();
            this.contentJson = new List<string>();
            this.path = path;
            this.differentUsers = new List<string>();
            this.getJson();
        }

        public Twitter()// cuando es un único usuario, dados sus credenciales
        {
            this.path = "";
            this.eachTweetUser = new List<string>();
            this.tweetList = new List<Tweet>();
            this.differentUsers = new List<string>();
        }
    
        /// <summary>
        /// Obtiene los tweets de un usuario.
        /// </summary>
        /// <param name="content">Corresponde a una cadena que indica el nombre de usuario y la cantidad máxima de twees que desea obtener.</param>
        /// <returns>String con los twwets.</returns>
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
                // Se almacena cada tweet en una lista para luego realizar el analisis.
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

                this.tweetList.Add(tweet);
            }
        }

        /// <summary>
        /// Lee el contenido de un archivo json.
        /// </summary>
        /// <param name="ruta">Ruta en donde se encuentra el archivo.</param>
        /// <returns>Contenido del json.</returns>
        protected string readJson(string ruta)
        {
            string content = "";
            content = File.ReadAllText(ruta, Encoding.UTF8);

            content = Regex.Replace(content, "<.*?>", string.Empty);
            return content.ToLower().Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Replace("&", "");
        }

        /// <summary>
        /// Obtiene todos los archivos json de la ruta especificada en la variable path de la clase.
        /// </summary>
        public void getJson()
        {
            try
            {
                DirectoryInfo Dir = new DirectoryInfo(this.path);
                FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.AllDirectories);
                
                // Recorre cada uno de los archivos de la ruta.
                foreach (FileInfo file in FileList)
                {
                    if (file.Extension == ".json")
                    {
                        // Pide el contenido del json y lo almacena en una lista.
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
        /// Busca si un usuario ya se encuentra en la lista.
        /// </summary>
        /// <param name="user">Usuario a buscar.</param>
        /// <returns>Booleano si lo encuentra o no.</returns>
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
        /// Determina la cantidad y porcentaje de referencias en un determinado idioma en los tweets.
        /// </summary>
        public void generalAnalysis()
        {
            // porcentaje de mensajes en un idioma determinado
            double percentSpanish,
                   percentEnglish,
                   percentGerman,
                   percentDutch,
                   percentUnknown,
                   catArt = 0,
                   catTec = 0,
                   catDep = 0,
                   catMed = 0, 
                   catEco = 0, 
                   catGas = 0,
                   contSpanish = 0,
                   contEnglish = 0,
                   contGerman = 0,
                   contDutch = 0,
                   contUnknown = 0;

            int totalTweets = this.tweetList.Count;

            foreach (Tweet tweet in this.tweetList)
            {
                if (tweet.category == "Arte y Cultura")
                {
                    catArt += 1;
                }
                else if (tweet.category == "Ciencia y Tecnologia")
                {
                    catTec += 1;
                }
                else if (tweet.category == "Deportes")
                {
                    catDep += 1;
                }
                else if (tweet.category == "Medio Ambiente")
                {
                    catMed += 1;
                }
                else if (tweet.category == "Economia y Negocios")
                {
                    catEco += 1;
                }
                else if (tweet.category == "Gastronomía")
                {
                    catGas += 1;
                }

                if (tweet.lang == "Español" || tweet.lang == "es")
                {
                    contSpanish++;
                }
                else if (tweet.lang == "Inglés" || tweet.lang == "en")
                {
                    contEnglish++;
                }
                else if (tweet.lang == "Alemán" || tweet.lang == "de")
                {
                    contGerman++;
                }
                else if (tweet.lang == "Holandés" || tweet.lang == "nl")
                {
                    contDutch++;
                }
                else
                {
                    contUnknown++;
                }

                // Solo para cuando se analizan tweets masivos.
                if (!this.path.Equals(""))
                {
                    if (!existUser(tweet.user))
                    {
                        this.differentUsers.Add(tweet.user);
                    }
                }
            }

            percentSpanish = (contSpanish * 100) / totalTweets;
            percentEnglish = (contEnglish * 100) / totalTweets;
            percentGerman = (contGerman * 100) / totalTweets;
            percentDutch = (contDutch * 100) / totalTweets;
            percentUnknown = (contUnknown * 100) / totalTweets;

            // Se almacenan los resultados para mostrarlos en los graficos.
            this.langPercents = new double[] { percentSpanish, percentEnglish, percentGerman, percentDutch, percentUnknown };
            this.langCount = new double[] { contSpanish, contEnglish, contGerman, contDutch, contUnknown };
            this.catPercents = new double[] { (catArt * 100) / totalTweets, (catTec * 100) / totalTweets, (catDep * 100) / totalTweets, (catMed * 100) / totalTweets, (catEco * 100) / totalTweets, (catGas * 100) / totalTweets };
        }

        public string getCategory(List<Categoria> resultados)
        {
            double porcentaje = 0.0d, porcentajeTemp;
            string nombreCat = "";

            foreach (Categoria cat in resultados)
            {
                porcentajeTemp = cat.Porcentaje;
                if (porcentajeTemp > porcentaje)
                {
                    porcentaje = porcentajeTemp;
                    nombreCat = cat.NombreCategoria;
                }
            }
            return nombreCat;
        }

        /// <summary>
        /// Se encarga de llamar a las funciones que realizan el analisis de los tweets.
        /// </summary>
        public void tweetsAnalysis()
        {
            JToken token;
            Tweet tweet;
            string[] infoJson;

            // Se recorre cada uno de los json.
            foreach (string eachJson in this.contentJson)
            {
                // Se separa el contenido del json por cambio de linea, esto para recorrer uno a uno cada tweet contenido en el json.
                infoJson = eachJson.Split('\n');

                // Se recorre cada uno de los tweets.
                foreach (string subJson in infoJson)
                {
                    // Se utiliza un try porque algunos de los tweets no tienen el formato correcto, al parsear a json se cae.
                    try
                    {
                        tweet = new Tweet();
                        token = JObject.Parse(subJson);
                        this.language = new Language();
                        this.language.text = token.SelectToken("text").ToString();
                        tweet.lang = this.language.languageAnalisys();
                        tweet.user = token.SelectToken("user").SelectToken("name").ToString();
                        tweet.msg = token.SelectToken("text").ToString();

                        // Si el lenguaje del texto es desconocido, se usa el lenguaje del usuario.
                        if (tweet.lang.Equals("Desconocido"))
                        {
                            tweet.lang = token.SelectToken("user").SelectToken("lang").ToString();
                        }

                        this.clasificador = new NaiveBayes(tweet.lang);
                        List<string> listaPalabras = clasificador.dividirTexto(tweet.msg);
                        List<Categoria> resultados = clasificador.clasificar(listaPalabras);
                        tweet.category = getCategory(resultados);

                        // Se agrega el tweet a una lista para luego analizarlo.
                        this.tweetList.Add(tweet);
                    }
                    catch { }
                }
            }
        }
    }
}