using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace AnalisisEstadistico
{
    public partial class txtView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void buttonUpload_Click(object sender, EventArgs e)
        {
            string fileExtention = System.IO.Path.GetExtension(fileReader.FileName);
            string inputContent;

            if (fileExtention == ".txt")
            {
                using (StreamReader inputStreamReader = new StreamReader(fileReader.PostedFile.InputStream))
                {
                    inputContent = inputStreamReader.ReadToEnd();
                }
                message.Text = inputContent;
            }
            else if (fileExtention == ".doc" || fileExtention == ".docx") 
            {
                inputContent = fileExtention;
            }
            else
            {
                inputContent = "unkown";
            }

            message.Text = inputContent;
        }
    }
}