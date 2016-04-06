using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.IO.Compression;
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
using AnalisisEstadistico.Modulos;
using ICSharpCode.SharpZipLib.BZip2;

namespace AnalisisEstadistico
{
    public partial class index : System.Web.UI.Page
    {
        public Sentiment sentiment;
        public Language language;
        public Twitter twitter;
        public FB facebook;

        protected void Page_Load(object sender, EventArgs e)
        { }

        /// <summary>
        /// Limpia las carpetas utilizadas para almacenar los archivos a analizar.
        /// </summary>
        /// <param name="ruta">Ruta a limpiar.</param>
        protected void cleanFolder(string ruta)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(ruta);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete(); 
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true); 
            }
        }

        /// <summary>
        /// Limpia los textarea y oculta los graficos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cleanTextArea(object sender, EventArgs e) 
        {
            contentBox.Text = "";
            resultBox.Text = "";
            textLinkFolder.Text = "";
            textFacebook.Text = "";
            textTwitter.Text = "";
            textChart.Visible = false;
            langChart.Visible = false;
            sentimentPercentChart.Visible = false;
            sentimentScoresChart.Visible = false;
            tweetChart.Visible = false;
            tweetCChart.Visible = false;
        }

        /// <summary>
        /// Método utilizado para descomprimir los archivos de un zip file.
        /// </summary>
        /// <param name="ArchivoZip">Ruta donde se encuentra el archivo ZIP.</param>
        /// <param name="RutaGuardar">Ruta donde se guardaran los archivos extraídos del ZIP.</param>
        /// <returns>Booleano si pudo descomprimir o no.</returns>
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

        /// <summary>
        /// Guarda un archivo.
        /// </summary>
        /// <param name="ruta">Ruta en la que se va a guardar el archivo.</param>
        protected void saveFile(string ruta)
        {
            fileReader.SaveAs(ruta);
        }

        /// <summary>
        /// Extrae el contenido de un archivo bz2 y lo almacena en un carpeta.
        /// </summary>
        /// <param name="ruta">Ruta donde se encuentra el archivo bz2.</param>
        /// <param name="jsonID">ID para el nombre del json resultante conel contenido de descomprimir.</param>
        protected void extractBZ2(string ruta, int jsonID)
        {
            FileInfo fileToBeZipped = new FileInfo(ruta);

            using (FileStream fileToDecompressAsStream = fileToBeZipped.OpenRead())
            {
                // Ruta destino.
                string decompressedFileName = Server.MapPath("~") + "twitterJSON\\" + jsonID + ".json";
                using (FileStream decompressedStream = File.Create(decompressedFileName))
                {
                    try
                    {
                        BZip2.Decompress(fileToDecompressAsStream, decompressedStream, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene la ruta donde se encuentra el archivo a descomprimir, obtiene cada uno de los archivos de dicha ruta y a cada uno de ellos los manda a descomprimir.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void click_unpackageBZ2(object sender, EventArgs e)
        {
            string ruta = textLinkFolder.Text;

            if (!ruta.Equals(""))
            {
                cleanFolder(Server.MapPath("~") + "twitterJSON\\");
                try
                {
                    DirectoryInfo Dir = new DirectoryInfo(ruta);
                    FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.AllDirectories);
                    int i = 1;
                    foreach (FileInfo file in FileList)
                    {
                        //resultBox.Text = resultBox.Text + file.FullName + "\n";
                        if (file.Extension == ".bz2")
                        {
                            extractBZ2(file.FullName, i++);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Lee el contenido de archivos (html, json, xml, txt).
        /// </summary>
        /// <param name="ruta">Ruta donde se encuentra el archivo.</param>
        /// <returns>El contenido leido.</returns>
        protected string readHtmlJsonXmlTxt(string ruta)
        {
            string content = "";
            content = File.ReadAllText(ruta, Encoding.UTF8);

            content = Regex.Replace(content, "<.*?>", string.Empty);
            return content.ToLower().Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Replace("&", "");
        }

        /// <summary>
        /// Captura el archivo cargado, lo manda a guardar y luego a leer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void click_readHtmlJsonXmlTxt(object sender, EventArgs e) 
        {
            string ruta = Server.MapPath("~") + "files\\" + fileReader.FileName;
            saveFile(ruta);
            contentBox.Text = readHtmlJsonXmlTxt(ruta);
        }

        /// <summary>
        /// Lee el contenido de un archivo (doc, docx).
        /// </summary>
        /// <param name="ruta">Ruta donde se encuentra el archivo.</param>
        /// <returns>El contenido leido.</returns>
        protected string readDoc(string ruta)
        {
            string content = "";

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = ruta;
            object readOnly = true;
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                content += " \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString();
            }
            docs.Close();
            word.Quit();

            content = Regex.Replace(content, "<.*?>", string.Empty);
            return content.ToLower().Replace(",", "").Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Replace("&", "");
        }

        /// <summary>
        /// Captura el archivo cargado, lo manda a guardar y luego a leer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void click_readDoc(object sender, EventArgs e)
        {
            string ruta = Server.MapPath("~") + "files\\" + fileReader.FileName;
            saveFile(ruta);
            contentBox.Text = readDoc(ruta);
        }

        /// <summary>
        /// Captura el link a leer y lo lee.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void click_readLink(object sender, EventArgs e)
        {
            string link = textLinkFolder.Text,
                   content;
            WebClient client = new WebClient();
            byte[] byteData = null;
            byteData = client.DownloadData(link);

            UTF8Encoding UTF8Encod = new UTF8Encoding();

            content = Regex.Replace(UTF8Encod.GetString(byteData), "<(.|\\n)*?>", string.Empty);
            content = Regex.Replace(content, "<.*?>", string.Empty);
            contentBox.Text = content.ToLower().Replace(",", "").Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Replace("&", "");
        }

        /// <summary>
        /// Lee todos los archivos en una carpeta.
        /// </summary>
        /// <param name="ruta">Ruta a leer.</param>
        protected void readAllInFolder(string ruta)
        {
            try
            {
                DirectoryInfo Dir = new DirectoryInfo(ruta);
                FileInfo[] FileList = Dir.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo file in FileList)
                {
                    //resultBox.Text = resultBox.Text + file.FullName + "\n";

                    if (file.Extension == ".txt" || file.Extension == ".html" || file.Extension == ".json" || file.Extension == ".xml")
                    {
                        contentBox.Text = contentBox.Text + readHtmlJsonXmlTxt(file.FullName) + "\n\n";
                    }
                    else if (file.Extension == ".doc" || file.Extension == ".docx")
                    {
                        contentBox.Text = contentBox.Text + readDoc(file.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Captura la ruta de la carpeta a leer y la manda a ser leida.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void click_readFolder(object sender, EventArgs e)
        {
            readAllInFolder(textLinkFolder.Text);
        }

        /// <summary>
        /// Captura el archivo zip cargado, limpia la carpeta destino, lo manda a descomprimir y luego lee los archivos que descomprimió.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void click_unpackageZIP(object sender, EventArgs e)
        {
            // Almacenar el .zip en la carpeta zips del proyecto
            string ruta = Server.MapPath("~") + "zips\\" + fileReader.FileName;
            string destino = Server.MapPath("~") + "zipsContent\\";
            cleanFolder(Server.MapPath("~") + "zips\\");
            cleanFolder(destino);
            saveFile(ruta);

            // Si la descompresion fue exitosa.
            if (extract(ruta, destino))
            {
                // Lee todos los archivos descomprimidos.
                readAllInFolder(destino);
            }
            else
            {
                contentBox.Text = "Fallo al descomprimir";
            }
        }

        /// <summary>
        /// Captura el texto a analizar y lo manda para que se le realice el analisis del sentimiento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sentimentAnalysis(object sender, EventArgs e)
        {
            string text = contentBox.Text;

            if (!text.Equals(""))
            {
                this.sentiment = new Sentiment(text);
                resultBox.Text = this.sentiment.sentimentAnalysis();
                resultBox.Text = resultBox.Text + this.sentiment.showScores();
                resultBox.Text = resultBox.Text + this.sentiment.giveProbabilities();
                generateSentimentCharts(this.sentiment.porcentajes, this.sentiment.scores);
            }
        }

        /// <summary>
        /// Captura el texto a analizar y lo manda para que se le realice el analisis de lenguaje.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void languageAnalysis(object sender, EventArgs e) 
        {
            string content = contentBox.Text;

            if (!content.Equals(""))
            {
                this.language = new Language();
                this.language.text = content;
                resultBox.Text = this.language.languageAnalisys();
                generateLanguageCharts();
            }
        }

        /// <summary>
        /// Captura el texto/ruta a analizar y lo manda para que se analicen los tweets.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tweetsAnalysis(object sender, EventArgs e) 
        {
            // Tweets masivos
            if (!textLinkFolder.Text.Equals("")) 
            {
                this.twitter = new Twitter(Server.MapPath("~") + "twitterJSON\\");
                this.twitter.tweetsAnalysis();
                this.twitter.generalAnalysis();
                generateCharts(this.twitter.langPercents, this.twitter.langCount);
                resultBox.Text = "Total de tweets: " + this.twitter.tweetList.Count() + "\n";
                resultBox.Text = resultBox.Text + "Cantidad de usuarios diferentes: " + this.twitter.differentUsers.Count().ToString();
            }
            // Tweets de un usuario.
            else if (!textTwitter.Text.Equals("")) 
            {
                this.twitter = new Twitter();
                contentBox.Text = this.twitter.searchTweets(textTwitter.Text);
                this.twitter.tweetsAnalysisForUser();
                this.twitter.generalAnalysis();
                generateCharts(this.twitter.langPercents, this.twitter.langCount);
            }
        }

        /// <summary>
        /// Captura el token y manda a analizar los post.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void postsAnalysis(object sender, EventArgs e)
        {
            // CAAPhsmMoJk8BAOJXl4e8Mhi9mkH6cddoZA3p1CshScNsDKmh9CVOaF4k9znLd625UhYmHhvKVsJIwkRsSO9kim9uXneJDzookMx2COacUtitAAm4GU1BjPRlmOhdULtXt2nPg6f7Uc7Smu7oyI69e9cZBry6B8ZAuaa5sSrUZAtZANhi2fMqtnfZBGU4gIdZBA7GXWeQZC40hAZDZD
            string token = textFacebook.Text;
            if (!token.Equals(""))
            {
                this.facebook = new FB(token);
                this.facebook.getPosts();
                contentBox.Text = this.facebook.allMessages;
                this.facebook.postsAnalysis();
                this.facebook.generalAnalysis();
                generateCharts(this.facebook.langPercents, this.facebook.langCount);
            }
        }

        /// <summary>
        /// Genera los graficos del analisis de los tweets.
        /// </summary>
        /// <param name="percents"></param>
        /// <param name="langCount"></param>
        protected void generateCharts(double[] percents, double[] langCount)
        {
            tweetChart.Visible = true;
            tweetCChart.Visible = true;

            string[] langs = { "Español", "Ingles", "Alemán", "Holandés", "Desconocido" };            

            tweetChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            tweetChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            tweetChart.Series["tweets"].Points.DataBindXY(langs, percents);

            tweetCChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            tweetCChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            tweetCChart.Series["tweets"].Points.DataBindXY(langs, langCount);
        }

        /// <summary>
        /// Genera los graficos del analisis del sentimiento..
        /// </summary>
        protected void generateLanguageCharts()
        {
            textChart.Visible = true;
            langChart.Visible = true;

            string[] letters = new string[language.percents.Count];
            double[] lettersValues = new double[language.percents.Count];
            Dictionary<char, double> selectedDictionary;
            string[] languageLetters = new string[language.percents.Count]; ;
            double[] languageLettersValues = new double[language.percents.Count];

            if (resultBox.Text == "Spanish")
            {
                selectedDictionary = language.dictionarySpanish;
            }
            else if (resultBox.Text == "English")
            {
                selectedDictionary = language.dictionaryEnglish;
            }
            else if (resultBox.Text == "German")
            {
                selectedDictionary = language.dictionaryGerman;
            }
            else
            {
                selectedDictionary = language.dictionaryDutch;
            }

            int i = 0;
            foreach (KeyValuePair<char, double> letter in language.percents.OrderBy(Letter => Letter.Key))
            {
                letters[i] = letter.Key.ToString();
                lettersValues[i] = letter.Value;

                languageLetters[i] = letter.Key.ToString();
                languageLettersValues[i] = selectedDictionary[letter.Key];

                i++;
            }

            textChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            textChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            textChart.Series["Letras"].Points.DataBindXY(letters, lettersValues);

            langChart.ChartAreas["ChartArea2"].AxisX.Interval = 1;
            langChart.ChartAreas["ChartArea2"].Area3DStyle.Enable3D = true;
            langChart.Series["Letras"].Points.DataBindXY(languageLetters, languageLettersValues);
        }

        /// <summary>
        /// Genera los graficos del analisis de sentimiento.
        /// </summary>
        /// <param name="porcentajes"></param>
        /// <param name="scores"></param>
        protected void generateSentimentCharts(float[] porcentajes, float[] scores)
        {
            string[] categorias = { "Positivo", "Negativo", "Desconocido" };
            string[] decantamiento = { "Positivo", "Negativo" };

            sentimentPercentChart.Visible = true;
            sentimentScoresChart.Visible = true;

            sentimentPercentChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            sentimentPercentChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            sentimentPercentChart.Series["resultados"].Points.DataBindXY(categorias, porcentajes);

            sentimentScoresChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            sentimentScoresChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            sentimentScoresChart.Series["Series1"].Points.DataBindXY(decantamiento, scores);
        }
    }
}