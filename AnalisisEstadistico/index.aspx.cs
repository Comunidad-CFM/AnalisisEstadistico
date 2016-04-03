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
using AnalisisEstadistico.Modulos;

namespace AnalisisEstadistico
{
    public partial class index : System.Web.UI.Page
    {
        public Sentiment sentiment;
        public Language language;
        public Twitter twitter;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.sentiment = new Sentiment();
            this.language = new Language();
            this.twitter = new Twitter();
        }

        protected void cleanTextArea(object sender, EventArgs e) 
        {
            contentBox.Text = "";
            resultBox.Text = "";
            textChart.Visible = false;
            langChart.Visible = false;
            sentimentPercentChart.Visible = true;
            sentimentScoresChart.Visible = true;
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
            string rutaDescomprimir = Server.MapPath("~") + "zips\\descomprimidos\\";
            saveFile(ruta);

            if (extract(ruta, rutaDescomprimir))
            {
                readAllInFolder(rutaDescomprimir);
            }
            else
            {
                contentBox.Text = "Fallo al descomprimir";
            }
        }

        protected void click_searchTweets(object sender, EventArgs e)
        {
            string userTweets = textTwitter.Text;

            if (userTweets != "")
            {
                contentBox.Text = this.twitter.searchTweets(userTweets);
            }
        }

        protected void sentimentAnalysis(object sender, EventArgs e)
        {
            if (!contentBox.Text.Equals(""))
            {
                resultBox.Text = ""; // Se vacia el textarea de resultados.
                this.sentiment.text = contentBox.Text; // Se le asigna el texto a analizar.
                resultBox.Text = this.sentiment.sentimentAnalysis();
                resultBox.Text = resultBox.Text + this.sentiment.showScores();
                resultBox.Text = resultBox.Text + this.sentiment.giveProbabilities();
                generateSentimentCharts();
            }
        }

        protected void languageAnalysis(object sender, EventArgs e) 
        {
            if (!contentBox.Text.Equals(""))
            {
                language.text = contentBox.Text;
                resultBox.Text = language.languageAnalisys();
                generateLanguageCharts();
            }
        }

        protected void tweetsAnalysis(object sender, EventArgs e) 
        {
            resultBox.Text = this.twitter.tweetsAnalysis();
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

        protected void generateSentimentCharts()
        {
            sentimentPercentChart.Visible = true;
            sentimentScoresChart.Visible = true;

            sentimentPercentChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            sentimentPercentChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            sentimentPercentChart.Series["resultados"].Points.DataBindXY(this.sentiment.categorias, this.sentiment.porcentajes);

            sentimentScoresChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            sentimentScoresChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            sentimentScoresChart.Series["Series1"].Points.DataBindXY(this.sentiment.decantamiento, this.sentiment.scores);
        }
    }
}