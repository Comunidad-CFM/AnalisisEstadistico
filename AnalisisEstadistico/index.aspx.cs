using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;
using Ionic.Zip;
using Microsoft.Office;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using TweetSharp;
using Newtonsoft.Json.Linq;
using AnalisisEstadistico.Data;
using AnalisisEstadistico.Model;

namespace AnalisisEstadistico
{
    public partial class index : System.Web.UI.Page
    {
        List<feelingWord> feelingWords = new List<feelingWord>();
        List<emoji> emojis = new List<emoji>();
        List<stopword> stopWords = new List<stopword>();
        List<string> positiveCorpus = new List<string>();
        List<string> negativeCorpus = new List<string>();
        List<enhancer> enhancers = new List<enhancer>();
        List<string> reducers = new List<string>();

        List<feelingWord> feelingWordsList = new List<feelingWord>();
        List<emoji> emojisList = new List<emoji>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Add words to positivecorpus list for find a new word
            //positiveCorpus.Add("and");
            //positiveCorpus.Add("y");
            //positiveCorpus.Add("mas");

            //negativeCorpus.Add("but");
            //negativeCorpus.Add("or");
            //negativeCorpus.Add("either");
            //negativeCorpus.Add("pero");
            //negativeCorpus.Add("o");
        }

        protected void readFolder(object sender, EventArgs e)
        {
            resultBox.Text = "";
            try
            {
                DirectoryInfo Dir = new DirectoryInfo(textFolder.Text);
                FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo FI in FileList)
                {
                    resultBox.Text = resultBox.Text + FI.FullName + "\n";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Método utilizado para descomprimir los archivos de un zip file
        /// </summary>
        /// <param name="ArchivoZip">Ruta donde se encuentra el archivo ZIP
        /// <param name="RutaGuardar">Ruta donde se guardaran los archivos extraídos del ZIP
        /// <returns></returns>
        protected bool extract(string archivoZip, string rutaGuardar)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(archivoZip))
                {
                    zip.ExtractAll(rutaGuardar);
                    zip.Dispose();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void choose(string fileName, string dir)
        {
            string fileExtention = getFileExtention(fileName);

            if (fileExtention == ".txt" || fileExtention == ".html" || fileExtention == ".json" || fileExtention == ".xml")
            {
                readTxt(fileName, dir);
            }
            else if (fileExtention == ".doc" || fileExtention == ".docx")
            {
                readDoc(fileName, dir);
            }
            else if (fileExtention == ".zip")
            {
                // Almacenar el .zip en la carpeta zips del proyecto
                string ruta = Server.MapPath("~") + "zips\\" + fileReader.FileName;
                string rutaDescomprimir = Server.MapPath("~") + "zips\\descomprimidos\\";
                saveFile(ruta);

                if (extract(ruta, rutaDescomprimir))
                {
                    // Averiguar extention file in zip
                    DirectoryInfo dirInfo = new DirectoryInfo(rutaDescomprimir);
                    FileInfo[] files = dirInfo.GetFiles();

                    /*foreach (System.IO.FileInfo file in files)
                    {
                        choose(file.Name);
                    }*/
                    choose(files[0].Name, "zips\\descomprimidos\\");
                }
                else
                {
                    contentBox.Text = "Fallo al descomprimir";
                }
            }
            else
            {
                contentBox.Text = "unkown";
            }
        }

        protected string getFileExtention(string file)
        {
            string fileExtention = System.IO.Path.GetExtension(file);

            if (fileExtention == ".txt" || fileExtention == ".html" || fileExtention == ".doc" || fileExtention == ".docx"
                || fileExtention == ".zip" || fileExtention == ".json" || fileExtention == ".xml")
            {
                return fileExtention;
            }
            else
            {
                return "unkown";
            }
        }

        protected void readTxt(string fileName, string dir)
        {
            string ruta = Server.MapPath("~") + dir + fileName;
            if (dir != "zips\\descomprimidos\\")
                saveFile(ruta);

            string content = File.ReadAllText(ruta, Encoding.UTF8);
            contentBox.Text = content;
        }

        protected void readDoc(string fileName, string dir)
        {
            string ruta = Server.MapPath("~") + dir + fileName;
            if (dir != "zips\\descomprimidos\\")
                saveFile(ruta);

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = ruta;
            object readOnly = true;
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            string totaltext = "";
            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                totaltext += " \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString();
            }
            contentBox.Text = totaltext;
            docs.Close();
            word.Quit();
        }

        protected void saveFile(string ruta)
        {
            fileReader.SaveAs(ruta);
        }

        protected void buttonUpload_Click(object sender, EventArgs e)
        {
            choose(fileReader.FileName, "files\\");
        }

        protected void buttonCargar_Click(object sender, EventArgs e)
        {
            string link = textLink.Text;
            WebClient client = new WebClient();
            byte[] byteData = null;
            byteData = client.DownloadData(link);

            UTF8Encoding UTF8Encod = new UTF8Encoding();
            contentBox.Text = Regex.Replace(UTF8Encod.GetString(byteData), "<(.|\\n)*?>", string.Empty);
        }

        protected void searchTweets(object sender, EventArgs e)
        {
            string txtTwitterName = textTwitter.Text;

            if (txtTwitterName != "")
            {
                var service = new TwitterService("C98uX0MU7n24kXYROPs1YfZGd", "nDEBrbJXszSZKfrOmmDfAm4NNrvDsfqkE5BwvsXsdFVZKJMdQg");

                //AuthenticateWith("Access Token", "AccessTokenSecret");
                service.AuthenticateWith("711043579699982336-scPSu5YliFK6yov7Jf5aQOLrtklaQFU", "ZiwI7zz8oAX37Ht7jLJ0rjlzaT44CQdsyzjarz1xTRmOC");

                //ScreenName="screeen name not username", Count=Number of Tweets / www.Twitter.com/mcansozeri. 
                IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = txtTwitterName, Count = 10, });
                var tweetsList = tweets.ToList();
                string strTweets = "Tweets de " + txtTwitterName + ":\n";
                for (int i = 0; i < tweetsList.Count; i++)
                {
                    strTweets += tweetsList[i].Text + "\n";
                }

                contentBox.Text = strTweets;
            }
        }

        protected void identificarIdioma(List<string> strWords)
        {
            // contadores de coincidencias
            int contEsp = 0;
            int contEng = 0;
            int contPort = 0;
            int contAlem = 0;

            //listas con palabras que coinciden
            List<Coincidencia> coincidenciasEsp = new List<Coincidencia>();
            List<Coincidencia> coincidenciasEng = new List<Coincidencia>();
            List<Coincidencia> coincidenciasAlem = new List<Coincidencia>(); ;
            List<Coincidencia> coincidenciasPort = new List<Coincidencia>(); ;

            using (var db = new AnalizadorBDEntities())
            {
                foreach (string strWord in strWords)
                {
                    var wordsEsp = from w in db.words where w.idiomID == 1 && w.word1 == strWord select w;
                    var wordsEng = from w in db.words where w.idiomID == 2 && w.word1 == strWord select w;
                    var wordsAlem = from w in db.words where w.idiomID == 3 && w.word1 == strWord select w;
                    var wordsPort = from w in db.words where w.idiomID == 4 && w.word1 == strWord select w;

                    if (wordsEsp.Count() > 0)
                    {
                        contEsp++;
                        if (coincidenciasEsp.Count() < 1)// si la lista no tiene coincidencias registradas
                        {
                            Coincidencia coincidencia = new Coincidencia(strWord);
                            coincidenciasEsp.Add(coincidencia);
                        }
                        else
                        {
                            coincidenciasEsp = registrarCoincidencia(strWord, coincidenciasEsp);
                        }

                    }
                    if (wordsEng.Count() > 0)
                    {
                        contEng++;
                        if (coincidenciasEng.Count() < 1)// si la lista no tiene coincidencias registradas
                        {
                            Coincidencia coincidencia = new Coincidencia(strWord);
                            coincidenciasEng.Add(coincidencia);
                        }
                        else
                        {
                            coincidenciasEng = registrarCoincidencia(strWord, coincidenciasEng);
                        }
                    }
                    if (wordsAlem.Count() > 0)
                    {
                        contAlem++;
                        if (coincidenciasAlem.Count() < 1)// si la lista no tiene coincidencias registradas
                        {
                            Coincidencia coincidencia = new Coincidencia(strWord);
                            coincidenciasAlem.Add(coincidencia);
                        }
                        else
                        {
                            coincidenciasAlem = registrarCoincidencia(strWord, coincidenciasAlem);
                        }
                    }
                    if (wordsPort.Count() > 0)
                    {
                        contPort++;
                        if (coincidenciasPort.Count() < 1)// si la lista no tiene coincidencias registradas
                        {
                            Coincidencia coincidencia = new Coincidencia(strWord);
                            coincidenciasPort.Add(coincidencia);
                        }
                        else
                        {
                            coincidenciasPort = registrarCoincidencia(strWord, coincidenciasPort);
                        }
                    }
                }
                string strApariciones = contEsp + ", " + contEng + ", " + contAlem + ", " + contPort;
                int porcentajeEsp = (contEsp * 100) / strWords.Count();
                int porcentajeEng = (contEng * 100) / strWords.Count();
                int porcentajeAlem = (contAlem * 100) / strWords.Count();
                int porcentajePort = (contPort * 100) / strWords.Count();

                string idiomaTexto;

                if (porcentajeEsp > porcentajeEng && porcentajeEsp > porcentajeAlem && porcentajeEsp > porcentajePort)
                {
                    idiomaTexto = "El texto se encuentra en Español";
                }
                else if (porcentajeEng > porcentajeEsp && porcentajeEng > porcentajeAlem && porcentajeEng > porcentajePort)
                {
                    idiomaTexto = "El texto se encuentra en Inglés";
                }
                else if (porcentajeAlem > porcentajeEsp && porcentajeAlem > porcentajeEng && porcentajeAlem > porcentajePort)
                {
                    idiomaTexto = "El texto se encuentra en Alemán";
                }
                else if (porcentajePort > porcentajeEsp && porcentajePort > porcentajeEng && porcentajePort > porcentajeAlem)
                {
                    idiomaTexto = "El texto se encuentra en Portugués";
                }
                else
                {
                    idiomaTexto = "No se ha podido definir el idioma del texto";
                }

                Console.WriteLine(porcentajeEsp);
                Console.WriteLine(strApariciones);
            }
        }

        /// <summary>
        /// Método para registrar la incidencia de una palabra, si ya está registrada esa palabra entonces se modifica el
        /// numero de apariciones de esa palabra
        /// </summary>
        /// <param name="palabra">Palabra a buscar</param>
        /// <param name="coincidencias">Lista donde están registradas la coincidencias</param>
        /// <returns></returns>
        protected List<Coincidencia> registrarCoincidencia(string palabra, List<Coincidencia> coincidencias)
        {
            foreach (Coincidencia item in coincidencias)
            {
                if (item.palabra == palabra)
                {
                    item.apariciones++;
                    return coincidencias;
                }
            }
            Coincidencia coincidencia = new Coincidencia(palabra);
            coincidencias.Add(coincidencia);
            return coincidencias;

        }


        //////////// ANALISIS DEL SENTIMIENTO
        protected void insertIntoStopWords()
        {
            stopWords.Add(new stopword { word = "un" });
            stopWords.Add(new stopword { word = "una" });
            stopWords.Add(new stopword { word = "unas" });
            stopWords.Add(new stopword { word = "unos" });
            stopWords.Add(new stopword { word = "uno" });
            stopWords.Add(new stopword { word = "sobre" });
            stopWords.Add(new stopword { word = "todo" });
            stopWords.Add(new stopword { word = "tambien" });
            stopWords.Add(new stopword { word = "tras" });
            stopWords.Add(new stopword { word = "otro" });
            stopWords.Add(new stopword { word = "algun" });
            stopWords.Add(new stopword { word = "alguno" });
            stopWords.Add(new stopword { word = "alguna" });
            stopWords.Add(new stopword { word = "algunos" });
            stopWords.Add(new stopword { word = "algunas" });
            stopWords.Add(new stopword { word = "ser" });
            stopWords.Add(new stopword { word = "es" });
            stopWords.Add(new stopword { word = "soy" });
            stopWords.Add(new stopword { word = "eres" });
            stopWords.Add(new stopword { word = "somos" });
            stopWords.Add(new stopword { word = "sois" });
            stopWords.Add(new stopword { word = "estoy" });
            stopWords.Add(new stopword { word = "esta" });
            stopWords.Add(new stopword { word = "estamos" });
            stopWords.Add(new stopword { word = "estais" });
            stopWords.Add(new stopword { word = "estan" });
            stopWords.Add(new stopword { word = "como" });
            stopWords.Add(new stopword { word = "en" });
            stopWords.Add(new stopword { word = "para" });
            stopWords.Add(new stopword { word = "atras" });
            stopWords.Add(new stopword { word = "porque" });
            stopWords.Add(new stopword { word = "por" });
            stopWords.Add(new stopword { word = "que" });
            stopWords.Add(new stopword { word = "estado" });
            stopWords.Add(new stopword { word = "estaba" });
            stopWords.Add(new stopword { word = "ante" });
            stopWords.Add(new stopword { word = "antes" });
            stopWords.Add(new stopword { word = "siendo" });
            stopWords.Add(new stopword { word = "ambos" });
            stopWords.Add(new stopword { word = "pero" });
            stopWords.Add(new stopword { word = "poder" });
            stopWords.Add(new stopword { word = "puede" });
            stopWords.Add(new stopword { word = "puedo" });
            stopWords.Add(new stopword { word = "podemos" });
            stopWords.Add(new stopword { word = "podeis" });
            stopWords.Add(new stopword { word = "pueden" });
            stopWords.Add(new stopword { word = "fui" });
            stopWords.Add(new stopword { word = "fue" });
            stopWords.Add(new stopword { word = "fuimos" });
            stopWords.Add(new stopword { word = "fueron" });
            stopWords.Add(new stopword { word = "hacer" });
            stopWords.Add(new stopword { word = "hago" });
            stopWords.Add(new stopword { word = "hace" });
            stopWords.Add(new stopword { word = "hacemos" });
            stopWords.Add(new stopword { word = "haceis" });
            stopWords.Add(new stopword { word = "hacen" });
            stopWords.Add(new stopword { word = "cada" });
            stopWords.Add(new stopword { word = "fin" });
            stopWords.Add(new stopword { word = "incluso" });
            stopWords.Add(new stopword { word = "primero" });
            stopWords.Add(new stopword { word = "desde" });
            stopWords.Add(new stopword { word = "conseguir" });
            stopWords.Add(new stopword { word = "consigo" });
            stopWords.Add(new stopword { word = "consigue" });
            stopWords.Add(new stopword { word = "consigues" });
            stopWords.Add(new stopword { word = "conseguimos" });
            stopWords.Add(new stopword { word = "consiguen" });
            stopWords.Add(new stopword { word = "ir" });
            stopWords.Add(new stopword { word = "voy" });
            stopWords.Add(new stopword { word = "va" });
            stopWords.Add(new stopword { word = "vamos" });
            stopWords.Add(new stopword { word = "vais" });
            stopWords.Add(new stopword { word = "van" });
            stopWords.Add(new stopword { word = "ha" });
            stopWords.Add(new stopword { word = "tener" });
            stopWords.Add(new stopword { word = "tengo" });
            stopWords.Add(new stopword { word = "tiene" });
            stopWords.Add(new stopword { word = "tenemos" });
            stopWords.Add(new stopword { word = "teneis" });
            stopWords.Add(new stopword { word = "tienen" });
            stopWords.Add(new stopword { word = "el" });
            stopWords.Add(new stopword { word = "la" });
            stopWords.Add(new stopword { word = "lo" });
            stopWords.Add(new stopword { word = "las" });
            stopWords.Add(new stopword { word = "los" });
            stopWords.Add(new stopword { word = "su" });
            stopWords.Add(new stopword { word = "aqui" });
            stopWords.Add(new stopword { word = "mio" });
            stopWords.Add(new stopword { word = "tuyo" });
            stopWords.Add(new stopword { word = "ellos" });
            stopWords.Add(new stopword { word = "ellas" });
            stopWords.Add(new stopword { word = "nos" });
            stopWords.Add(new stopword { word = "nosotros" });
            stopWords.Add(new stopword { word = "vosotros" });
            stopWords.Add(new stopword { word = "vosotras" });
            stopWords.Add(new stopword { word = "si" });
            stopWords.Add(new stopword { word = "dentro" });
            stopWords.Add(new stopword { word = "solo" });
            stopWords.Add(new stopword { word = "solamente" });
            stopWords.Add(new stopword { word = "saber" });
            stopWords.Add(new stopword { word = "sabes" });
            stopWords.Add(new stopword { word = "sabe" });
            stopWords.Add(new stopword { word = "sabemos" });
            stopWords.Add(new stopword { word = "sabeis" });
            stopWords.Add(new stopword { word = "saben" });
            stopWords.Add(new stopword { word = "ultimo" });
            stopWords.Add(new stopword { word = "largo" });
            stopWords.Add(new stopword { word = "bastante" });
            stopWords.Add(new stopword { word = "haces" });
            stopWords.Add(new stopword { word = "muchos" });
            stopWords.Add(new stopword { word = "aquellos" });
            stopWords.Add(new stopword { word = "aquellas" });
            stopWords.Add(new stopword { word = "sus" });
            stopWords.Add(new stopword { word = "entonces" });
            stopWords.Add(new stopword { word = "tiempo" });
            stopWords.Add(new stopword { word = "dos" });
            stopWords.Add(new stopword { word = "bajo" });
            stopWords.Add(new stopword { word = "arriba" });
            stopWords.Add(new stopword { word = "encima" });
            stopWords.Add(new stopword { word = "usar" });
            stopWords.Add(new stopword { word = "uso" });
            stopWords.Add(new stopword { word = "usas" });
            stopWords.Add(new stopword { word = "usa" });
            stopWords.Add(new stopword { word = "usamos" });
            stopWords.Add(new stopword { word = "usais" });
            stopWords.Add(new stopword { word = "usan" });
            stopWords.Add(new stopword { word = "era" });
            stopWords.Add(new stopword { word = "eras" });
            stopWords.Add(new stopword { word = "eramos" });
            stopWords.Add(new stopword { word = "eran" });
            stopWords.Add(new stopword { word = "modo" });
            stopWords.Add(new stopword { word = "cual" });
            stopWords.Add(new stopword { word = "cuando" });
            stopWords.Add(new stopword { word = "donde" });
            stopWords.Add(new stopword { word = "mientras" });
            stopWords.Add(new stopword { word = "quien" });
            stopWords.Add(new stopword { word = "con" });
            stopWords.Add(new stopword { word = "entre" });
            stopWords.Add(new stopword { word = "sin" });
            stopWords.Add(new stopword { word = "yo" });
            stopWords.Add(new stopword { word = "aquel" });
            stopWords.Add(new stopword { word = "de" });
            stopWords.Add(new stopword { word = "se" });
            stopWords.Add(new stopword { word = "que" });
            stopWords.Add(new stopword { word = "a" });
            stopWords.Add(new stopword { word = "http" });
            stopWords.Add(new stopword { word = "del" });
            stopWords.Add(new stopword { word = "eso" });
            stopWords.Add(new stopword { word = "hay" });
            stopWords.Add(new stopword { word = "este" });
            stopWords.Add(new stopword { word = "tu" });
            stopWords.Add(new stopword { word = "x" });
            stopWords.Add(new stopword { word = "esa" });
            stopWords.Add(new stopword { word = "oye" });
            stopWords.Add(new stopword { word = "han" });
            stopWords.Add(new stopword { word = "segun" });
            stopWords.Add(new stopword { word = "cosa" });
            stopWords.Add(new stopword { word = "te" });
            stopWords.Add(new stopword { word = "al" });
        }

        protected void insertIntoEmojis()
        {
            // Add emojis to emojis list
            emojis.Add(new emoji { emoticon = ":-)", score = 3 });
            emojis.Add(new emoji { emoticon = ":)", score = 3 });
            emojis.Add(new emoji { emoticon = "=)", score = 3 });
            emojis.Add(new emoji { emoticon = ":D", score = 3 });
            emojis.Add(new emoji { emoticon = ":o)", score = 3 });
            emojis.Add(new emoji { emoticon = ":]", score = 3 });
            emojis.Add(new emoji { emoticon = ":3", score = 3 });
            emojis.Add(new emoji { emoticon = ":>", score = 3 });
            emojis.Add(new emoji { emoticon = "=]", score = 3 });
            emojis.Add(new emoji { emoticon = ":}", score = 3 });
            emojis.Add(new emoji { emoticon = ":-D", score = 3 });
            emojis.Add(new emoji { emoticon = "8-D", score = 3 });
            emojis.Add(new emoji { emoticon = "xD", score = 3 });
            emojis.Add(new emoji { emoticon = "X-D", score = 3 });
            emojis.Add(new emoji { emoticon = "XD", score = 3 });
            emojis.Add(new emoji { emoticon = "=-D", score = 3 });
            emojis.Add(new emoji { emoticon = "=D", score = 3 });
            emojis.Add(new emoji { emoticon = "=-3", score = 3 });
            emojis.Add(new emoji { emoticon = "=3", score = 3 });
            emojis.Add(new emoji { emoticon = ":'-)", score = 3 });
            emojis.Add(new emoji { emoticon = ":')", score = 3 });
            emojis.Add(new emoji { emoticon = ":*", score = 3 });
            emojis.Add(new emoji { emoticon = ";*", score = 3 });
            emojis.Add(new emoji { emoticon = "", score = 3 });
            emojis.Add(new emoji { emoticon = ";-)", score = 3 });
            emojis.Add(new emoji { emoticon = ";)", score = 3 });
            emojis.Add(new emoji { emoticon = ";-]", score = 3 });
            emojis.Add(new emoji { emoticon = ";]", score = 3 });
            emojis.Add(new emoji { emoticon = ";D", score = 3 });
            emojis.Add(new emoji { emoticon = ":-,", score = 3 });
            emojis.Add(new emoji { emoticon = ":P", score = 3 });
            emojis.Add(new emoji { emoticon = ";-P", score = 3 });
            emojis.Add(new emoji { emoticon = "X-P", score = 3 });
            emojis.Add(new emoji { emoticon = "xp", score = 3 });
            emojis.Add(new emoji { emoticon = "x-p", score = 3 });
            emojis.Add(new emoji { emoticon = ":-p", score = 3 });
            emojis.Add(new emoji { emoticon = ":p", score = 3 });
            emojis.Add(new emoji { emoticon = "=p", score = 3 });
            emojis.Add(new emoji { emoticon = "#-)", score = 3 });
            emojis.Add(new emoji { emoticon = ":v", score = 3 });
            emojis.Add(new emoji { emoticon = ":'v", score = 3 });
            emojis.Add(new emoji { emoticon = ";v", score = 3 });
            emojis.Add(new emoji { emoticon = "<3", score = 3 });
            emojis.Add(new emoji { emoticon = "^^", score = 3 });
            emojis.Add(new emoji { emoticon = "^.^", score = 3 });
            emojis.Add(new emoji { emoticon = "*.*", score = 3 });
            emojis.Add(new emoji { emoticon = ":-(", score = -3 });
            emojis.Add(new emoji { emoticon = ":(", score = -3 });
            emojis.Add(new emoji { emoticon = "=(", score = -3 });
            emojis.Add(new emoji { emoticon = ";(", score = -3 });
            emojis.Add(new emoji { emoticon = ":c", score = -3 });
            emojis.Add(new emoji { emoticon = ">:v", score = -3 });
            emojis.Add(new emoji { emoticon = ":'c", score = -3 });
            emojis.Add(new emoji { emoticon = ":-<", score = -3 });
            emojis.Add(new emoji { emoticon = ":<", score = -3 });
            emojis.Add(new emoji { emoticon = ":-[", score = -3 });
            emojis.Add(new emoji { emoticon = ":[", score = -3 });
            emojis.Add(new emoji { emoticon = ":{", score = -3 });
            emojis.Add(new emoji { emoticon = ":-|", score = -3 });
            emojis.Add(new emoji { emoticon = ":'(", score = -3 });
            emojis.Add(new emoji { emoticon = ":@", score = -3 });
            emojis.Add(new emoji { emoticon = ">:[", score = -3 });
            emojis.Add(new emoji { emoticon = "Q.Q", score = -3 });
            emojis.Add(new emoji { emoticon = ":#", score = -3 });
            emojis.Add(new emoji { emoticon = ":-#", score = -3 });
            emojis.Add(new emoji { emoticon = "-.-", score = -3 });
            emojis.Add(new emoji { emoticon = ".-.", score = -3 });
            emojis.Add(new emoji { emoticon = "._.", score = -3 });
            emojis.Add(new emoji { emoticon = "x_x", score = -3 });
            emojis.Add(new emoji { emoticon = "X_X", score = -3 });
            emojis.Add(new emoji { emoticon = "-.-'", score = -3 });
            emojis.Add(new emoji { emoticon = ":/", score = -3 });
            emojis.Add(new emoji { emoticon = ":-/", score = -3 });
            emojis.Add(new emoji { emoticon = ";/", score = -3 });
            emojis.Add(new emoji { emoticon = ":|", score = -3 });
            emojis.Add(new emoji { emoticon = "=_=", score = -3 });
            emojis.Add(new emoji { emoticon = "-_-", score = -3 });
            emojis.Add(new emoji { emoticon = "?_?", score = -3 });
            emojis.Add(new emoji { emoticon = "-\"-", score = -3 });

            using (var db = new AnalizadorBDEntities())
            {
                foreach (var emoticon in emojis)
                {
                    db.emojis.Add(emoticon);
                    db.SaveChanges();
                }
            }
        }

        protected void insertIntoFeelingWords()
        {
            feelingWords.Add(new feelingWord { word = "abiertamente", score = 1 });
            feelingWords.Add(new feelingWord { word = "abrazo", score = 3 });
            feelingWords.Add(new feelingWord { word = "absoluta", score = 2 });
            feelingWords.Add(new feelingWord { word = "absolutamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "absorbe", score = 1 });
            feelingWords.Add(new feelingWord { word = "absorbente", score = 2 });
            feelingWords.Add(new feelingWord { word = "abunda", score = 2 });
            feelingWords.Add(new feelingWord { word = "abundan", score = 2 });
            feelingWords.Add(new feelingWord { word = "abundancia", score = 3 });
            feelingWords.Add(new feelingWord { word = "abundante", score = 3 });
            feelingWords.Add(new feelingWord { word = "acariciado", score = 4 });
            feelingWords.Add(new feelingWord { word = "accesible", score = 4 });
            feelingWords.Add(new feelingWord { word = "acelerado", score = 2 });
            feelingWords.Add(new feelingWord { word = "aceptable", score = 3 });
            feelingWords.Add(new feelingWord { word = "aceptación", score = 4 });
            feelingWords.Add(new feelingWord { word = "aceptada", score = 3 });
            feelingWords.Add(new feelingWord { word = "aceptar", score = 3 });
            feelingWords.Add(new feelingWord { word = "aclamada", score = 2 });
            feelingWords.Add(new feelingWord { word = "aclara", score = 2 });
            feelingWords.Add(new feelingWord { word = "acogedor", score = 3 });
            feelingWords.Add(new feelingWord { word = "acomodado", score = 1 });
            feelingWords.Add(new feelingWord { word = "acomodar", score = 1 });
            feelingWords.Add(new feelingWord { word = "actitud", score = 3 });
            feelingWords.Add(new feelingWord { word = "activar", score = 3 });
            feelingWords.Add(new feelingWord { word = "activo", score = 3 });
            feelingWords.Add(new feelingWord { word = "actualidad", score = 2 });
            feelingWords.Add(new feelingWord { word = "actualizable", score = 3 });
            feelingWords.Add(new feelingWord { word = "actualización", score = 2 });
            feelingWords.Add(new feelingWord { word = "actualizado", score = 3 });
            feelingWords.Add(new feelingWord { word = "acuerdan", score = 2 });
            feelingWords.Add(new feelingWord { word = "adaptabilidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "adaptación", score = 2 });
            feelingWords.Add(new feelingWord { word = "adecuada", score = 2 });
            feelingWords.Add(new feelingWord { word = "adecuadamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "adecuado", score = 2 });
            feelingWords.Add(new feelingWord { word = "adelante", score = 1 });
            feelingWords.Add(new feelingWord { word = "admirablemente", score = 2 });
            feelingWords.Add(new feelingWord { word = "admiración", score = 3 });
            feelingWords.Add(new feelingWord { word = "adorable", score = 3 });
            feelingWords.Add(new feelingWord { word = "afable", score = 2 });
            feelingWords.Add(new feelingWord { word = "afecto", score = 2 });
            feelingWords.Add(new feelingWord { word = "afirmación", score = 1 });
            feelingWords.Add(new feelingWord { word = "afirmar", score = 2 });
            feelingWords.Add(new feelingWord { word = "afirmativa", score = 2 });
            feelingWords.Add(new feelingWord { word = "afortunado", score = 3 });
            feelingWords.Add(new feelingWord { word = "afortunadamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "agrada", score = 2 });
            feelingWords.Add(new feelingWord { word = "agradable", score = 3 });
            feelingWords.Add(new feelingWord { word = "agradecido", score = 2 });
            feelingWords.Add(new feelingWord { word = "agradecimiento", score = 3 });
            feelingWords.Add(new feelingWord { word = "ahorro", score = 2 });
            feelingWords.Add(new feelingWord { word = "ahorros", score = 3 });
            feelingWords.Add(new feelingWord { word = "alcanzable", score = 3 });
            feelingWords.Add(new feelingWord { word = "alcanzar", score = 2 });
            feelingWords.Add(new feelingWord { word = "alegre", score = 4 });
            feelingWords.Add(new feelingWord { word = "alegremente", score = 4 });
            feelingWords.Add(new feelingWord { word = "alegría", score = 4 });
            feelingWords.Add(new feelingWord { word = "alentar", score = 3 });
            feelingWords.Add(new feelingWord { word = "alimentos", score = 2 });
            feelingWords.Add(new feelingWord { word = "aliviado", score = 2 });
            feelingWords.Add(new feelingWord { word = "aliviar", score = 2 });
            feelingWords.Add(new feelingWord { word = "alivio", score = 2 });
            feelingWords.Add(new feelingWord { word = "amable", score = 3 });
            feelingWords.Add(new feelingWord { word = "amado", score = 3 });
            feelingWords.Add(new feelingWord { word = "amigo", score = 2 });
            feelingWords.Add(new feelingWord { word = "amistad", score = 2 });
            feelingWords.Add(new feelingWord { word = "amo", score = 3 });
            feelingWords.Add(new feelingWord { word = "amor", score = 3 });
            feelingWords.Add(new feelingWord { word = "amistoso", score = 2 });
            feelingWords.Add(new feelingWord { word = "amistosa", score = 2 });
            feelingWords.Add(new feelingWord { word = "animar", score = 2 });
            feelingWords.Add(new feelingWord { word = "ánimo", score = 2 });
            feelingWords.Add(new feelingWord { word = "apertura", score = 1 });
            feelingWords.Add(new feelingWord { word = "aprecio", score = 2 });
            feelingWords.Add(new feelingWord { word = "aprender", score = 2 });
            feelingWords.Add(new feelingWord { word = "apropiado", score = 1 });
            feelingWords.Add(new feelingWord { word = "aprendizaje", score = 2 });
            feelingWords.Add(new feelingWord { word = "armonía", score = 2 });
            feelingWords.Add(new feelingWord { word = "atención", score = 2 });
            feelingWords.Add(new feelingWord { word = "atractivo", score = 3 });
            feelingWords.Add(new feelingWord { word = "auténtico", score = 2 });
            feelingWords.Add(new feelingWord { word = "autonomía", score = 2 });
            feelingWords.Add(new feelingWord { word = "autoridad", score = 1 });
            feelingWords.Add(new feelingWord { word = "avance", score = 2 });
            feelingWords.Add(new feelingWord { word = "avanzada", score = 3 });
            feelingWords.Add(new feelingWord { word = "aventajado", score = 3 });
            feelingWords.Add(new feelingWord { word = "ayudado", score = 3 });
            feelingWords.Add(new feelingWord { word = "ayudando", score = 3 });
            feelingWords.Add(new feelingWord { word = "barato", score = 2 });
            feelingWords.Add(new feelingWord { word = "bastante", score = 3 });
            feelingWords.Add(new feelingWord { word = "bella", score = 3 });
            feelingWords.Add(new feelingWord { word = "belleza", score = 3 });
            feelingWords.Add(new feelingWord { word = "bendición", score = 3 });
            feelingWords.Add(new feelingWord { word = "bendita", score = 3 });
            feelingWords.Add(new feelingWord { word = "beneficio", score = 3 });
            feelingWords.Add(new feelingWord { word = "beneficios", score = 3 });
            feelingWords.Add(new feelingWord { word = "beneficioso", score = 3 });
            feelingWords.Add(new feelingWord { word = "beneficiosamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "beso", score = 4 });
            feelingWords.Add(new feelingWord { word = "bien", score = 3 });
            feelingWords.Add(new feelingWord { word = "bienestar", score = 3 });
            feelingWords.Add(new feelingWord { word = "bondad", score = 2 });
            feelingWords.Add(new feelingWord { word = "bonitamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "bono", score = 2 });
            feelingWords.Add(new feelingWord { word = "brilla", score = 2 });
            feelingWords.Add(new feelingWord { word = "brillante", score = 3 });
            feelingWords.Add(new feelingWord { word = "bueno", score = 1 });
            feelingWords.Add(new feelingWord { word = "caballerosidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "calidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "calificado", score = 2 });
            feelingWords.Add(new feelingWord { word = "calificar", score = 3 });
            feelingWords.Add(new feelingWord { word = "calma", score = 2 });
            feelingWords.Add(new feelingWord { word = "cambio", score = 2 });
            feelingWords.Add(new feelingWord { word = "capacidad", score = 2 });
            feelingWords.Add(new feelingWord { word = "capaz", score = 2 });
            feelingWords.Add(new feelingWord { word = "cariño", score = 3 });
            feelingWords.Add(new feelingWord { word = "cariñoso", score = 3 });
            feelingWords.Add(new feelingWord { word = "cautivar", score = 2 });
            feelingWords.Add(new feelingWord { word = "cautivó", score = 2 });
            feelingWords.Add(new feelingWord { word = "celebración", score = 1 });
            feelingWords.Add(new feelingWord { word = "celebrar", score = 2 });
            feelingWords.Add(new feelingWord { word = "certeza", score = 1 });
            feelingWords.Add(new feelingWord { word = "cercanía", score = 1 });
            feelingWords.Add(new feelingWord { word = "chistoso", score = 1 });
            feelingWords.Add(new feelingWord { word = "chistosa", score = 1 });
            feelingWords.Add(new feelingWord { word = "cierto", score = 1 });
            feelingWords.Add(new feelingWord { word = "citas", score = 1 });
            feelingWords.Add(new feelingWord { word = "clara", score = 1 });
            feelingWords.Add(new feelingWord { word = "claramente", score = 2 });
            feelingWords.Add(new feelingWord { word = "claridad", score = 2 });
            feelingWords.Add(new feelingWord { word = "claro", score = 1 });
            feelingWords.Add(new feelingWord { word = "coherencia", score = 2 });
            feelingWords.Add(new feelingWord { word = "comfortable", score = 2 });
            feelingWords.Add(new feelingWord { word = "cómodamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "cómodo", score = 2 });
            feelingWords.Add(new feelingWord { word = "compañerismo", score = 2 });
            feelingWords.Add(new feelingWord { word = "compartir", score = 1 });
            feelingWords.Add(new feelingWord { word = "compasión", score = 1 });
            feelingWords.Add(new feelingWord { word = "competencia", score = 1 });
            feelingWords.Add(new feelingWord { word = "complementado", score = 2 });
            feelingWords.Add(new feelingWord { word = "complemento", score = 2 });
            feelingWords.Add(new feelingWord { word = "comprensivo", score = 2 });
            feelingWords.Add(new feelingWord { word = "comprobado", score = 2 });
            feelingWords.Add(new feelingWord { word = "comprometido", score = 2 });
            feelingWords.Add(new feelingWord { word = "compromiso", score = 1 });
            feelingWords.Add(new feelingWord { word = "competitiva", score = 2 });
            feelingWords.Add(new feelingWord { word = "comunicación", score = 2 });
            feelingWords.Add(new feelingWord { word = "comunión", score = 1 });
            feelingWords.Add(new feelingWord { word = "concentración", score = 1 });
            feelingWords.Add(new feelingWord { word = "conciencia", score = 2 });
            feelingWords.Add(new feelingWord { word = "conectividad", score = 1 });
            feelingWords.Add(new feelingWord { word = "conexión", score = 1 });
            feelingWords.Add(new feelingWord { word = "confiabilidad", score = 2 });
            feelingWords.Add(new feelingWord { word = "confiable", score = 3 });
            feelingWords.Add(new feelingWord { word = "confianza", score = 2 });
            feelingWords.Add(new feelingWord { word = "confiar", score = 3 });
            feelingWords.Add(new feelingWord { word = "confort", score = 2 });
            feelingWords.Add(new feelingWord { word = "conocimiento", score = 3 });
            feelingWords.Add(new feelingWord { word = "consciente", score = 2 });
            feelingWords.Add(new feelingWord { word = "consideración", score = 2 });
            feelingWords.Add(new feelingWord { word = "consistencia", score = 2 });
            feelingWords.Add(new feelingWord { word = "consolidación", score = 1 });
            feelingWords.Add(new feelingWord { word = "constancia", score = 1 });
            feelingWords.Add(new feelingWord { word = "contento", score = 3 });
            feelingWords.Add(new feelingWord { word = "continuidad", score = 2 });
            feelingWords.Add(new feelingWord { word = "contribución", score = 2 });
            feelingWords.Add(new feelingWord { word = "conveniencia", score = 2 });
            feelingWords.Add(new feelingWord { word = "conveniente", score = 2 });
            feelingWords.Add(new feelingWord { word = "convincente", score = 1 });
            feelingWords.Add(new feelingWord { word = "convicción", score = 1 });
            feelingWords.Add(new feelingWord { word = "cooperación", score = 2 });
            feelingWords.Add(new feelingWord { word = "coraje", score = 2 });
            feelingWords.Add(new feelingWord { word = "correctamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "correcto", score = 3 });
            feelingWords.Add(new feelingWord { word = "cortés", score = 2 });
            feelingWords.Add(new feelingWord { word = "cortesía", score = 1 });
            feelingWords.Add(new feelingWord { word = "creativo", score = 1 });
            feelingWords.Add(new feelingWord { word = "crecer", score = 1 });
            feelingWords.Add(new feelingWord { word = "cumplido", score = 3 });
            feelingWords.Add(new feelingWord { word = "cumple", score = 2 });
            feelingWords.Add(new feelingWord { word = "curiosidad", score = 2 });
            feelingWords.Add(new feelingWord { word = "cumplir", score = 2 });
            feelingWords.Add(new feelingWord { word = "cumplimiento", score = 2 });
            feelingWords.Add(new feelingWord { word = "deber", score = 2 });
            feelingWords.Add(new feelingWord { word = "decente", score = 2 });
            feelingWords.Add(new feelingWord { word = "decisivo", score = 2 });
            feelingWords.Add(new feelingWord { word = "decencia", score = 2 });
            feelingWords.Add(new feelingWord { word = "delicado", score = 2 });
            feelingWords.Add(new feelingWord { word = "deliciosa", score = 2 });
            feelingWords.Add(new feelingWord { word = "delicioso", score = 2 });
            feelingWords.Add(new feelingWord { word = "deportivo", score = 2 });
            feelingWords.Add(new feelingWord { word = "derecho", score = 3 });
            feelingWords.Add(new feelingWord { word = "descubrimiento", score = 1 });
            feelingWords.Add(new feelingWord { word = "deseable", score = 1 });
            feelingWords.Add(new feelingWord { word = "deseo", score = 1 });
            feelingWords.Add(new feelingWord { word = "deseando", score = 2 });
            feelingWords.Add(new feelingWord { word = "despliegue", score = 1 });
            feelingWords.Add(new feelingWord { word = "despejado", score = 1 });
            feelingWords.Add(new feelingWord { word = "destreza", score = 1 });
            feelingWords.Add(new feelingWord { word = "destacar", score = 2 });
            feelingWords.Add(new feelingWord { word = "determinación", score = 2 });
            feelingWords.Add(new feelingWord { word = "devoción", score = 1 });
            feelingWords.Add(new feelingWord { word = "dichosamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "dichoso", score = 2 });
            feelingWords.Add(new feelingWord { word = "digna", score = 2 });
            feelingWords.Add(new feelingWord { word = "digno", score = 2 });
            feelingWords.Add(new feelingWord { word = "disciplina", score = 1 });
            feelingWords.Add(new feelingWord { word = "disfruta", score = 3 });
            feelingWords.Add(new feelingWord { word = "disfrutado", score = 2 });
            feelingWords.Add(new feelingWord { word = "disfrutar", score = 3 });
            feelingWords.Add(new feelingWord { word = "disponible", score = 2 });
            feelingWords.Add(new feelingWord { word = "divertida", score = 2 });
            feelingWords.Add(new feelingWord { word = "divertido", score = 2 });
            feelingWords.Add(new feelingWord { word = "divino", score = 1 });
            feelingWords.Add(new feelingWord { word = "dulce", score = 2 });
            feelingWords.Add(new feelingWord { word = "dulzura", score = 2 });
            feelingWords.Add(new feelingWord { word = "educación", score = 2 });
            feelingWords.Add(new feelingWord { word = "educada", score = 2 });
            feelingWords.Add(new feelingWord { word = "educado", score = 2 });
            feelingWords.Add(new feelingWord { word = "efectivamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "eficaz", score = 1 });
            feelingWords.Add(new feelingWord { word = "eficacia", score = 1 });
            feelingWords.Add(new feelingWord { word = "eficiente", score = 1 });
            feelingWords.Add(new feelingWord { word = "eficiencia", score = 1 });
            feelingWords.Add(new feelingWord { word = "ejemplar", score = 2 });
            feelingWords.Add(new feelingWord { word = "elegancia", score = 2 });
            feelingWords.Add(new feelingWord { word = "elogiar", score = 2 });
            feelingWords.Add(new feelingWord { word = "elogio", score = 2 });
            feelingWords.Add(new feelingWord { word = "embellecer", score = 2 });
            feelingWords.Add(new feelingWord { word = "emoción", score = 2 });
            feelingWords.Add(new feelingWord { word = "emocionada", score = 2 });
            feelingWords.Add(new feelingWord { word = "emocionado", score = 3 });
            feelingWords.Add(new feelingWord { word = "emocionante", score = 3 });
            feelingWords.Add(new feelingWord { word = "emociones", score = 2 });
            feelingWords.Add(new feelingWord { word = "emprendedor", score = 2 });
            feelingWords.Add(new feelingWord { word = "enamorado", score = 2 });
            feelingWords.Add(new feelingWord { word = "encantado", score = 2 });
            feelingWords.Add(new feelingWord { word = "encanto", score = 3 });
            feelingWords.Add(new feelingWord { word = "endulzar", score = 2 });
            feelingWords.Add(new feelingWord { word = "enhorabuena", score = 3 });
            feelingWords.Add(new feelingWord { word = "enorme", score = 3 });
            feelingWords.Add(new feelingWord { word = "enriquecimiento", score = 2 });
            feelingWords.Add(new feelingWord { word = "enseñar", score = 1 });
            feelingWords.Add(new feelingWord { word = "entender", score = 1 });
            feelingWords.Add(new feelingWord { word = "entendimiento", score = 1 });
            feelingWords.Add(new feelingWord { word = "entusiasmar", score = 2 });
            feelingWords.Add(new feelingWord { word = "entusiasmo", score = 2 });
            feelingWords.Add(new feelingWord { word = "equilibrado", score = 1 });
            feelingWords.Add(new feelingWord { word = "equidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "equipo", score = 1 });
            feelingWords.Add(new feelingWord { word = "equitativo", score = 1 });
            feelingWords.Add(new feelingWord { word = "esfuerzo", score = 2 });
            feelingWords.Add(new feelingWord { word = "espectacular", score = 3 });
            feelingWords.Add(new feelingWord { word = "esperanza", score = 2 });
            feelingWords.Add(new feelingWord { word = "especial", score = 1 });
            feelingWords.Add(new feelingWord { word = "esplendor", score = 1 });
            feelingWords.Add(new feelingWord { word = "estudioso", score = 1 });
            feelingWords.Add(new feelingWord { word = "estupendo", score = 3 });
            feelingWords.Add(new feelingWord { word = "ética", score = 2 });
            feelingWords.Add(new feelingWord { word = "euforia", score = 2 });
            feelingWords.Add(new feelingWord { word = "eufórico", score = 2 });
            feelingWords.Add(new feelingWord { word = "excelencia", score = 1 });
            feelingWords.Add(new feelingWord { word = "excelente", score = 2 });
            feelingWords.Add(new feelingWord { word = "excelentemente", score = 3 });
            feelingWords.Add(new feelingWord { word = "éxito", score = 2 });
            feelingWords.Add(new feelingWord { word = "éxitos", score = 2 });
            feelingWords.Add(new feelingWord { word = "exitosa", score = 2 });
            feelingWords.Add(new feelingWord { word = "experiencia", score = 2 });
            feelingWords.Add(new feelingWord { word = "exquisito", score = 1 });
            feelingWords.Add(new feelingWord { word = "exuberante", score = 1 });
            feelingWords.Add(new feelingWord { word = "fabuloso", score = 3 });
            feelingWords.Add(new feelingWord { word = "fácil", score = 1 });
            feelingWords.Add(new feelingWord { word = "facilidad", score = 2 });
            feelingWords.Add(new feelingWord { word = "facilitar", score = 1 });
            feelingWords.Add(new feelingWord { word = "facilita", score = 1 });
            feelingWords.Add(new feelingWord { word = "factible", score = 1 });
            feelingWords.Add(new feelingWord { word = "fantástico", score = 2 });
            feelingWords.Add(new feelingWord { word = "fascinado", score = 2 });
            feelingWords.Add(new feelingWord { word = "fascinante", score = 2 });
            feelingWords.Add(new feelingWord { word = "favor", score = 1 });
            feelingWords.Add(new feelingWord { word = "favorecida", score = 1 });
            feelingWords.Add(new feelingWord { word = "felicidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "felicidades", score = 3 });
            feelingWords.Add(new feelingWord { word = "felicita", score = 3 });
            feelingWords.Add(new feelingWord { word = "feliz", score = 3 });
            feelingWords.Add(new feelingWord { word = "fenomenal", score = 3 });
            feelingWords.Add(new feelingWord { word = "festividad", score = 2 });
            feelingWords.Add(new feelingWord { word = "festivo", score = 2 });
            feelingWords.Add(new feelingWord { word = "fiel", score = 2 });
            feelingWords.Add(new feelingWord { word = "fino", score = 1 });
            feelingWords.Add(new feelingWord { word = "firme", score = 1 });
            feelingWords.Add(new feelingWord { word = "formalmente", score = 1 });
            feelingWords.Add(new feelingWord { word = "fortaleza", score = 1 });
            feelingWords.Add(new feelingWord { word = "fraternal", score = 1 });
            feelingWords.Add(new feelingWord { word = "fresco", score = 1 });
            feelingWords.Add(new feelingWord { word = "fuerte", score = 2 });
            feelingWords.Add(new feelingWord { word = "fuerza", score = 2 });
            feelingWords.Add(new feelingWord { word = "futuro", score = 2 });
            feelingWords.Add(new feelingWord { word = "ganado", score = 2 });
            feelingWords.Add(new feelingWord { word = "ganador", score = 2 });
            feelingWords.Add(new feelingWord { word = "ganancias", score = 1 });
            feelingWords.Add(new feelingWord { word = "ganando", score = 1 });
            feelingWords.Add(new feelingWord { word = "ganar", score = 2 });
            feelingWords.Add(new feelingWord { word = "garantía", score = 1 });
            feelingWords.Add(new feelingWord { word = "generar", score = 1 });
            feelingWords.Add(new feelingWord { word = "generosamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "generoso", score = 2 });
            feelingWords.Add(new feelingWord { word = "generosidad", score = 2 });
            feelingWords.Add(new feelingWord { word = "genial", score = 3 });
            feelingWords.Add(new feelingWord { word = "glamour", score = 2 });
            feelingWords.Add(new feelingWord { word = "gloria", score = 2 });
            feelingWords.Add(new feelingWord { word = "gloriosa", score = 2 });
            feelingWords.Add(new feelingWord { word = "gloriosamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "gracias", score = 1 });
            feelingWords.Add(new feelingWord { word = "gran", score = 3 });
            feelingWords.Add(new feelingWord { word = "grande", score = 3 });
            feelingWords.Add(new feelingWord { word = "grandeza", score = 3 });
            feelingWords.Add(new feelingWord { word = "gratamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "gratificante", score = 3 });
            feelingWords.Add(new feelingWord { word = "gratis", score = 1 });
            feelingWords.Add(new feelingWord { word = "gusto", score = 1 });
            feelingWords.Add(new feelingWord { word = "gustos", score = 1 });
            feelingWords.Add(new feelingWord { word = "gratitud", score = 1 });
            feelingWords.Add(new feelingWord { word = "habilidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "habilitado", score = 1 });
            feelingWords.Add(new feelingWord { word = "hábil", score = 1 });
            feelingWords.Add(new feelingWord { word = "halagar", score = 2 });
            feelingWords.Add(new feelingWord { word = "hermosa", score = 3 });
            feelingWords.Add(new feelingWord { word = "hermosamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "hermoso", score = 3 });
            feelingWords.Add(new feelingWord { word = "héroe", score = 2 });
            feelingWords.Add(new feelingWord { word = "héroes", score = 2 });
            feelingWords.Add(new feelingWord { word = "heroísmo", score = 2 });
            feelingWords.Add(new feelingWord { word = "hola", score = 1 });
            feelingWords.Add(new feelingWord { word = "homenaje", score = 2 });
            feelingWords.Add(new feelingWord { word = "honestidad", score = 2 });
            feelingWords.Add(new feelingWord { word = "honesto", score = 2 });
            feelingWords.Add(new feelingWord { word = "honrado", score = 2 });
            feelingWords.Add(new feelingWord { word = "humildad", score = 2 });
            feelingWords.Add(new feelingWord { word = "humilde", score = 2 });
            feelingWords.Add(new feelingWord { word = "humor", score = 1 });
            feelingWords.Add(new feelingWord { word = "idolatrado", score = 2 });
            feelingWords.Add(new feelingWord { word = "idolatrar", score = 2 });
            feelingWords.Add(new feelingWord { word = "igualdad", score = 1 });
            feelingWords.Add(new feelingWord { word = "iluminado", score = 1 });
            feelingWords.Add(new feelingWord { word = "imparable", score = 2 });
            feelingWords.Add(new feelingWord { word = "imponente", score = 2 });
            feelingWords.Add(new feelingWord { word = "importante", score = 2 });
            feelingWords.Add(new feelingWord { word = "impresiona", score = 1 });
            feelingWords.Add(new feelingWord { word = "impresionante", score = 2 });
            feelingWords.Add(new feelingWord { word = "inafectado", score = 1 });
            feelingWords.Add(new feelingWord { word = "impresionantemente", score = 2 });
            feelingWords.Add(new feelingWord { word = "increíble", score = 2 });
            feelingWords.Add(new feelingWord { word = "increíblemente", score = 2 });
            feelingWords.Add(new feelingWord { word = "incuestionable", score = 3 });
            feelingWords.Add(new feelingWord { word = "indiscutible", score = 2 });
            feelingWords.Add(new feelingWord { word = "indudable", score = 1 });
            feelingWords.Add(new feelingWord { word = "infalible", score = 1 });
            feelingWords.Add(new feelingWord { word = "ingenio", score = 1 });
            feelingWords.Add(new feelingWord { word = "ingenioso", score = 2 });
            feelingWords.Add(new feelingWord { word = "ingenuo", score = 3 });
            feelingWords.Add(new feelingWord { word = "inigualable", score = 1 });
            feelingWords.Add(new feelingWord { word = "inmejorable", score = 1 });
            feelingWords.Add(new feelingWord { word = "innovador", score = 1 });
            feelingWords.Add(new feelingWord { word = "inolvidable", score = 3 });
            feelingWords.Add(new feelingWord { word = "inspiración", score = 2 });
            feelingWords.Add(new feelingWord { word = "inspirado", score = 2 });
            feelingWords.Add(new feelingWord { word = "inteligencia", score = 2 });
            feelingWords.Add(new feelingWord { word = "inteligente", score = 3 });
            feelingWords.Add(new feelingWord { word = "interés", score = 3 });
            feelingWords.Add(new feelingWord { word = "interesado", score = 2 });
            feelingWords.Add(new feelingWord { word = "interesante", score = 2 });
            feelingWords.Add(new feelingWord { word = "intereses", score = 2 });
            feelingWords.Add(new feelingWord { word = "invencible", score = 2 });
            feelingWords.Add(new feelingWord { word = "investigación", score = 1 });
            feelingWords.Add(new feelingWord { word = "invertir", score = 1 });
            feelingWords.Add(new feelingWord { word = "juguetona", score = 1 });
            feelingWords.Add(new feelingWord { word = "justicia", score = 1 });
            feelingWords.Add(new feelingWord { word = "justo", score = 1 });
            feelingWords.Add(new feelingWord { word = "juventud", score = 1 });
            feelingWords.Add(new feelingWord { word = "juicioso", score = 1 });
            feelingWords.Add(new feelingWord { word = "juvenil", score = 1 });
            feelingWords.Add(new feelingWord { word = "leal", score = 1 });
            feelingWords.Add(new feelingWord { word = "lealtad", score = 2 });
            feelingWords.Add(new feelingWord { word = "legal", score = 2 });
            feelingWords.Add(new feelingWord { word = "legible", score = 1 });
            feelingWords.Add(new feelingWord { word = "legítimo", score = 1 });
            feelingWords.Add(new feelingWord { word = "liberado", score = 1 });
            feelingWords.Add(new feelingWord { word = "liberar", score = 1 });
            feelingWords.Add(new feelingWord { word = "libertad", score = 1 });
            feelingWords.Add(new feelingWord { word = "líder", score = 2 });
            feelingWords.Add(new feelingWord { word = "liderazgo", score = 2 });
            feelingWords.Add(new feelingWord { word = "limpieza", score = 2 });
            feelingWords.Add(new feelingWord { word = "limpia", score = 2 });
            feelingWords.Add(new feelingWord { word = "lindo", score = 2 });
            feelingWords.Add(new feelingWord { word = "lograr", score = 2 });
            feelingWords.Add(new feelingWord { word = "logro", score = 2 });
            feelingWords.Add(new feelingWord { word = "logros", score = 2 });
            feelingWords.Add(new feelingWord { word = "lujoso", score = 2 });
            feelingWords.Add(new feelingWord { word = "luz", score = 1 });
            feelingWords.Add(new feelingWord { word = "maestro", score = 1 });
            feelingWords.Add(new feelingWord { word = "magistral", score = 2 });
            feelingWords.Add(new feelingWord { word = "mágico", score = 2 });
            feelingWords.Add(new feelingWord { word = "magnífica", score = 3 });
            feelingWords.Add(new feelingWord { word = "magníficamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "magnífico", score = 3 });
            feelingWords.Add(new feelingWord { word = "manejable", score = 1 });
            feelingWords.Add(new feelingWord { word = "maravillaron", score = 2 });
            feelingWords.Add(new feelingWord { word = "maravillas", score = 2 });
            feelingWords.Add(new feelingWord { word = "maravillosa", score = 3 });
            feelingWords.Add(new feelingWord { word = "maravilloso", score = 3 });
            feelingWords.Add(new feelingWord { word = "más", score = 3 });
            feelingWords.Add(new feelingWord { word = "mejor", score = 2 });
            feelingWords.Add(new feelingWord { word = "mejora", score = 2 });
            feelingWords.Add(new feelingWord { word = "mejorado", score = 2 });
            feelingWords.Add(new feelingWord { word = "mejorar", score = 2 });
            feelingWords.Add(new feelingWord { word = "milagro", score = 2 });
            feelingWords.Add(new feelingWord { word = "milagrosa", score = 2 });
            feelingWords.Add(new feelingWord { word = "milagros", score = 2 });
            feelingWords.Add(new feelingWord { word = "moderno", score = 2 });
            feelingWords.Add(new feelingWord { word = "modesto", score = 2 });
            feelingWords.Add(new feelingWord { word = "motivación", score = 2 });
            feelingWords.Add(new feelingWord { word = "motivado", score = 3 });
            feelingWords.Add(new feelingWord { word = "nacimiento", score = 1 });
            feelingWords.Add(new feelingWord { word = "noble", score = 2 });
            feelingWords.Add(new feelingWord { word = "notable", score = 1 });
            feelingWords.Add(new feelingWord { word = "notablemente", score = 1 });
            feelingWords.Add(new feelingWord { word = "novedad", score = 1 });
            feelingWords.Add(new feelingWord { word = "nuevo", score = 1 });
            feelingWords.Add(new feelingWord { word = "nutritiva", score = 1 });
            feelingWords.Add(new feelingWord { word = "navegable", score = 1 });
            feelingWords.Add(new feelingWord { word = "navegar", score = 1 });
            feelingWords.Add(new feelingWord { word = "obediente", score = 1 });
            feelingWords.Add(new feelingWord { word = "oportunidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "omnipotencia", score = 1 });
            feelingWords.Add(new feelingWord { word = "oportuno", score = 1 });
            feelingWords.Add(new feelingWord { word = "optimismo", score = 1 });
            feelingWords.Add(new feelingWord { word = "óptima", score = 2 });
            feelingWords.Add(new feelingWord { word = "optimista", score = 1 });
            feelingWords.Add(new feelingWord { word = "optimizar", score = 1 });
            feelingWords.Add(new feelingWord { word = "orden", score = 1 });
            feelingWords.Add(new feelingWord { word = "ordenado", score = 1 });
            feelingWords.Add(new feelingWord { word = "organización", score = 1 });
            feelingWords.Add(new feelingWord { word = "orgullo", score = 2 });
            feelingWords.Add(new feelingWord { word = "orgulloso", score = 3 });
            feelingWords.Add(new feelingWord { word = "originalidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "paciente", score = 1 });
            feelingWords.Add(new feelingWord { word = "pacífica", score = 1 });
            feelingWords.Add(new feelingWord { word = "pacífico", score = 1 });
            feelingWords.Add(new feelingWord { word = "participación", score = 1 });
            feelingWords.Add(new feelingWord { word = "paz", score = 1 });
            feelingWords.Add(new feelingWord { word = "positivo", score = 2 });
            feelingWords.Add(new feelingWord { word = "pensamiento", score = 1 });
            feelingWords.Add(new feelingWord { word = "perdón", score = 1 });
            feelingWords.Add(new feelingWord { word = "perdonar", score = 1 });
            feelingWords.Add(new feelingWord { word = "perfecto", score = 2 });
            feelingWords.Add(new feelingWord { word = "permitir", score = 1 });
            feelingWords.Add(new feelingWord { word = "perseverancia", score = 1 });
            feelingWords.Add(new feelingWord { word = "pertenencia", score = 1 });
            feelingWords.Add(new feelingWord { word = "pertenecer", score = 1 });
            feelingWords.Add(new feelingWord { word = "placer", score = 1 });
            feelingWords.Add(new feelingWord { word = "poder", score = 1 });
            feelingWords.Add(new feelingWord { word = "poderoso", score = 1 });
            feelingWords.Add(new feelingWord { word = "placeres", score = 1 });
            feelingWords.Add(new feelingWord { word = "positivamente", score = 1 });
            feelingWords.Add(new feelingWord { word = "positivo", score = 1 });
            feelingWords.Add(new feelingWord { word = "precioso", score = 3 });
            feelingWords.Add(new feelingWord { word = "precisamente", score = 1 });
            feelingWords.Add(new feelingWord { word = "preferible", score = 1 });
            feelingWords.Add(new feelingWord { word = "prefiere", score = 2 });
            feelingWords.Add(new feelingWord { word = "preferido", score = 2 });
            feelingWords.Add(new feelingWord { word = "premio", score = 2 });
            feelingWords.Add(new feelingWord { word = "premios", score = 2 });
            feelingWords.Add(new feelingWord { word = "prestigio", score = 2 });
            feelingWords.Add(new feelingWord { word = "privilegio", score = 2 });
            feelingWords.Add(new feelingWord { word = "productiva", score = 1 });
            feelingWords.Add(new feelingWord { word = "productivo", score = 1 });
            feelingWords.Add(new feelingWord { word = "progreso", score = 2 });
            feelingWords.Add(new feelingWord { word = "promesa", score = 1 });
            feelingWords.Add(new feelingWord { word = "prometedora", score = 2 });
            feelingWords.Add(new feelingWord { word = "próspera", score = 1 });
            feelingWords.Add(new feelingWord { word = "prosperidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "próspero", score = 1 });
            feelingWords.Add(new feelingWord { word = "prudente", score = 1 });
            feelingWords.Add(new feelingWord { word = "puntual", score = 1 });
            feelingWords.Add(new feelingWord { word = "puntualidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "puntualmente", score = 1 });
            feelingWords.Add(new feelingWord { word = "querido", score = 2 });
            feelingWords.Add(new feelingWord { word = "rápidamente", score = 1 });
            feelingWords.Add(new feelingWord { word = "rapidez", score = 1 });
            feelingWords.Add(new feelingWord { word = "rápido", score = 1 });
            feelingWords.Add(new feelingWord { word = "razón", score = 2 });
            feelingWords.Add(new feelingWord { word = "razonable", score = 2 });
            feelingWords.Add(new feelingWord { word = "radiante", score = 2 });
            feelingWords.Add(new feelingWord { word = "realidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "realista", score = 1 });
            feelingWords.Add(new feelingWord { word = "recomendable", score = 2 });
            feelingWords.Add(new feelingWord { word = "recomendado", score = 2 });
            feelingWords.Add(new feelingWord { word = "recomendar", score = 2 });
            feelingWords.Add(new feelingWord { word = "recompensa", score = 2 });
            feelingWords.Add(new feelingWord { word = "reconciliación", score = 1 });
            feelingWords.Add(new feelingWord { word = "reconfortante", score = 3 });
            feelingWords.Add(new feelingWord { word = "reconocido", score = 1 });
            feelingWords.Add(new feelingWord { word = "recomendación", score = 2 });
            feelingWords.Add(new feelingWord { word = "reconocimiento", score = 1 });
            feelingWords.Add(new feelingWord { word = "récord", score = 1 });
            feelingWords.Add(new feelingWord { word = "reconocimientos", score = 1 });
            feelingWords.Add(new feelingWord { word = "recuperar", score = 1 });
            feelingWords.Add(new feelingWord { word = "reembolso", score = 1 });
            feelingWords.Add(new feelingWord { word = "reestructurado", score = 1 });
            feelingWords.Add(new feelingWord { word = "reestructuración", score = 1 });
            feelingWords.Add(new feelingWord { word = "refugio", score = 1 });
            feelingWords.Add(new feelingWord { word = "regalo", score = 1 });
            feelingWords.Add(new feelingWord { word = "rejuvenecido", score = 1 });
            feelingWords.Add(new feelingWord { word = "relación", score = 1 });
            feelingWords.Add(new feelingWord { word = "relajado", score = 2 });
            feelingWords.Add(new feelingWord { word = "relájese", score = 2 });
            feelingWords.Add(new feelingWord { word = "relajante", score = 2 });
            feelingWords.Add(new feelingWord { word = "reluciente", score = 2 });
            feelingWords.Add(new feelingWord { word = "remunerado", score = 2 });
            feelingWords.Add(new feelingWord { word = "remunerar", score = 2 });
            feelingWords.Add(new feelingWord { word = "renovar", score = 1 });
            feelingWords.Add(new feelingWord { word = "reputación", score = 2 });
            feelingWords.Add(new feelingWord { word = "respeto", score = 2 });
            feelingWords.Add(new feelingWord { word = "respetable", score = 2 });
            feelingWords.Add(new feelingWord { word = "respetuoso", score = 2 });
            feelingWords.Add(new feelingWord { word = "respetuosamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "respiro", score = 1 });
            feelingWords.Add(new feelingWord { word = "responsabilidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "revolucionará", score = 1 });
            feelingWords.Add(new feelingWord { word = "rindas", score = -1 });
            feelingWords.Add(new feelingWord { word = "risa", score = 2 });
            feelingWords.Add(new feelingWord { word = "romántico", score = 3 });
            feelingWords.Add(new feelingWord { word = "riqueza", score = 1 });
            feelingWords.Add(new feelingWord { word = "saber", score = 2 });
            feelingWords.Add(new feelingWord { word = "sabroso", score = 2 });
            feelingWords.Add(new feelingWord { word = "saborear", score = 2 });
            feelingWords.Add(new feelingWord { word = "salud", score = 2 });
            feelingWords.Add(new feelingWord { word = "sano", score = 2 });
            feelingWords.Add(new feelingWord { word = "satisface", score = 2 });
            feelingWords.Add(new feelingWord { word = "satisfacer", score = 2 });
            feelingWords.Add(new feelingWord { word = "satisfactoria", score = 2 });
            feelingWords.Add(new feelingWord { word = "satisfecho", score = 2 });
            feelingWords.Add(new feelingWord { word = "segura", score = 2 });
            feelingWords.Add(new feelingWord { word = "seguridad", score = 2 });
            feelingWords.Add(new feelingWord { word = "seguro", score = 1 });
            feelingWords.Add(new feelingWord { word = "sensacional", score = 2 });
            feelingWords.Add(new feelingWord { word = "satisfactoriamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "servir", score = 2 });
            feelingWords.Add(new feelingWord { word = "sincero", score = 2 });
            feelingWords.Add(new feelingWord { word = "sinceramente", score = 2 });
            feelingWords.Add(new feelingWord { word = "sobresalientemente", score = 2 });
            feelingWords.Add(new feelingWord { word = "sobreviviente", score = 1 });
            feelingWords.Add(new feelingWord { word = "solidaridad", score = 2 });
            feelingWords.Add(new feelingWord { word = "solidez", score = 1 });
            feelingWords.Add(new feelingWord { word = "soñador", score = 2 });
            feelingWords.Add(new feelingWord { word = "sonriendo", score = 2 });
            feelingWords.Add(new feelingWord { word = "sonriente", score = 2 });
            feelingWords.Add(new feelingWord { word = "sonrisa", score = 2 });
            feelingWords.Add(new feelingWord { word = "sonrisas", score = 2 });
            feelingWords.Add(new feelingWord { word = "sorprendente", score = 2 });
            feelingWords.Add(new feelingWord { word = "sueña", score = 2 });
            feelingWords.Add(new feelingWord { word = "suerte", score = 1 });
            feelingWords.Add(new feelingWord { word = "suficiente", score = 2 });
            feelingWords.Add(new feelingWord { word = "suficientemente", score = 2 });
            feelingWords.Add(new feelingWord { word = "superado", score = 2 });
            feelingWords.Add(new feelingWord { word = "superadas", score = 2 });
            feelingWords.Add(new feelingWord { word = "superar", score = 2 });
            feelingWords.Add(new feelingWord { word = "superó", score = 2 });
            feelingWords.Add(new feelingWord { word = "superando", score = 2 });
            feelingWords.Add(new feelingWord { word = "terapia", score = 1 });
            feelingWords.Add(new feelingWord { word = "tesoro", score = 2 });
            feelingWords.Add(new feelingWord { word = "tolerancia", score = 1 });
            feelingWords.Add(new feelingWord { word = "trabajador", score = 2 });
            feelingWords.Add(new feelingWord { word = "trabajo", score = 2 });
            feelingWords.Add(new feelingWord { word = "tradición", score = 1 });
            feelingWords.Add(new feelingWord { word = "tranquila", score = 1 });
            feelingWords.Add(new feelingWord { word = "tranquilidad", score = 1 });
            feelingWords.Add(new feelingWord { word = "transparente", score = 1 });
            feelingWords.Add(new feelingWord { word = "trascendental", score = 1 });
            feelingWords.Add(new feelingWord { word = "tremendamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "triunfal", score = 2 });
            feelingWords.Add(new feelingWord { word = "triunfante", score = 2 });
            feelingWords.Add(new feelingWord { word = "triunfo", score = 2 });
            feelingWords.Add(new feelingWord { word = "trofeo", score = 2 });
            feelingWords.Add(new feelingWord { word = "útil", score = 1 });
            feelingWords.Add(new feelingWord { word = "útiles", score = 1 });
            feelingWords.Add(new feelingWord { word = "valentía", score = 1 });
            feelingWords.Add(new feelingWord { word = "válido", score = 1 });
            feelingWords.Add(new feelingWord { word = "valiente", score = 2 });
            feelingWords.Add(new feelingWord { word = "valientemente", score = 2 });
            feelingWords.Add(new feelingWord { word = "valioso", score = 2 });
            feelingWords.Add(new feelingWord { word = "valor", score = 2 });
            feelingWords.Add(new feelingWord { word = "valores", score = 1 });
            feelingWords.Add(new feelingWord { word = "venerar", score = 1 });
            feelingWords.Add(new feelingWord { word = "ventaja", score = 2 });
            feelingWords.Add(new feelingWord { word = "ventajas", score = 2 });
            feelingWords.Add(new feelingWord { word = "ventajoso", score = 3 });
            feelingWords.Add(new feelingWord { word = "verdad", score = 2 });
            feelingWords.Add(new feelingWord { word = "veraz", score = 1 });
            feelingWords.Add(new feelingWord { word = "victoria", score = 2 });
            feelingWords.Add(new feelingWord { word = "victorioso", score = 2 });
            feelingWords.Add(new feelingWord { word = "vivo", score = 1 });
            feelingWords.Add(new feelingWord { word = "voluntad", score = 1 });
            feelingWords.Add(new feelingWord { word = "voluntariamente", score = 1 });
            feelingWords.Add(new feelingWord { word = "vivir", score = 1 });
            //// Add negative words to feeling words list
            feelingWords.Add(new feelingWord { word = "malo", score = -2 });
            feelingWords.Add(new feelingWord { word = "horrible", score = -3 });
            feelingWords.Add(new feelingWord { word = "no", score = -2 });
            feelingWords.Add(new feelingWord { word = "triste", score = -3 });
            feelingWords.Add(new feelingWord { word = "enojado", score = -3 });
            feelingWords.Add(new feelingWord { word = "hambre", score = -1 });
            feelingWords.Add(new feelingWord { word = "perdió", score = -2 });
            feelingWords.Add(new feelingWord { word = "menos", score = -2 });
            feelingWords.Add(new feelingWord { word = "ninguna", score = -3 });
            feelingWords.Add(new feelingWord { word = "nadie", score = -2 });
            feelingWords.Add(new feelingWord { word = "nada", score = -1 });
            feelingWords.Add(new feelingWord { word = "ninguno", score = -1 });
            feelingWords.Add(new feelingWord { word = "ningún", score = -2 });
            feelingWords.Add(new feelingWord { word = "nunca", score = -2 });
            feelingWords.Add(new feelingWord { word = "jamás", score = -2 });
            feelingWords.Add(new feelingWord { word = "tampoco", score = -2 });
            feelingWords.Add(new feelingWord { word = "ni", score = -1 });

            using (var db = new AnalizadorBDEntities())
            {
                foreach (var feelingWord in feelingWords)
                {
                    db.feelingWords.Add(feelingWord);
                    db.SaveChanges();
                }
            }
        }

        protected void insertIntoEnhancers()
        {
            enhancers.Add(new enhancer { word = "extremadamente", score = 2 });
            enhancers.Add(new enhancer { word = "tan", score = 2 });
            enhancers.Add(new enhancer { word = "demasiado", score = 2 });
            enhancers.Add(new enhancer { word = "muy", score = 2 });
            enhancers.Add(new enhancer { word = "bastante", score = 2 });
            enhancers.Add(new enhancer { word = "aumenta", score = 1 });
            enhancers.Add(new enhancer { word = "más", score = 1 });
            enhancers.Add(new enhancer { word = "todo", score = 1 });
            enhancers.Add(new enhancer { word = "mucho", score = 2 });
            enhancers.Add(new enhancer { word = "suficientemente", score = 1 });
            enhancers.Add(new enhancer { word = "algo", score = 1 });
            enhancers.Add(new enhancer { word = "mejor", score = 2 });
            enhancers.Add(new enhancer { word = "mejores", score = 3 });
            enhancers.Add(new enhancer { word = "mucho", score = 2 });
            enhancers.Add(new enhancer { word = "gran", score = 2 });
            enhancers.Add(new enhancer { word = "menos", score = -1 });
            enhancers.Add(new enhancer { word = "ligeramente", score = -1 });
            enhancers.Add(new enhancer { word = "poco", score = -1 });
            enhancers.Add(new enhancer { word = "disminuye", score = -1 });
            enhancers.Add(new enhancer { word = "nunca", score = -1 });
            enhancers.Add(new enhancer { word = "tampoco", score = -1 });
            enhancers.Add(new enhancer { word = "contra", score = -1 });
            enhancers.Add(new enhancer { word = "poco", score = -1 });
            enhancers.Add(new enhancer { word = "menos", score = -2 });
            enhancers.Add(new enhancer { word = "casi", score = -1 });
            enhancers.Add(new enhancer { word = "peor", score = -3 });
            enhancers.Add(new enhancer { word = "pores", score = -3 });
            enhancers.Add(new enhancer { word = "mal", score = -2 });
            enhancers.Add(new enhancer { word = "malo", score = -3 });

            using (var db = new AnalizadorBDEntities())
            {
                foreach (var enhancer in enhancers)
                {
                    db.enhancers.Add(enhancer);
                    db.SaveChanges();
                }
            }
        }

        protected bool existInStopWords(string word)
        {
            foreach (stopword sw in stopWords)
            {
                if (sw.word.Equals(word))
                {
                    return true;
                }
            }

            return false;
        }

        protected List<string> removeStopWords()
        {
            string[] words = contentBox.Text.ToLower().Replace(",", "").Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Split(' ');
            List<string> result = new List<string>();

            foreach (string word in words)
            {
                if (!existInStopWords(word))
                {
                    result.Add(word);
                }
            }

            return result;
        }

        protected feelingWord existFeelingWord(string word)
        {
            foreach (feelingWord fw in feelingWords)
            {
                if (fw.word.Equals(word))
                {
                    return fw;
                }
            }

            return null;
        }

        protected emoji existEmoji(string emoji)
        {
            foreach (emoji e in emojis)
            {
                if (e.emoticon.Equals(emoji))
                {
                    return e;
                }
            }

            return null;
        }

        protected feelingWord getNewScore(enhancer e, feelingWord fw)
        {
            if (e.score > 0) // El potenciador es positivo
            {
                if (fw.score > 0) // El feeling word es positivo
                {
                    fw.score += e.score;
                }
                else // El feeling word es negativo
                {
                    fw.score = e.score;
                }
            }
            else // El potenciador es negativo
            {
                if (fw.score > 0) // El feeling word es positivo
                {
                    fw.score = e.score;
                }
                else // El feeling word es negativo
                {
                    fw.score = (e.score * -1) + (fw.score * -1);
                }
            }

            return fw;
        }

        protected feelingWord validateWithEnhancers(string wordBefore, feelingWord feelingWord)
        {
            foreach (enhancer e in enhancers)
            {
                if (e.word.Equals(wordBefore))
                {
                    return getNewScore(e, feelingWord);
                }
            }

            return feelingWord;
        }

        protected void sentimentAnalysis(List<string> content)
        {
            string pieceBefore = "";
            feelingWord fw;
            emoji e;

            foreach (string piece in content)
            {
                fw = existFeelingWord(piece);
                if (fw != null)
                {
                    // Only the first time
                    if (pieceBefore.Equals(""))
                    {
                        feelingWordsList.Add(fw);
                    }
                    else
                    {
                        feelingWordsList.Add(validateWithEnhancers(pieceBefore, fw));
                    }
                }

                e = existEmoji(piece);
                if (e != null)
                {
                    emojisList.Add(e);
                }

                pieceBefore = piece;
            }
        }

        protected int makeMeasurement()
        {
            int measurement = 0;

            foreach (feelingWord fw in feelingWordsList)
            {
                measurement += (int)fw.score;
            }

            foreach (emoji e in emojisList)
            {
                measurement += (int)e.score;
            }

            return measurement;
        }

        protected void showScores()
        {
            string result = "---> Palabras y emojis encontrados";
            
            foreach (feelingWord fw in feelingWordsList)
            {
                result += "\n" + fw.word + " -> " + fw.score;
            }

            foreach (emoji e in emojisList)
            {
                result += "\n" + e.emoticon + " -> " + e.score;
            }

            resultBox.Text = resultBox.Text + result;
        }

        protected void giveResult(int result)
        {
            string res = "\n\n---> Medicion";

            if (result > 0)
            {
                res += "\nEl texto es positivo.";
            }
            else if (result < 0)
            {
                res += "\nEl texto es negativo.";
            }
            else
            {
                res += "\nEl texto es neutro.";
            }

            resultBox.Text = resultBox.Text + res;
        }

        protected void giveProbabilities()
        {
            string[] words = contentBox.Text.ToLower().Replace(",", "").Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Split(' ');
            float totalWords = words.Count(),
                  totalPositives = 0,
                  totalNegatives = 0,
                  scorePositive = 0,
                  scoreNegative = 0;
            string result = "";

            foreach (feelingWord fw in feelingWordsList)
            {
                if (fw.score > 0)
                {
                    totalPositives++;
                    scorePositive += (float)fw.score;
                }
                else
                {
                    totalNegatives++;
                    scoreNegative += (float)fw.score * -1;
                }
            }

            foreach (emoji e in emojisList)
            {
                if (e.score > 0)
                {
                    totalPositives++;
                    scorePositive += (float)e.score;
                }
                else
                {
                    totalNegatives++;
                    scoreNegative += (float)e.score * -1;
                }
            }

            result += "\n\n---> Estadísticas";
            result += "\nTotal de palabras: " + totalWords.ToString();
            result += "\nTotal de palabras y emojis positivos: " + totalPositives + " -> " + (totalPositives / totalWords) * 100 + "%";
            result += "\nTotal de palabras y emojis negativos: " + totalNegatives + " -> " + (totalNegatives / totalWords) * 100 + "%";
            result += "\nTotal de palabras desconocidas: " + (totalWords - (totalPositives + totalNegatives)) + " -> " + ((totalWords - (totalPositives + totalNegatives)) / totalWords) * 100 + "%";
            result += "\nEl texto es positivo en un: " + (totalPositives / (totalPositives + totalNegatives)) * 100 + "%";
            result += "\nEl texto es negativo en un: " + (totalNegatives / (totalPositives + totalNegatives)) * 100 + "%";
            resultBox.Text = resultBox.Text + result;
        }

        protected void buttonAnalizar_Click(object sender, EventArgs e)
        {
            resultBox.Text = "";
            insertIntoStopWords();
            insertIntoFeelingWords();
            insertIntoEmojis();
            insertIntoEnhancers();

            List<string> result = removeStopWords();

            // Texto sin stopwords
            string res = "---> Texto sin stopwords\n";
            //Console.WriteLine("\n---> Texto sin stopwords");
            foreach (string word in result)
            {
                //Console.Write(word + " ");
                res += word + " ";
            }
            resultBox.Text = resultBox.Text + res + "\n\n";

            sentimentAnalysis(result);
            showScores();
            giveResult(makeMeasurement());
            giveProbabilities();
        }

    }
}