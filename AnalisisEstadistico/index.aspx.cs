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
        List<emoji> emoticons = new List<emoji>();
        List<string> stopWords = new List<string>();
        List<string> positiveCorpus = new List<string>();
        List<string> negativeCorpus = new List<string>();
        List<string> enhancers = new List<string>();
        List<string> reducers = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> strWords = new List<string>();

            //strWords.Add("hola");
            //strWords.Add("hello");
            //strWords.Add("no");
            strWords.Add("ja");
            strWords.Add("junge");
            strWords.Add("junge");
            strWords.Add("junge");
            strWords.Add("gracias");
            //strWords.Add("não");


            identificarIdioma(strWords);

            // Add words to positivecorpus list for find a new word
            positiveCorpus.Add("and");
            positiveCorpus.Add("y");
            positiveCorpus.Add("mas");

            negativeCorpus.Add("but");
            negativeCorpus.Add("or");
            negativeCorpus.Add("either");
            negativeCorpus.Add("pero");
            negativeCorpus.Add("o");

            // Add words to enhancers/reducers list for multiply de score of the words
            enhancers.Add("muy");
            enhancers.Add("más");
            enhancers.Add("mas");
            enhancers.Add("bastante");

            reducers.Add("poco");
            reducers.Add("menos");
            reducers.Add("casi");
        }

        protected void insertIntoEmoji()
        {
            // Add emoticons to emoticons list
            emoticons.Add(new emoji { emoticon = ":-)", score = 3 });
            emoticons.Add(new emoji { emoticon = ":)", score = 3 });
            emoticons.Add(new emoji { emoticon = "=)", score = 3 });
            emoticons.Add(new emoji { emoticon = ":D", score = 3 });
            emoticons.Add(new emoji { emoticon = ":o)", score = 3 });
            emoticons.Add(new emoji { emoticon = ":]", score = 3 });
            emoticons.Add(new emoji { emoticon = ":3", score = 3 });
            emoticons.Add(new emoji { emoticon = ":>", score = 3 });
            emoticons.Add(new emoji { emoticon = "=]", score = 3 });
            emoticons.Add(new emoji { emoticon = ":}", score = 3 });
            emoticons.Add(new emoji { emoticon = ":-D", score = 3 });
            emoticons.Add(new emoji { emoticon = "8-D", score = 3 });
            emoticons.Add(new emoji { emoticon = "xD", score = 3 });
            emoticons.Add(new emoji { emoticon = "X-D", score = 3 });
            emoticons.Add(new emoji { emoticon = "XD", score = 3 });
            emoticons.Add(new emoji { emoticon = "=-D", score = 3 });
            emoticons.Add(new emoji { emoticon = "=D", score = 3 });
            emoticons.Add(new emoji { emoticon = "=-3", score = 3 });
            emoticons.Add(new emoji { emoticon = "=3", score = 3 });
            emoticons.Add(new emoji { emoticon = ":'-)", score = 3 });
            emoticons.Add(new emoji { emoticon = ":')", score = 3 });
            emoticons.Add(new emoji { emoticon = ":*", score = 3 });
            emoticons.Add(new emoji { emoticon = ";*", score = 3 });
            emoticons.Add(new emoji { emoticon = "", score = 3 });
            emoticons.Add(new emoji { emoticon = ";-)", score = 3 });
            emoticons.Add(new emoji { emoticon = ";)", score = 3 });
            emoticons.Add(new emoji { emoticon = ";-]", score = 3 });
            emoticons.Add(new emoji { emoticon = ";]", score = 3 });
            emoticons.Add(new emoji { emoticon = ";D", score = 3 });
            emoticons.Add(new emoji { emoticon = ":-,", score = 3 });
            emoticons.Add(new emoji { emoticon = ":P", score = 3 });
            emoticons.Add(new emoji { emoticon = ";-P", score = 3 });
            emoticons.Add(new emoji { emoticon = "X-P", score = 3 });
            emoticons.Add(new emoji { emoticon = "xp", score = 3 });
            emoticons.Add(new emoji { emoticon = "x-p", score = 3 });
            emoticons.Add(new emoji { emoticon = ":-p", score = 3 });
            emoticons.Add(new emoji { emoticon = ":p", score = 3 });
            emoticons.Add(new emoji { emoticon = "=p", score = 3 });
            emoticons.Add(new emoji { emoticon = "#-)", score = 3 });
            emoticons.Add(new emoji { emoticon = ":v", score = 3 });
            emoticons.Add(new emoji { emoticon = ":'v", score = 3 });
            emoticons.Add(new emoji { emoticon = ";v", score = 3 });
            emoticons.Add(new emoji { emoticon = "<3", score = 3 });
            emoticons.Add(new emoji { emoticon = "^^", score = 3 });
            emoticons.Add(new emoji { emoticon = "^.^", score = 3 });
            emoticons.Add(new emoji { emoticon = "*.*", score = 3 });
            emoticons.Add(new emoji { emoticon = ":-(", score = -3 });
            emoticons.Add(new emoji { emoticon = ":(", score = -3 });
            emoticons.Add(new emoji { emoticon = "=(", score = -3 });
            emoticons.Add(new emoji { emoticon = ";(", score = -3 });
            emoticons.Add(new emoji { emoticon = ":c", score = -3 });
            emoticons.Add(new emoji { emoticon = ">:v", score = -3 });
            emoticons.Add(new emoji { emoticon = ":'c", score = -3 });
            emoticons.Add(new emoji { emoticon = ":-<", score = -3 });
            emoticons.Add(new emoji { emoticon = ":<", score = -3 });
            emoticons.Add(new emoji { emoticon = ":-[", score = -3 });
            emoticons.Add(new emoji { emoticon = ":[", score = -3 });
            emoticons.Add(new emoji { emoticon = ":{", score = -3 });
            emoticons.Add(new emoji { emoticon = ":-|", score = -3 });
            emoticons.Add(new emoji { emoticon = ":'(", score = -3 });
            emoticons.Add(new emoji { emoticon = ":@", score = -3 });
            emoticons.Add(new emoji { emoticon = ">:[", score = -3 });
            emoticons.Add(new emoji { emoticon = "Q.Q", score = -3 });
            emoticons.Add(new emoji { emoticon = ":#", score = -3 });
            emoticons.Add(new emoji { emoticon = ":-#", score = -3 });
            emoticons.Add(new emoji { emoticon = "-.-", score = -3 });
            emoticons.Add(new emoji { emoticon = ".-.", score = -3 });
            emoticons.Add(new emoji { emoticon = "._.", score = -3 });
            emoticons.Add(new emoji { emoticon = "x_x", score = -3 });
            emoticons.Add(new emoji { emoticon = "X_X", score = -3 });
            emoticons.Add(new emoji { emoticon = "-.-'", score = -3 });
            emoticons.Add(new emoji { emoticon = ":/", score = -3 });
            emoticons.Add(new emoji { emoticon = ":-/", score = -3 });
            emoticons.Add(new emoji { emoticon = ";/", score = -3 });
            emoticons.Add(new emoji { emoticon = ":|", score = -3 });
            emoticons.Add(new emoji { emoticon = "=_=", score = -3 });
            emoticons.Add(new emoji { emoticon = "-_-", score = -3 });
            emoticons.Add(new emoji { emoticon = "?_?", score = -3 });
            emoticons.Add(new emoji { emoticon = "-\"-", score = -3 });

            using (var db = new AnalizadorBDEntities())
            {
                foreach (var emoticon in emoticons)
                {
                    db.emojis.Add(emoticon);
                    db.SaveChanges();
                }
            }
        }

        protected void insertIntoFeelingWord()
        {
            //// Add words to dictionary list
            feelingWords.Add(new feelingWord { word = "abiertamente", score = 2 });
            feelingWords.Add(new feelingWord { word = "abrazo", score = 3 });
            feelingWords.Add(new feelingWord { word = "absoluta", score = 3 });
            feelingWords.Add(new feelingWord { word = "absolutamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "absorbe", score = 3 });
            feelingWords.Add(new feelingWord { word = "absorbente", score = 3 });
            feelingWords.Add(new feelingWord { word = "absorto", score = 3 });
            feelingWords.Add(new feelingWord { word = "abunda", score = 3 });
            feelingWords.Add(new feelingWord { word = "abundan", score = 3 });
            feelingWords.Add(new feelingWord { word = "abundancia", score = 3 });
            feelingWords.Add(new feelingWord { word = "abundando", score = 3 });
            feelingWords.Add(new feelingWord { word = "abundante", score = 3 });
            feelingWords.Add(new feelingWord { word = "acariciado", score = 3 });
            feelingWords.Add(new feelingWord { word = "accesible", score = 3 });
            feelingWords.Add(new feelingWord { word = "acción", score = 3 });
            feelingWords.Add(new feelingWord { word = "acelerado", score = 3 });
            feelingWords.Add(new feelingWord { word = "aceptable", score = 3 });
            feelingWords.Add(new feelingWord { word = "aceptación", score = 3 });
            feelingWords.Add(new feelingWord { word = "aceptada", score = 3 });
            feelingWords.Add(new feelingWord { word = "aceptar", score = 3 });
            feelingWords.Add(new feelingWord { word = "aclamación", score = 3 });
            feelingWords.Add(new feelingWord { word = "aclamada", score = 3 });
            feelingWords.Add(new feelingWord { word = "aclara", score = 3 });
            feelingWords.Add(new feelingWord { word = "acogedor", score = 3 });
            feelingWords.Add(new feelingWord { word = "acomodación", score = 3 });
            feelingWords.Add(new feelingWord { word = "acomodado", score = 3 });
            feelingWords.Add(new feelingWord { word = "acomodar", score = 3 });
            feelingWords.Add(new feelingWord { word = "acomodaticia", score = 3 });
            feelingWords.Add(new feelingWord { word = "actitud", score = 3 });
            feelingWords.Add(new feelingWord { word = "actitud", score = 3 });
            feelingWords.Add(new feelingWord { word = "activar", score = 3 });
            feelingWords.Add(new feelingWord { word = "activo", score = 3 });
            feelingWords.Add(new feelingWord { word = "actualidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "actualizable", score = 3 });
            feelingWords.Add(new feelingWord { word = "actualización", score = 3 });
            feelingWords.Add(new feelingWord { word = "actualizado", score = 3 });
            feelingWords.Add(new feelingWord { word = "acuerdan", score = 3 });
            feelingWords.Add(new feelingWord { word = "adaptabilidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "adaptación", score = 3 });
            feelingWords.Add(new feelingWord { word = "adecuada", score = 3 });
            feelingWords.Add(new feelingWord { word = "adecuadamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "adecuado", score = 3 });
            feelingWords.Add(new feelingWord { word = "además", score = 3 });
            feelingWords.Add(new feelingWord { word = "adelante", score = 3 });
            feelingWords.Add(new feelingWord { word = "admirablemente", score = 3 });
            feelingWords.Add(new feelingWord { word = "admiración", score = 3 });
            feelingWords.Add(new feelingWord { word = "adorable", score = 3 });
            feelingWords.Add(new feelingWord { word = "afable", score = 3 });
            feelingWords.Add(new feelingWord { word = "afecto", score = 3 });
            feelingWords.Add(new feelingWord { word = "afirmación", score = 3 });
            feelingWords.Add(new feelingWord { word = "afirmar", score = 3 });
            feelingWords.Add(new feelingWord { word = "afirmativa", score = 3 });
            feelingWords.Add(new feelingWord { word = "afortunado", score = 3 });
            feelingWords.Add(new feelingWord { word = "afortunadamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "agrada", score = 3 });
            feelingWords.Add(new feelingWord { word = "agradable", score = 3 });
            feelingWords.Add(new feelingWord { word = "agradecido", score = 3 });
            feelingWords.Add(new feelingWord { word = "agradecimiento", score = 3 });
            feelingWords.Add(new feelingWord { word = "ahorro", score = 3 });
            feelingWords.Add(new feelingWord { word = "ahorros", score = 3 });
            feelingWords.Add(new feelingWord { word = "alcanzable", score = 3 });
            feelingWords.Add(new feelingWord { word = "alcanzar", score = 3 });
            feelingWords.Add(new feelingWord { word = "alegre", score = 3 });
            feelingWords.Add(new feelingWord { word = "alegremente", score = 3 });
            feelingWords.Add(new feelingWord { word = "alegría", score = 3 });
            feelingWords.Add(new feelingWord { word = "alentar", score = 3 });
            feelingWords.Add(new feelingWord { word = "alimentos", score = 3 });
            feelingWords.Add(new feelingWord { word = "aliviado", score = 3 });
            feelingWords.Add(new feelingWord { word = "aliviar", score = 3 });
            feelingWords.Add(new feelingWord { word = "alivio", score = 3 });
            feelingWords.Add(new feelingWord { word = "amable", score = 3 });
            feelingWords.Add(new feelingWord { word = "amado", score = 3 });
            feelingWords.Add(new feelingWord { word = "amigo", score = 3 });
            feelingWords.Add(new feelingWord { word = "amistad", score = 3 });
            feelingWords.Add(new feelingWord { word = "amor", score = 3 });
            feelingWords.Add(new feelingWord { word = "amistoso", score = 3 });
            feelingWords.Add(new feelingWord { word = "amistosa", score = 3 });
            feelingWords.Add(new feelingWord { word = "animar", score = 3 });
            feelingWords.Add(new feelingWord { word = "ánimo", score = 3 });
            feelingWords.Add(new feelingWord { word = "apertura", score = 3 });
            feelingWords.Add(new feelingWord { word = "aprecio", score = 3 });
            feelingWords.Add(new feelingWord { word = "aprender", score = 3 });
            feelingWords.Add(new feelingWord { word = "apropiado", score = 3 });
            feelingWords.Add(new feelingWord { word = "aprendizaje", score = 3 });
            feelingWords.Add(new feelingWord { word = "armonía", score = 3 });
            feelingWords.Add(new feelingWord { word = "atención", score = 3 });
            feelingWords.Add(new feelingWord { word = "atractivo", score = 3 });
            feelingWords.Add(new feelingWord { word = "auténtico", score = 3 });
            feelingWords.Add(new feelingWord { word = "autónoma", score = 3 });
            feelingWords.Add(new feelingWord { word = "autonomía", score = 3 });
            feelingWords.Add(new feelingWord { word = "avance", score = 3 });
            feelingWords.Add(new feelingWord { word = "avanzada", score = 3 });
            feelingWords.Add(new feelingWord { word = "aventajado", score = 3 });
            feelingWords.Add(new feelingWord { word = "ayudado", score = 3 });
            feelingWords.Add(new feelingWord { word = "ayudando", score = 3 });
            feelingWords.Add(new feelingWord { word = "barato", score = 3 });
            feelingWords.Add(new feelingWord { word = "bastante", score = 3 });
            feelingWords.Add(new feelingWord { word = "bella", score = 3 });
            feelingWords.Add(new feelingWord { word = "belleza", score = 3 });
            feelingWords.Add(new feelingWord { word = "bendición", score = 3 });
            feelingWords.Add(new feelingWord { word = "bendita", score = 3 });
            feelingWords.Add(new feelingWord { word = "beneficio", score = 3 });
            feelingWords.Add(new feelingWord { word = "beneficios", score = 3 });
            feelingWords.Add(new feelingWord { word = "beneficioso", score = 3 });
            feelingWords.Add(new feelingWord { word = "beneficiosamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "beso", score = 3 });
            feelingWords.Add(new feelingWord { word = "bien", score = 3 });
            feelingWords.Add(new feelingWord { word = "bienestar", score = 3 });
            feelingWords.Add(new feelingWord { word = "bondad", score = 3 });
            feelingWords.Add(new feelingWord { word = "bonitamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "bono", score = 3 });
            feelingWords.Add(new feelingWord { word = "brilla", score = 3 });
            feelingWords.Add(new feelingWord { word = "brillante", score = 3 });
            feelingWords.Add(new feelingWord { word = "brillo", score = 3 });
            feelingWords.Add(new feelingWord { word = "caballerosidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "calidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "calificado", score = 3 });
            feelingWords.Add(new feelingWord { word = "calificar", score = 3 });
            feelingWords.Add(new feelingWord { word = "calma", score = 3 });
            feelingWords.Add(new feelingWord { word = "cambio", score = 3 });
            feelingWords.Add(new feelingWord { word = "capacidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "capaz", score = 3 });
            feelingWords.Add(new feelingWord { word = "cariño", score = 3 });
            feelingWords.Add(new feelingWord { word = "cariñoso", score = 3 });
            feelingWords.Add(new feelingWord { word = "cautivar", score = 3 });
            feelingWords.Add(new feelingWord { word = "cautivó", score = 3 });
            feelingWords.Add(new feelingWord { word = "celebración", score = 3 });
            feelingWords.Add(new feelingWord { word = "celebrar", score = 3 });
            feelingWords.Add(new feelingWord { word = "certeza", score = 3 });
            feelingWords.Add(new feelingWord { word = "cercanía", score = 3 });
            feelingWords.Add(new feelingWord { word = "chistoso", score = 3 });
            feelingWords.Add(new feelingWord { word = "cielo", score = 3 });
            feelingWords.Add(new feelingWord { word = "chistosa", score = 3 });
            feelingWords.Add(new feelingWord { word = "cierto", score = 3 });
            feelingWords.Add(new feelingWord { word = "citas", score = 3 });
            feelingWords.Add(new feelingWord { word = "clara", score = 3 });
            feelingWords.Add(new feelingWord { word = "claramente", score = 3 });
            feelingWords.Add(new feelingWord { word = "claridad", score = 3 });
            feelingWords.Add(new feelingWord { word = "claro", score = 3 });
            feelingWords.Add(new feelingWord { word = "clásico", score = 3 });
            feelingWords.Add(new feelingWord { word = "coherencia", score = 3 });
            feelingWords.Add(new feelingWord { word = "comfortable", score = 3 });
            feelingWords.Add(new feelingWord { word = "cómodamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "cómodo", score = 3 });
            feelingWords.Add(new feelingWord { word = "compañerismo", score = 3 });
            feelingWords.Add(new feelingWord { word = "compartir", score = 3 });
            feelingWords.Add(new feelingWord { word = "compasión", score = 3 });
            feelingWords.Add(new feelingWord { word = "competencia", score = 3 });
            feelingWords.Add(new feelingWord { word = "complementado", score = 3 });
            feelingWords.Add(new feelingWord { word = "complemento", score = 3 });
            feelingWords.Add(new feelingWord { word = "comprensivo", score = 3 });
            feelingWords.Add(new feelingWord { word = "comprobado", score = 3 });
            feelingWords.Add(new feelingWord { word = "comprometido", score = 3 });
            feelingWords.Add(new feelingWord { word = "compromiso", score = 3 });
            feelingWords.Add(new feelingWord { word = "competitiva", score = 3 });
            feelingWords.Add(new feelingWord { word = "comunicación", score = 3 });
            feelingWords.Add(new feelingWord { word = "comunión", score = 3 });
            feelingWords.Add(new feelingWord { word = "concentración", score = 3 });
            feelingWords.Add(new feelingWord { word = "conciencia", score = 3 });
            feelingWords.Add(new feelingWord { word = "conectividad", score = 3 });
            feelingWords.Add(new feelingWord { word = "conexión", score = 3 });
            feelingWords.Add(new feelingWord { word = "confiabilidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "confiable", score = 3 });
            feelingWords.Add(new feelingWord { word = "confianza", score = 3 });
            feelingWords.Add(new feelingWord { word = "confiar", score = 3 });
            feelingWords.Add(new feelingWord { word = "confort", score = 3 });
            feelingWords.Add(new feelingWord { word = "conocimiento", score = 3 });
            feelingWords.Add(new feelingWord { word = "consciente", score = 3 });
            feelingWords.Add(new feelingWord { word = "consideración", score = 3 });
            feelingWords.Add(new feelingWord { word = "consistencia", score = 3 });
            feelingWords.Add(new feelingWord { word = "consolidación", score = 3 });
            feelingWords.Add(new feelingWord { word = "constancia", score = 3 });
            feelingWords.Add(new feelingWord { word = "contento", score = 3 });
            feelingWords.Add(new feelingWord { word = "continuidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "contribución", score = 3 });
            feelingWords.Add(new feelingWord { word = "conveniencia", score = 3 });
            feelingWords.Add(new feelingWord { word = "conveniente", score = 3 });
            feelingWords.Add(new feelingWord { word = "convincente", score = 3 });
            feelingWords.Add(new feelingWord { word = "convicción", score = 3 });
            feelingWords.Add(new feelingWord { word = "cooperación", score = 3 });
            feelingWords.Add(new feelingWord { word = "coraje", score = 3 });
            feelingWords.Add(new feelingWord { word = "corazón", score = 3 });
            feelingWords.Add(new feelingWord { word = "correctamente", score = 3 });
            feelingWords.Add(new feelingWord { word = "correcto", score = 3 });
            feelingWords.Add(new feelingWord { word = "cortés", score = 3 });
            feelingWords.Add(new feelingWord { word = "cortesía", score = 3 });
            feelingWords.Add(new feelingWord { word = "creativo", score = 3 });
            feelingWords.Add(new feelingWord { word = "crecer", score = 3 });
            feelingWords.Add(new feelingWord { word = "cuidado", score = 3 });
            feelingWords.Add(new feelingWord { word = "cumplido", score = 3 });
            feelingWords.Add(new feelingWord { word = "cumple", score = 3 });
            feelingWords.Add(new feelingWord { word = "curiosidad", score = 3 });
            feelingWords.Add(new feelingWord { word = "cumplir", score = 3 });
            feelingWords.Add(new feelingWord { word = "cumplimiento", score = 3 });
            feelingWords.Add(new feelingWord { word = "deber", score = 3 });
            feelingWords.Add(new feelingWord { word = "decente", score = 3 });
            feelingWords.Add(new feelingWord { word = "decisivo", score = 3 });
            feelingWords.Add(new feelingWord { word = "decencia", score = 3 });
            feelingWords.Add(new feelingWord { word = "delicado", score = 3 });
            feelingWords.Add(new feelingWord { word = "deliciosa", score = 3 });
            feelingWords.Add(new feelingWord { word = "delicioso", score = 3 });
            feelingWords.Add(new feelingWord { word = "deportivo", score = 3 });
            feelingWords.Add(new feelingWord { word = "derecho", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });
            feelingWords.Add(new feelingWord { word = "", score = 3 });

            // deliciosas, , , , derrame, derrota, derrotado, derrotando, derrotas, descansamos, descaradamente, descubrimiento, deseable, deseando, deseo, desinterés, deslumbrado, desmontable, despejado, despertó, despierte, despliegue, desprendimiento, despreocupación, despreocupada, destacar, destino, destreza, desvergonzado, determinación, deuda, devoción, devolucion de dinero, devoto, devuelto, dichosamente, dichoso, diestramente, diestro, dificultad para respirar, digna, dignidad, dignificar, digno, diligencia, diligente, diligentemente, diluyente, dinámica, dios, dios manda, dios mio, diosa, diplomática, direccion, disciplina, discreción, disfruta, disfruta de, disfrutado, disfrutar, disfrute, disponible, dispuestos, distinción, distinguido, distintivo, diversidad, diversificada, divertida, divertido, divina, divinamente, divino, dócil, domina, dominado, dominar, dorado, dotado, dulce, dulzura, dummy-prueba, duradero

            feelingWords.Add(new feelingWord { word = "malo", score = -2 });
            feelingWords.Add(new feelingWord { word = "horrible", score = -3 });
            feelingWords.Add(new feelingWord { word = "no", score = -2 });
            feelingWords.Add(new feelingWord { word = "triste", score = -3 });
            feelingWords.Add(new feelingWord { word = "enojado", score = -3 });
            feelingWords.Add(new feelingWord { word = "hambre", score = -1 });

            using (var db = new AnalizadorBDEntities())
            {
                foreach (var feelingWord in feelingWords)
                {
                    db.feelingWords.Add(feelingWord);
                    db.SaveChanges();
                }
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

        protected void buttonAnalizar_Click(object sender, EventArgs e)
        {
            sentimentAnalysis();
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

        protected void sentimentAnalysis()
        {
            List<dynamic> listWords = crawler();
            // Call a method that returns the score of the words.

            //if (listWordsP.Count() > 0 || listWordsN.Count() > 0)
            //{
            //    if (listWordsP.Count() > listWordsN.Count())
            //    {
            //        resultBox.Text = "Positivo. ";

            //        foreach (dynamic word in listWordsP)
            //        {
            //            resultBox.Text = resultBox.Text + word.word;
            //        }
            //    }
            //    else if (listWordsP.Count() < listWordsN.Count())
            //    {
            //        resultBox.Text = "Negativo. ";

            //        foreach (dynamic word in listWordsN)
            //        {
            //            resultBox.Text = resultBox.Text + word.word;
            //        }
            //    }
            //    else
            //    {
            //        resultBox.Text = "Neutro.";
            //    }
            //}
            //else
            //{
            //    resultBox.Text = "No detectado.";
            //}
        }

        protected string removeStopWords()
        {
            string[] words = contentBox.Text.Replace(",", " ").Replace(".", " ").Split(' ');
            string content = "";

            foreach (string word in words)
            {
                if (!stopWords.Contains(word))
                {
                    content += word + " ";
                }
            }

            return content;
        }
        /// <summary>
        /// Busca las palabras en el texto que coincidan con las que se buscan.
        /// </summary>
        /// <returns>Una lista con las palabras encontradas</returns>
        protected List<dynamic> crawler()
        {
            List<dynamic> listWords = new List<dynamic>();
            string content = removeStopWords();

            foreach (dynamic word in listWords)
            {
                if (content.Contains(word.word.ToString()))
                {
                    listWords.Add(word);
                }
            }

            return listWords;
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


            //strWords.Add("hola");
            //strWords.Add("hello");
            //strWords.Add("no");
                     ///strWords.Add("ja");
                     ///strWords.Add("junge");
                    ///strWords.Add("gracias");
            //strWords.Add("não");

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
    }
}