using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace AnalisisEstadistico.Views
{
    public partial class txtView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected string getPathFile() {
            if (fileReader.HasFile)
            {
                try
                {
                    //string filename = Path.GetFileName(fileReader.PostedFile.FileName);
                    string filename = Server.MapPath(fileReader.FileName);
                    return filename;
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }
            else 
            { 
                return "Nothing";
            }
        }

        protected void readFile(string path) {
            /*string[] lines;
            var list = new List<string>();
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            lines = list.ToArray();*/

            string[] lines = File.ReadAllLines(path, Encoding.UTF8);

            foreach (string line in lines)
            {
                message.Text = line;
            }
        }

        protected void buttonUpload_Click(object sender, EventArgs e)
        {
            string path = getPathFile();
            readFile(path);
            //message.Text = path;
        }
    }
}