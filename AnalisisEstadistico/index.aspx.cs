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

namespace AnalisisEstadistico
{
    public partial class index : System.Web.UI.Page
    {
        List<string> listPositiveWords = new List<string>();
        List<string> listNegativeWords = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            listPositiveWords.Add("bueno");
            listPositiveWords.Add("genial");
            listPositiveWords.Add("si");
            listPositiveWords.Add("feliz");
            listPositiveWords.Add("alegre");
            listPositiveWords.Add("contento");
            listPositiveWords.Add(":)");
            listPositiveWords.Add(":D");

            listNegativeWords.Add("malo");
            listNegativeWords.Add("horrible");
            listNegativeWords.Add("no");
            listNegativeWords.Add("triste");
            listNegativeWords.Add("enojado");
            listNegativeWords.Add("hambre");
            listNegativeWords.Add(":(");
            listNegativeWords.Add(":c");
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
                    //message.Text = "Fallo al descomprimir";
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
            
            String strResult;
            WebResponse objResponse;
            WebRequest objRequest = HttpWebRequest.Create(link);
            objResponse = objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                strResult = sr.ReadToEnd();
                sr.Close();
            }
            //contentBox.Text = strResult;
            contentBox.Text = Regex.Replace(strResult, "<(.|\\n)*?>", string.Empty);

            //var wc = new WebClient();
            //var html = wc.DownloadString(textLink.Text);
            //contentBox.Text = Regex.Replace(html, "<(.|\\n)*?>", string.Empty);
           
          
 

        }

        protected void buttonAnalizar_Click(object sender, EventArgs e)
        {
            sentimentAnalysis();
        }

    
        protected void searchTweets(object sender, EventArgs e)
        {
            string txtTwitterName = textTwitter.Text;

            if(txtTwitterName != ""){
                var service = new TwitterService("C98uX0MU7n24kXYROPs1YfZGd", "nDEBrbJXszSZKfrOmmDfAm4NNrvDsfqkE5BwvsXsdFVZKJMdQg");

                //AuthenticateWith("Access Token", "AccessTokenSecret");
                service.AuthenticateWith("711043579699982336-scPSu5YliFK6yov7Jf5aQOLrtklaQFU", "ZiwI7zz8oAX37Ht7jLJ0rjlzaT44CQdsyzjarz1xTRmOC");

                //ScreenName="screeen name not username", Count=Number of Tweets / www.Twitter.com/mcansozeri. 
                IEnumerable<TwitterStatus> tweets = service.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions { ScreenName = txtTwitterName, Count = 10, });
                var tweetsList = tweets.ToList();
                string strTweets = "Tweets de "+txtTwitterName+":\n";
                for (int i = 0; i < tweetsList.Count; i++) {
                    strTweets += tweetsList[i].Text+"\n";
                }

                contentBox.Text = strTweets;
            }
        }

        protected void sentimentAnalysis() {
            List<string> listWordsP = crawler(listPositiveWords);
            List<string> listWordsN = crawler(listNegativeWords);

            if (listWordsP.Count() > 0 || listWordsN.Count() > 0)
            {
                if (listWordsP.Count() > listWordsN.Count())
                {
                    resultBox.Text = "Positivo. ";

                    foreach (string word in listWordsP)
                    {
                        resultBox.Text = resultBox.Text + word;
                    }
                }
                else if (listWordsP.Count() < listWordsN.Count())
                {
                    resultBox.Text = "Negativo. ";

                    foreach (string word in listWordsN)
                    {
                        resultBox.Text = resultBox.Text + word;
                    }
                }
                else
                {
                    resultBox.Text = "Neutro.";
                }
            }
            else 
            {
                resultBox.Text = "No detectado.";
            }
        }

        /// <summary>
        /// Busca las palabras en el texto que coincidan con las que se buscan.
        /// </summary>
        /// <returns>Una lista con las palabras encontradas</returns>
        protected List<string> crawler(List<string> listWordsToSearch) { 
            List<string> listWords = new List<string>();
            string content = contentBox.Text;

            foreach (string word in listWordsToSearch)
            {
                if (content.Contains(word)) {
                    listWords.Add(word);
                }
            }

            return listWords;
        }

        /// <summary>
        /// Cuenta la cantidad de apariciones de una determinada palabra en el texto a analizar.
        /// </summary>
        /// <param name="wordToSearch">Palabra a buscar</param>
        /// <returns>Cantidad de apariciones</returns>
        protected int countWords(string wordToSearch) {
            string content = contentBox.Text.Replace(",", " ").Replace(".", " ");
            string[] words = contentBox.Text.Split(' ');
            int cant = 0;

            foreach (string word in words)
            {
                if (word.Equals(wordToSearch))
                {
                    cant++;
                }
            }

            return cant;
        }
    }
}