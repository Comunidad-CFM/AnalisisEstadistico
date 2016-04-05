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

        protected void saveFile(string ruta)
        {
            fileReader.SaveAs(ruta);
        }

        protected void extractBZ2(string ruta, int jsonID)
        {
            FileInfo fileToBeZipped = new FileInfo(ruta);

            using (FileStream fileToDecompressAsStream = fileToBeZipped.OpenRead())
            {
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

        protected string readHtmlJsonXmlTxt(string ruta)
        {
            string content = "";
            content = File.ReadAllText(ruta, Encoding.UTF8);

            content = Regex.Replace(content, "<.*?>", string.Empty);
            return content.ToLower().Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Replace("&", "");
        }

        protected void click_readHtmlJsonXmlTxt(object sender, EventArgs e) 
        {
            string ruta = Server.MapPath("~") + "files\\" + fileReader.FileName;
            saveFile(ruta);

            contentBox.Text = readHtmlJsonXmlTxt(ruta);
        }

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
        protected void click_readDoc(object sender, EventArgs e)
        {
            string ruta = Server.MapPath("~") + "files\\" + fileReader.FileName;
            saveFile(ruta);
            contentBox.Text = readDoc(ruta);
        }

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

        protected void click_readFolder(object sender, EventArgs e)
        {
            readAllInFolder(textLinkFolder.Text);
        }

        protected void click_unpackageZIP(object sender, EventArgs e)
        {
            // Almacenar el .zip en la carpeta zips del proyecto
            string ruta = Server.MapPath("~") + "zips\\" + fileReader.FileName;
            string destino = Server.MapPath("~") + "zipsContent\\";
            cleanFolder(Server.MapPath("~") + "zips\\");
            cleanFolder(destino);
            saveFile(ruta);

            if (extract(ruta, destino))
            {
                readAllInFolder(destino);
            }
            else
            {
                contentBox.Text = "Fallo al descomprimir";
            }
        }

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

        protected void tweetsAnalysis(object sender, EventArgs e) 
        {
            if (!textLinkFolder.Text.Equals("")) // Tweets masivos
            {
                this.twitter = new Twitter(Server.MapPath("~") + "twitterJSON\\");
                this.twitter.tweetsAnalysis();
                this.twitter.generalAnalysis();
                generateCharts(this.twitter.langPercents, this.twitter.langCount);
                resultBox.Text = "Total de tweets: " + this.twitter.tweetList.Count() + "\n";
                resultBox.Text = resultBox.Text + "Cantidad de usuarios diferentes: " + this.twitter.differentUsers.Count().ToString();
            }
            else if (!textTwitter.Text.Equals(""))
            {
                this.twitter = new Twitter();
                contentBox.Text = this.twitter.searchTweets(textTwitter.Text);
                this.twitter.tweetsAnalysisForUser();
                this.twitter.generalAnalysis();
                generateCharts(this.twitter.langPercents, this.twitter.langCount);
            }
        }

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