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
using TweetSharp;

namespace AnalisisEstadistico
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Lenguaje l = new Lenguaje();
            Label1.Text = l.saludar();
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
                    message.Text = "Fallo al descomprimir";
                }
            }
            else
            {
                message.Text = "unkown";
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
            message.Text = content;
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
            message.Text = totaltext;
            docs.Close();
            word.Quit();
        }

        protected void saveFile(string ruta)
        {
            message.Text = ruta;
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
            message.Text = strResult;
        }

    
        protected void searchTweets(object sender, EventArgs e)
        {
            string txtTwitterName = searchTweetsTB.Text;
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
            searchTweetsTB.Text = strTweets;
            }
        }
    }
}