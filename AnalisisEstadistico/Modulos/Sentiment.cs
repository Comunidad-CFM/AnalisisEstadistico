using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnalisisEstadistico.Model;

namespace AnalisisEstadistico.Modulos
{
    public class Sentiment
    {
        public string text;
        public List<FeelingWord> feelingWords;
        public List<Emoji> emojis = new List<Emoji>();
        public List<string> stopWords = new List<string>();
        public List<Enhancer> enhancers = new List<Enhancer>();
        public List<FeelingWord> feelingWordsList = new List<FeelingWord>();
        public List<Emoji> emojisList = new List<Emoji>();
        public string[] categorias;
        public string[] decantamiento;
        public float[] porcentajes;
        public float[] scores;

        public Sentiment(string text) 
        {
            this.text = text;
            this.feelingWords = new List<FeelingWord>();
            this.emojis = new List<Emoji>();
            this.stopWords = new List<string>();
            this.enhancers = new List<Enhancer>();
            this.feelingWordsList = new List<FeelingWord>();
            this.emojisList = new List<Emoji>();
            this.categorias = new string[3] { "Positivo", "Negativo", "Desconocido" };
            this.decantamiento = new string[2] { "Positivo", "Negativo" };
            
            this.insertIntoStopWords();
            this.insertIntoEmojis();
            this.insertIntoFeelingWords();
            this.insertIntoEnhancers();
        }

        protected void insertIntoStopWords()
        {
            stopWords.Add("un");
            stopWords.Add("una");
            stopWords.Add("unas");
            stopWords.Add("unos");
            stopWords.Add("uno");
            stopWords.Add("sobre");
            stopWords.Add("todo");
            stopWords.Add("tambien");
            stopWords.Add("tras");
            stopWords.Add("otro");
            stopWords.Add("algun");
            stopWords.Add("alguno");
            stopWords.Add("alguna");
            stopWords.Add("algunos");
            stopWords.Add("algunas");
            stopWords.Add("ser");
            stopWords.Add("es");
            stopWords.Add("soy");
            stopWords.Add("eres");
            stopWords.Add("somos");
            stopWords.Add("sois");
            stopWords.Add("estoy");
            stopWords.Add("esta");
            stopWords.Add("estamos");
            stopWords.Add("estais");
            stopWords.Add("estan");
            stopWords.Add("como");
            stopWords.Add("en");
            stopWords.Add("para");
            stopWords.Add("atras");
            stopWords.Add("porque");
            stopWords.Add("por");
            stopWords.Add("que");
            stopWords.Add("estado");
            stopWords.Add("estaba");
            stopWords.Add("ante");
            stopWords.Add("antes");
            stopWords.Add("siendo");
            stopWords.Add("ambos");
            stopWords.Add("pero");
            stopWords.Add("poder");
            stopWords.Add("puede");
            stopWords.Add("puedo");
            stopWords.Add("podemos");
            stopWords.Add("podeis");
            stopWords.Add("pueden");
            stopWords.Add("fui");
            stopWords.Add("fue");
            stopWords.Add("fuimos");
            stopWords.Add("fueron");
            stopWords.Add("hacer");
            stopWords.Add("hago");
            stopWords.Add("hace");
            stopWords.Add("hacemos");
            stopWords.Add("haceis");
            stopWords.Add("hacen");
            stopWords.Add("cada");
            stopWords.Add("fin");
            stopWords.Add("incluso");
            stopWords.Add("primero");
            stopWords.Add("desde");
            stopWords.Add("conseguir");
            stopWords.Add("consigo");
            stopWords.Add("consigue");
            stopWords.Add("consigues");
            stopWords.Add("conseguimos");
            stopWords.Add("consiguen");
            stopWords.Add("ir");
            stopWords.Add("voy");
            stopWords.Add("va");
            stopWords.Add("vamos");
            stopWords.Add("vais");
            stopWords.Add("van");
            stopWords.Add("ha");
            stopWords.Add("tener");
            stopWords.Add("tengo");
            stopWords.Add("tiene");
            stopWords.Add("tenemos");
            stopWords.Add("teneis");
            stopWords.Add("tienen");
            stopWords.Add("el");
            stopWords.Add("la");
            stopWords.Add("lo");
            stopWords.Add("las");
            stopWords.Add("los");
            stopWords.Add("su");
            stopWords.Add("aqui");
            stopWords.Add("mio");
            stopWords.Add("tuyo");
            stopWords.Add("ellos");
            stopWords.Add("ellas");
            stopWords.Add("nos");
            stopWords.Add("nosotros");
            stopWords.Add("vosotros");
            stopWords.Add("vosotras");
            stopWords.Add("si");
            stopWords.Add("dentro");
            stopWords.Add("solo");
            stopWords.Add("solamente");
            stopWords.Add("saber");
            stopWords.Add("sabes");
            stopWords.Add("sabe");
            stopWords.Add("sabemos");
            stopWords.Add("sabeis");
            stopWords.Add("saben");
            stopWords.Add("ultimo");
            stopWords.Add("largo");
            stopWords.Add("bastante");
            stopWords.Add("haces");
            stopWords.Add("muchos");
            stopWords.Add("aquellos");
            stopWords.Add("aquellas");
            stopWords.Add("sus");
            stopWords.Add("entonces");
            stopWords.Add("tiempo");
            stopWords.Add("dos");
            stopWords.Add("bajo");
            stopWords.Add("arriba");
            stopWords.Add("encima");
            stopWords.Add("usar");
            stopWords.Add("uso");
            stopWords.Add("usas");
            stopWords.Add("usa");
            stopWords.Add("usamos");
            stopWords.Add("usais");
            stopWords.Add("usan");
            stopWords.Add("era");
            stopWords.Add("eras");
            stopWords.Add("eramos");
            stopWords.Add("eran");
            stopWords.Add("modo");
            stopWords.Add("cual");
            stopWords.Add("cuando");
            stopWords.Add("donde");
            stopWords.Add("mientras");
            stopWords.Add("quien");
            stopWords.Add("con");
            stopWords.Add("entre");
            stopWords.Add("sin");
            stopWords.Add("yo");
            stopWords.Add("aquel");
            stopWords.Add("de");
            stopWords.Add("se");
            stopWords.Add("que");
            stopWords.Add("a");
            stopWords.Add("http");
            stopWords.Add("del");
            stopWords.Add("eso");
            stopWords.Add("hay");
            stopWords.Add("este");
            stopWords.Add("tu");
            stopWords.Add("x");
            stopWords.Add("esa");
            stopWords.Add("oye");
            stopWords.Add("han");
            stopWords.Add("segun");
            stopWords.Add("cosa");
            stopWords.Add("te");
            stopWords.Add("al");
        }

        protected void insertIntoEmojis()
        {
            emojis.Add(new Emoji(":-)", 3));
            emojis.Add(new Emoji(":)", 3));
            emojis.Add(new Emoji("=)", 3));
            emojis.Add(new Emoji(":D", 3));
            emojis.Add(new Emoji(":o)", 3));
            emojis.Add(new Emoji(":]", 3));
            emojis.Add(new Emoji(":3", 3));
            emojis.Add(new Emoji(":>", 3));
            emojis.Add(new Emoji("=]", 3));
            emojis.Add(new Emoji(":}", 3));
            emojis.Add(new Emoji(":-D", 3));
            emojis.Add(new Emoji("8-D", 3));
            emojis.Add(new Emoji("xD", 3));
            emojis.Add(new Emoji("X-D", 3));
            emojis.Add(new Emoji("XD", 3));
            emojis.Add(new Emoji("=-D", 3));
            emojis.Add(new Emoji("=D", 3));
            emojis.Add(new Emoji("=-3", 3));
            emojis.Add(new Emoji("=3", 3));
            emojis.Add(new Emoji(":'-)", 3));
            emojis.Add(new Emoji(":')", 3));
            emojis.Add(new Emoji(":*", 3));
            emojis.Add(new Emoji(";*", 3));
            emojis.Add(new Emoji("", 3));
            emojis.Add(new Emoji(";-)", 3));
            emojis.Add(new Emoji(";)", 3));
            emojis.Add(new Emoji(";-]", 3));
            emojis.Add(new Emoji(";]", 3));
            emojis.Add(new Emoji(";D", 3));
            emojis.Add(new Emoji(":-,", 3));
            emojis.Add(new Emoji(":P", 3));
            emojis.Add(new Emoji(";-P", 3));
            emojis.Add(new Emoji("X-P", 3));
            emojis.Add(new Emoji("xp", 3));
            emojis.Add(new Emoji("x-p", 3));
            emojis.Add(new Emoji(":-p", 3));
            emojis.Add(new Emoji(":p", 3));
            emojis.Add(new Emoji("=p", 3));
            emojis.Add(new Emoji("#-)", 3));
            emojis.Add(new Emoji(":v", 3));
            emojis.Add(new Emoji(":'v", 3));
            emojis.Add(new Emoji(";v", 3));
            emojis.Add(new Emoji("<3", 3));
            emojis.Add(new Emoji("^^", 3));
            emojis.Add(new Emoji("^.^", 3));
            emojis.Add(new Emoji("*.*", 3));
            emojis.Add(new Emoji(":-(", -3));
            emojis.Add(new Emoji(":(", -3));
            emojis.Add(new Emoji("=(", -3));
            emojis.Add(new Emoji(";(", -3));
            emojis.Add(new Emoji(":c", -3));
            emojis.Add(new Emoji(">:v", -3));
            emojis.Add(new Emoji(":'c", -3));
            emojis.Add(new Emoji(":-<", -3));
            emojis.Add(new Emoji(":<", -3));
            emojis.Add(new Emoji(":-[", -3));
            emojis.Add(new Emoji(":[", -3));
            emojis.Add(new Emoji(":{", -3));
            emojis.Add(new Emoji(":-|", -3));
            emojis.Add(new Emoji(":'(", -3));
            emojis.Add(new Emoji(":@", -3));
            emojis.Add(new Emoji(">:[", -3));
            emojis.Add(new Emoji("Q.Q", -3));
            emojis.Add(new Emoji(":#", -3));
            emojis.Add(new Emoji(":-#", -3));
            emojis.Add(new Emoji("-.-", -3));
            emojis.Add(new Emoji(".-.", -3));
            emojis.Add(new Emoji("._.", -3));
            emojis.Add(new Emoji("x_x", -3));
            emojis.Add(new Emoji("X_X", -3));
            emojis.Add(new Emoji("-.-'", -3));
            emojis.Add(new Emoji(":/", -3));
            emojis.Add(new Emoji(":-/", -3));
            emojis.Add(new Emoji(";/", -3));
            emojis.Add(new Emoji(":|", -3));
            emojis.Add(new Emoji("=_=", -3));
            emojis.Add(new Emoji("-_-", -3));
            emojis.Add(new Emoji("?_?", -3));
            emojis.Add(new Emoji("-\"-", -3));
        }

        protected void insertIntoFeelingWords()
        {
            feelingWords.Add(new FeelingWord("abiertamente", 1));
            feelingWords.Add(new FeelingWord("abrazo", 3));
            feelingWords.Add(new FeelingWord("absoluta", 2));
            feelingWords.Add(new FeelingWord("absolutamente", 3));
            feelingWords.Add(new FeelingWord("absorbe", 1));
            feelingWords.Add(new FeelingWord("absorbente", 2));
            feelingWords.Add(new FeelingWord("abunda", 2));
            feelingWords.Add(new FeelingWord("abundan", 2));
            feelingWords.Add(new FeelingWord("abundancia", 3));
            feelingWords.Add(new FeelingWord("abundante", 3));
            feelingWords.Add(new FeelingWord("acariciado", 4));
            feelingWords.Add(new FeelingWord("accesible", 4));
            feelingWords.Add(new FeelingWord("acelerado", 2));
            feelingWords.Add(new FeelingWord("aceptable", 3));
            feelingWords.Add(new FeelingWord("aceptación", 4));
            feelingWords.Add(new FeelingWord("aceptada", 3));
            feelingWords.Add(new FeelingWord("aceptar", 3));
            feelingWords.Add(new FeelingWord("aclamada", 2));
            feelingWords.Add(new FeelingWord("aclara", 2));
            feelingWords.Add(new FeelingWord("acogedor", 3));
            feelingWords.Add(new FeelingWord("acomodado", 1));
            feelingWords.Add(new FeelingWord("acomodar", 1));
            feelingWords.Add(new FeelingWord("actitud", 3));
            feelingWords.Add(new FeelingWord("activar", 3));
            feelingWords.Add(new FeelingWord("activo", 3));
            feelingWords.Add(new FeelingWord("actualidad", 2));
            feelingWords.Add(new FeelingWord("actualizable", 3));
            feelingWords.Add(new FeelingWord("actualización", 2));
            feelingWords.Add(new FeelingWord("actualizado", 3));
            feelingWords.Add(new FeelingWord("acuerdan", 2));
            feelingWords.Add(new FeelingWord("adaptabilidad", 3));
            feelingWords.Add(new FeelingWord("adaptación", 2));
            feelingWords.Add(new FeelingWord("adecuada", 2));
            feelingWords.Add(new FeelingWord("adecuadamente", 3));
            feelingWords.Add(new FeelingWord("adecuado", 2));
            feelingWords.Add(new FeelingWord("adelante", 1));
            feelingWords.Add(new FeelingWord("admirablemente", 2));
            feelingWords.Add(new FeelingWord("admiración", 3));
            feelingWords.Add(new FeelingWord("adorable", 3));
            feelingWords.Add(new FeelingWord("afable", 2));
            feelingWords.Add(new FeelingWord("afecto", 2));
            feelingWords.Add(new FeelingWord("afirmación", 1));
            feelingWords.Add(new FeelingWord("afirmar", 2));
            feelingWords.Add(new FeelingWord("afirmativa", 2));
            feelingWords.Add(new FeelingWord("afortunado", 3));
            feelingWords.Add(new FeelingWord("afortunadamente", 3));
            feelingWords.Add(new FeelingWord("agrada", 2));
            feelingWords.Add(new FeelingWord("agradable", 3));
            feelingWords.Add(new FeelingWord("agradecido", 2));
            feelingWords.Add(new FeelingWord("agradecimiento", 3));
            feelingWords.Add(new FeelingWord("ahorro", 2));
            feelingWords.Add(new FeelingWord("ahorros", 3));
            feelingWords.Add(new FeelingWord("alcanzable", 3));
            feelingWords.Add(new FeelingWord("alcanzar", 2));
            feelingWords.Add(new FeelingWord("alegre", 4));
            feelingWords.Add(new FeelingWord("alegremente", 4));
            feelingWords.Add(new FeelingWord("alegría", 4));
            feelingWords.Add(new FeelingWord("alentar", 3));
            feelingWords.Add(new FeelingWord("alimentos", 2));
            feelingWords.Add(new FeelingWord("aliviado", 2));
            feelingWords.Add(new FeelingWord("aliviar", 2));
            feelingWords.Add(new FeelingWord("alivio", 2));
            feelingWords.Add(new FeelingWord("amable", 3));
            feelingWords.Add(new FeelingWord("amado", 3));
            feelingWords.Add(new FeelingWord("amigo", 2));
            feelingWords.Add(new FeelingWord("amistad", 2));
            feelingWords.Add(new FeelingWord("amo", 3));
            feelingWords.Add(new FeelingWord("amor", 3));
            feelingWords.Add(new FeelingWord("amistoso", 2));
            feelingWords.Add(new FeelingWord("amistosa", 2));
            feelingWords.Add(new FeelingWord("animar", 2));
            feelingWords.Add(new FeelingWord("ánimo", 2));
            feelingWords.Add(new FeelingWord("apertura", 1));
            feelingWords.Add(new FeelingWord("aprecio", 2));
            feelingWords.Add(new FeelingWord("aprender", 2));
            feelingWords.Add(new FeelingWord("apropiado", 1));
            feelingWords.Add(new FeelingWord("aprendizaje", 2));
            feelingWords.Add(new FeelingWord("armonía", 2));
            feelingWords.Add(new FeelingWord("atención", 2));
            feelingWords.Add(new FeelingWord("atractivo", 3));
            feelingWords.Add(new FeelingWord("auténtico", 2));
            feelingWords.Add(new FeelingWord("autonomía", 2));
            feelingWords.Add(new FeelingWord("autoridad", 1));
            feelingWords.Add(new FeelingWord("avance", 2));
            feelingWords.Add(new FeelingWord("avanzada", 3));
            feelingWords.Add(new FeelingWord("aventajado", 3));
            feelingWords.Add(new FeelingWord("ayudado", 3));
            feelingWords.Add(new FeelingWord("ayudando", 3));
            feelingWords.Add(new FeelingWord("barato", 2));
            feelingWords.Add(new FeelingWord("bastante", 3));
            feelingWords.Add(new FeelingWord("bella", 3));
            feelingWords.Add(new FeelingWord("belleza", 3));
            feelingWords.Add(new FeelingWord("bendición", 3));
            feelingWords.Add(new FeelingWord("bendita", 3));
            feelingWords.Add(new FeelingWord("beneficio", 3));
            feelingWords.Add(new FeelingWord("beneficios", 3));
            feelingWords.Add(new FeelingWord("beneficioso", 3));
            feelingWords.Add(new FeelingWord("beneficiosamente", 3));
            feelingWords.Add(new FeelingWord("beso", 4));
            feelingWords.Add(new FeelingWord("bien", 3));
            feelingWords.Add(new FeelingWord("bienestar", 3));
            feelingWords.Add(new FeelingWord("bondad", 2));
            feelingWords.Add(new FeelingWord("bonitamente", 3));
            feelingWords.Add(new FeelingWord("bono", 2));
            feelingWords.Add(new FeelingWord("brilla", 2));
            feelingWords.Add(new FeelingWord("brillante", 3));
            feelingWords.Add(new FeelingWord("bueno", 1));
            feelingWords.Add(new FeelingWord("caballerosidad", 3));
            feelingWords.Add(new FeelingWord("calidad", 3));
            feelingWords.Add(new FeelingWord("calificado", 2));
            feelingWords.Add(new FeelingWord("calificar", 3));
            feelingWords.Add(new FeelingWord("calma", 2));
            feelingWords.Add(new FeelingWord("cambio", 2));
            feelingWords.Add(new FeelingWord("capacidad", 2));
            feelingWords.Add(new FeelingWord("capaz", 2));
            feelingWords.Add(new FeelingWord("cariño", 3));
            feelingWords.Add(new FeelingWord("cariñoso", 3));
            feelingWords.Add(new FeelingWord("cautivar", 2));
            feelingWords.Add(new FeelingWord("cautivó", 2));
            feelingWords.Add(new FeelingWord("celebración", 1));
            feelingWords.Add(new FeelingWord("celebrar", 2));
            feelingWords.Add(new FeelingWord("certeza", 1));
            feelingWords.Add(new FeelingWord("cercanía", 1));
            feelingWords.Add(new FeelingWord("chistoso", 1));
            feelingWords.Add(new FeelingWord("chistosa", 1));
            feelingWords.Add(new FeelingWord("cierto", 1));
            feelingWords.Add(new FeelingWord("citas", 1));
            feelingWords.Add(new FeelingWord("clara", 1));
            feelingWords.Add(new FeelingWord("claramente", 2));
            feelingWords.Add(new FeelingWord("claridad", 2));
            feelingWords.Add(new FeelingWord("claro", 1));
            feelingWords.Add(new FeelingWord("coherencia", 2));
            feelingWords.Add(new FeelingWord("comfortable", 2));
            feelingWords.Add(new FeelingWord("cómodamente", 2));
            feelingWords.Add(new FeelingWord("cómodo", 2));
            feelingWords.Add(new FeelingWord("compañerismo", 2));
            feelingWords.Add(new FeelingWord("compartir", 1));
            feelingWords.Add(new FeelingWord("compasión", 1));
            feelingWords.Add(new FeelingWord("competencia", 1));
            feelingWords.Add(new FeelingWord("complementado", 2));
            feelingWords.Add(new FeelingWord("complemento", 2));
            feelingWords.Add(new FeelingWord("comprensivo", 2));
            feelingWords.Add(new FeelingWord("comprobado", 2));
            feelingWords.Add(new FeelingWord("comprometido", 2));
            feelingWords.Add(new FeelingWord("compromiso", 1));
            feelingWords.Add(new FeelingWord("competitiva", 2));
            feelingWords.Add(new FeelingWord("comunicación", 2));
            feelingWords.Add(new FeelingWord("comunión", 1));
            feelingWords.Add(new FeelingWord("concentración", 1));
            feelingWords.Add(new FeelingWord("conciencia", 2));
            feelingWords.Add(new FeelingWord("conectividad", 1));
            feelingWords.Add(new FeelingWord("conexión", 1));
            feelingWords.Add(new FeelingWord("confiabilidad", 2));
            feelingWords.Add(new FeelingWord("confiable", 3));
            feelingWords.Add(new FeelingWord("confianza", 2));
            feelingWords.Add(new FeelingWord("confiar", 3));
            feelingWords.Add(new FeelingWord("confort", 2));
            feelingWords.Add(new FeelingWord("conocimiento", 3));
            feelingWords.Add(new FeelingWord("consciente", 2));
            feelingWords.Add(new FeelingWord("consideración", 2));
            feelingWords.Add(new FeelingWord("consistencia", 2));
            feelingWords.Add(new FeelingWord("consolidación", 1));
            feelingWords.Add(new FeelingWord("constancia", 1));
            feelingWords.Add(new FeelingWord("contento", 3));
            feelingWords.Add(new FeelingWord("continuidad", 2));
            feelingWords.Add(new FeelingWord("contribución", 2));
            feelingWords.Add(new FeelingWord("conveniencia", 2));
            feelingWords.Add(new FeelingWord("conveniente", 2));
            feelingWords.Add(new FeelingWord("convincente", 1));
            feelingWords.Add(new FeelingWord("convicción", 1));
            feelingWords.Add(new FeelingWord("cooperación", 2));
            feelingWords.Add(new FeelingWord("coraje", 2));
            feelingWords.Add(new FeelingWord("correctamente", 2));
            feelingWords.Add(new FeelingWord("correcto", 3));
            feelingWords.Add(new FeelingWord("cortés", 2));
            feelingWords.Add(new FeelingWord("cortesía", 1));
            feelingWords.Add(new FeelingWord("creativo", 1));
            feelingWords.Add(new FeelingWord("crecer", 1));
            feelingWords.Add(new FeelingWord("cumplido", 3));
            feelingWords.Add(new FeelingWord("cumple", 2));
            feelingWords.Add(new FeelingWord("curiosidad", 2));
            feelingWords.Add(new FeelingWord("cumplir", 2));
            feelingWords.Add(new FeelingWord("cumplimiento", 2));
            feelingWords.Add(new FeelingWord("deber", 2));
            feelingWords.Add(new FeelingWord("decente", 2));
            feelingWords.Add(new FeelingWord("decisivo", 2));
            feelingWords.Add(new FeelingWord("decencia", 2));
            feelingWords.Add(new FeelingWord("delicado", 2));
            feelingWords.Add(new FeelingWord("deliciosa", 2));
            feelingWords.Add(new FeelingWord("delicioso", 2));
            feelingWords.Add(new FeelingWord("deportivo", 2));
            feelingWords.Add(new FeelingWord("derecho", 3));
            feelingWords.Add(new FeelingWord("descubrimiento", 1));
            feelingWords.Add(new FeelingWord("deseable", 1));
            feelingWords.Add(new FeelingWord("deseo", 1));
            feelingWords.Add(new FeelingWord("deseando", 2));
            feelingWords.Add(new FeelingWord("despliegue", 1));
            feelingWords.Add(new FeelingWord("despejado", 1));
            feelingWords.Add(new FeelingWord("destreza", 1));
            feelingWords.Add(new FeelingWord("destacar", 2));
            feelingWords.Add(new FeelingWord("determinación", 2));
            feelingWords.Add(new FeelingWord("devoción", 1));
            feelingWords.Add(new FeelingWord("dichosamente", 2));
            feelingWords.Add(new FeelingWord("dichoso", 2));
            feelingWords.Add(new FeelingWord("digna", 2));
            feelingWords.Add(new FeelingWord("digno", 2));
            feelingWords.Add(new FeelingWord("disciplina", 1));
            feelingWords.Add(new FeelingWord("disfruta", 3));
            feelingWords.Add(new FeelingWord("disfrutado", 2));
            feelingWords.Add(new FeelingWord("disfrutar", 3));
            feelingWords.Add(new FeelingWord("disponible", 2));
            feelingWords.Add(new FeelingWord("divertida", 2));
            feelingWords.Add(new FeelingWord("divertido", 2));
            feelingWords.Add(new FeelingWord("divino", 1));
            feelingWords.Add(new FeelingWord("dulce", 2));
            feelingWords.Add(new FeelingWord("dulzura", 2));
            feelingWords.Add(new FeelingWord("educación", 2));
            feelingWords.Add(new FeelingWord("educada", 2));
            feelingWords.Add(new FeelingWord("educado", 2));
            feelingWords.Add(new FeelingWord("efectivamente", 2));
            feelingWords.Add(new FeelingWord("eficaz", 1));
            feelingWords.Add(new FeelingWord("eficacia", 1));
            feelingWords.Add(new FeelingWord("eficiente", 1));
            feelingWords.Add(new FeelingWord("eficiencia", 1));
            feelingWords.Add(new FeelingWord("ejemplar", 2));
            feelingWords.Add(new FeelingWord("elegancia", 2));
            feelingWords.Add(new FeelingWord("elogiar", 2));
            feelingWords.Add(new FeelingWord("elogio", 2));
            feelingWords.Add(new FeelingWord("embellecer", 2));
            feelingWords.Add(new FeelingWord("emoción", 2));
            feelingWords.Add(new FeelingWord("emocionada", 2));
            feelingWords.Add(new FeelingWord("emocionado", 3));
            feelingWords.Add(new FeelingWord("emocionante", 3));
            feelingWords.Add(new FeelingWord("emociones", 2));
            feelingWords.Add(new FeelingWord("emprendedor", 2));
            feelingWords.Add(new FeelingWord("enamorado", 2));
            feelingWords.Add(new FeelingWord("encantado", 2));
            feelingWords.Add(new FeelingWord("encanto", 3));
            feelingWords.Add(new FeelingWord("endulzar", 2));
            feelingWords.Add(new FeelingWord("enhorabuena", 3));
            feelingWords.Add(new FeelingWord("enorme", 3));
            feelingWords.Add(new FeelingWord("enriquecimiento", 2));
            feelingWords.Add(new FeelingWord("enseñar", 1));
            feelingWords.Add(new FeelingWord("entender", 1));
            feelingWords.Add(new FeelingWord("entendimiento", 1));
            feelingWords.Add(new FeelingWord("entusiasmar", 2));
            feelingWords.Add(new FeelingWord("entusiasmo", 2));
            feelingWords.Add(new FeelingWord("equilibrado", 1));
            feelingWords.Add(new FeelingWord("equidad", 1));
            feelingWords.Add(new FeelingWord("equipo", 1));
            feelingWords.Add(new FeelingWord("equitativo", 1));
            feelingWords.Add(new FeelingWord("esfuerzo", 2));
            feelingWords.Add(new FeelingWord("espectacular", 3));
            feelingWords.Add(new FeelingWord("esperanza", 2));
            feelingWords.Add(new FeelingWord("especial", 1));
            feelingWords.Add(new FeelingWord("esplendor", 1));
            feelingWords.Add(new FeelingWord("estudioso", 1));
            feelingWords.Add(new FeelingWord("estupendo", 3));
            feelingWords.Add(new FeelingWord("ética", 2));
            feelingWords.Add(new FeelingWord("euforia", 2));
            feelingWords.Add(new FeelingWord("eufórico", 2));
            feelingWords.Add(new FeelingWord("excelencia", 1));
            feelingWords.Add(new FeelingWord("excelente", 2));
            feelingWords.Add(new FeelingWord("excelentemente", 3));
            feelingWords.Add(new FeelingWord("éxito", 2));
            feelingWords.Add(new FeelingWord("éxitos", 2));
            feelingWords.Add(new FeelingWord("exitosa", 2));
            feelingWords.Add(new FeelingWord("experiencia", 2));
            feelingWords.Add(new FeelingWord("exquisito", 1));
            feelingWords.Add(new FeelingWord("exuberante", 1));
            feelingWords.Add(new FeelingWord("fabuloso", 3));
            feelingWords.Add(new FeelingWord("fácil", 1));
            feelingWords.Add(new FeelingWord("facilidad", 2));
            feelingWords.Add(new FeelingWord("facilitar", 1));
            feelingWords.Add(new FeelingWord("facilita", 1));
            feelingWords.Add(new FeelingWord("factible", 1));
            feelingWords.Add(new FeelingWord("fantástico", 2));
            feelingWords.Add(new FeelingWord("fascinado", 2));
            feelingWords.Add(new FeelingWord("fascinante", 2));
            feelingWords.Add(new FeelingWord("favor", 1));
            feelingWords.Add(new FeelingWord("favorecida", 1));
            feelingWords.Add(new FeelingWord("felicidad", 3));
            feelingWords.Add(new FeelingWord("felicidades", 3));
            feelingWords.Add(new FeelingWord("felicita", 3));
            feelingWords.Add(new FeelingWord("feliz", 3));
            feelingWords.Add(new FeelingWord("fenomenal", 3));
            feelingWords.Add(new FeelingWord("festividad", 2));
            feelingWords.Add(new FeelingWord("festivo", 2));
            feelingWords.Add(new FeelingWord("fiel", 2));
            feelingWords.Add(new FeelingWord("fino", 1));
            feelingWords.Add(new FeelingWord("firme", 1));
            feelingWords.Add(new FeelingWord("formalmente", 1));
            feelingWords.Add(new FeelingWord("fortaleza", 1));
            feelingWords.Add(new FeelingWord("fraternal", 1));
            feelingWords.Add(new FeelingWord("fresco", 1));
            feelingWords.Add(new FeelingWord("fuerte", 2));
            feelingWords.Add(new FeelingWord("fuerza", 2));
            feelingWords.Add(new FeelingWord("futuro", 2));
            feelingWords.Add(new FeelingWord("ganado", 2));
            feelingWords.Add(new FeelingWord("ganador", 2));
            feelingWords.Add(new FeelingWord("ganancias", 1));
            feelingWords.Add(new FeelingWord("ganando", 1));
            feelingWords.Add(new FeelingWord("ganar", 2));
            feelingWords.Add(new FeelingWord("garantía", 1));
            feelingWords.Add(new FeelingWord("generar", 1));
            feelingWords.Add(new FeelingWord("generosamente", 2));
            feelingWords.Add(new FeelingWord("generoso", 2));
            feelingWords.Add(new FeelingWord("generosidad", 2));
            feelingWords.Add(new FeelingWord("genial", 3));
            feelingWords.Add(new FeelingWord("glamour", 2));
            feelingWords.Add(new FeelingWord("gloria", 2));
            feelingWords.Add(new FeelingWord("gloriosa", 2));
            feelingWords.Add(new FeelingWord("gloriosamente", 2));
            feelingWords.Add(new FeelingWord("gracias", 1));
            feelingWords.Add(new FeelingWord("gran", 3));
            feelingWords.Add(new FeelingWord("grande", 3));
            feelingWords.Add(new FeelingWord("grandeza", 3));
            feelingWords.Add(new FeelingWord("gratamente", 2));
            feelingWords.Add(new FeelingWord("gratificante", 3));
            feelingWords.Add(new FeelingWord("gratis", 1));
            feelingWords.Add(new FeelingWord("gusto", 1));
            feelingWords.Add(new FeelingWord("gustos", 1));
            feelingWords.Add(new FeelingWord("gratitud", 1));
            feelingWords.Add(new FeelingWord("habilidad", 1));
            feelingWords.Add(new FeelingWord("habilitado", 1));
            feelingWords.Add(new FeelingWord("hábil", 1));
            feelingWords.Add(new FeelingWord("halagar", 2));
            feelingWords.Add(new FeelingWord("hermosa", 3));
            feelingWords.Add(new FeelingWord("hermosamente", 3));
            feelingWords.Add(new FeelingWord("hermoso", 3));
            feelingWords.Add(new FeelingWord("héroe", 2));
            feelingWords.Add(new FeelingWord("héroes", 2));
            feelingWords.Add(new FeelingWord("heroísmo", 2));
            feelingWords.Add(new FeelingWord("hola", 1));
            feelingWords.Add(new FeelingWord("homenaje", 2));
            feelingWords.Add(new FeelingWord("honestidad", 2));
            feelingWords.Add(new FeelingWord("honesto", 2));
            feelingWords.Add(new FeelingWord("honrado", 2));
            feelingWords.Add(new FeelingWord("humildad", 2));
            feelingWords.Add(new FeelingWord("humilde", 2));
            feelingWords.Add(new FeelingWord("humor", 1));
            feelingWords.Add(new FeelingWord("idolatrado", 2));
            feelingWords.Add(new FeelingWord("idolatrar", 2));
            feelingWords.Add(new FeelingWord("igualdad", 1));
            feelingWords.Add(new FeelingWord("iluminado", 1));
            feelingWords.Add(new FeelingWord("imparable", 2));
            feelingWords.Add(new FeelingWord("imponente", 2));
            feelingWords.Add(new FeelingWord("importante", 2));
            feelingWords.Add(new FeelingWord("impresiona", 1));
            feelingWords.Add(new FeelingWord("impresionante", 2));
            feelingWords.Add(new FeelingWord("inafectado", 1));
            feelingWords.Add(new FeelingWord("impresionantemente", 2));
            feelingWords.Add(new FeelingWord("increíble", 2));
            feelingWords.Add(new FeelingWord("increíblemente", 2));
            feelingWords.Add(new FeelingWord("incuestionable", 3));
            feelingWords.Add(new FeelingWord("indiscutible", 2));
            feelingWords.Add(new FeelingWord("indudable", 1));
            feelingWords.Add(new FeelingWord("infalible", 1));
            feelingWords.Add(new FeelingWord("ingenio", 1));
            feelingWords.Add(new FeelingWord("ingenioso", 2));
            feelingWords.Add(new FeelingWord("ingenuo", 3));
            feelingWords.Add(new FeelingWord("inigualable", 1));
            feelingWords.Add(new FeelingWord("inmejorable", 1));
            feelingWords.Add(new FeelingWord("innovador", 1));
            feelingWords.Add(new FeelingWord("inolvidable", 3));
            feelingWords.Add(new FeelingWord("inspiración", 2));
            feelingWords.Add(new FeelingWord("inspirado", 2));
            feelingWords.Add(new FeelingWord("inteligencia", 2));
            feelingWords.Add(new FeelingWord("inteligente", 3));
            feelingWords.Add(new FeelingWord("interés", 3));
            feelingWords.Add(new FeelingWord("interesado", 2));
            feelingWords.Add(new FeelingWord("interesante", 2));
            feelingWords.Add(new FeelingWord("intereses", 2));
            feelingWords.Add(new FeelingWord("invencible", 2));
            feelingWords.Add(new FeelingWord("investigación", 1));
            feelingWords.Add(new FeelingWord("invertir", 1));
            feelingWords.Add(new FeelingWord("juguetona", 1));
            feelingWords.Add(new FeelingWord("justicia", 1));
            feelingWords.Add(new FeelingWord("justo", 1));
            feelingWords.Add(new FeelingWord("juventud", 1));
            feelingWords.Add(new FeelingWord("juicioso", 1));
            feelingWords.Add(new FeelingWord("juvenil", 1));
            feelingWords.Add(new FeelingWord("leal", 1));
            feelingWords.Add(new FeelingWord("lealtad", 2));
            feelingWords.Add(new FeelingWord("legal", 2));
            feelingWords.Add(new FeelingWord("legible", 1));
            feelingWords.Add(new FeelingWord("legítimo", 1));
            feelingWords.Add(new FeelingWord("liberado", 1));
            feelingWords.Add(new FeelingWord("liberar", 1));
            feelingWords.Add(new FeelingWord("libertad", 1));
            feelingWords.Add(new FeelingWord("líder", 2));
            feelingWords.Add(new FeelingWord("liderazgo", 2));
            feelingWords.Add(new FeelingWord("limpieza", 2));
            feelingWords.Add(new FeelingWord("limpia", 2));
            feelingWords.Add(new FeelingWord("lindo", 2));
            feelingWords.Add(new FeelingWord("lograr", 2));
            feelingWords.Add(new FeelingWord("logro", 2));
            feelingWords.Add(new FeelingWord("logros", 2));
            feelingWords.Add(new FeelingWord("lujoso", 2));
            feelingWords.Add(new FeelingWord("luz", 1));
            feelingWords.Add(new FeelingWord("maestro", 1));
            feelingWords.Add(new FeelingWord("magistral", 2));
            feelingWords.Add(new FeelingWord("mágico", 2));
            feelingWords.Add(new FeelingWord("magnífica", 3));
            feelingWords.Add(new FeelingWord("magníficamente", 3));
            feelingWords.Add(new FeelingWord("magnífico", 3));
            feelingWords.Add(new FeelingWord("manejable", 1));
            feelingWords.Add(new FeelingWord("maravillaron", 2));
            feelingWords.Add(new FeelingWord("maravillas", 2));
            feelingWords.Add(new FeelingWord("maravillosa", 3));
            feelingWords.Add(new FeelingWord("maravilloso", 3));
            feelingWords.Add(new FeelingWord("más", 3));
            feelingWords.Add(new FeelingWord("mejor", 2));
            feelingWords.Add(new FeelingWord("mejora", 2));
            feelingWords.Add(new FeelingWord("mejorado", 2));
            feelingWords.Add(new FeelingWord("mejorar", 2));
            feelingWords.Add(new FeelingWord("milagro", 2));
            feelingWords.Add(new FeelingWord("milagrosa", 2));
            feelingWords.Add(new FeelingWord("milagros", 2));
            feelingWords.Add(new FeelingWord("moderno", 2));
            feelingWords.Add(new FeelingWord("modesto", 2));
            feelingWords.Add(new FeelingWord("motivación", 2));
            feelingWords.Add(new FeelingWord("motivado", 3));
            feelingWords.Add(new FeelingWord("nacimiento", 1));
            feelingWords.Add(new FeelingWord("noble", 2));
            feelingWords.Add(new FeelingWord("notable", 1));
            feelingWords.Add(new FeelingWord("notablemente", 1));
            feelingWords.Add(new FeelingWord("novedad", 1));
            feelingWords.Add(new FeelingWord("nuevo", 1));
            feelingWords.Add(new FeelingWord("nutritiva", 1));
            feelingWords.Add(new FeelingWord("navegable", 1));
            feelingWords.Add(new FeelingWord("navegar", 1));
            feelingWords.Add(new FeelingWord("obediente", 1));
            feelingWords.Add(new FeelingWord("oportunidad", 1));
            feelingWords.Add(new FeelingWord("omnipotencia", 1));
            feelingWords.Add(new FeelingWord("oportuno", 1));
            feelingWords.Add(new FeelingWord("optimismo", 1));
            feelingWords.Add(new FeelingWord("óptima", 2));
            feelingWords.Add(new FeelingWord("optimista", 1));
            feelingWords.Add(new FeelingWord("optimizar", 1));
            feelingWords.Add(new FeelingWord("orden", 1));
            feelingWords.Add(new FeelingWord("ordenado", 1));
            feelingWords.Add(new FeelingWord("organización", 1));
            feelingWords.Add(new FeelingWord("orgullo", 2));
            feelingWords.Add(new FeelingWord("orgulloso", 3));
            feelingWords.Add(new FeelingWord("originalidad", 1));
            feelingWords.Add(new FeelingWord("paciente", 1));
            feelingWords.Add(new FeelingWord("pacífica", 1));
            feelingWords.Add(new FeelingWord("pacífico", 1));
            feelingWords.Add(new FeelingWord("participación", 1));
            feelingWords.Add(new FeelingWord("paz", 1));
            feelingWords.Add(new FeelingWord("positivo", 2));
            feelingWords.Add(new FeelingWord("pensamiento", 1));
            feelingWords.Add(new FeelingWord("perdón", 1));
            feelingWords.Add(new FeelingWord("perdonar", 1));
            feelingWords.Add(new FeelingWord("perfecto", 2));
            feelingWords.Add(new FeelingWord("permitir", 1));
            feelingWords.Add(new FeelingWord("perseverancia", 1));
            feelingWords.Add(new FeelingWord("pertenencia", 1));
            feelingWords.Add(new FeelingWord("pertenecer", 1));
            feelingWords.Add(new FeelingWord("placer", 1));
            feelingWords.Add(new FeelingWord("poder", 1));
            feelingWords.Add(new FeelingWord("poderoso", 1));
            feelingWords.Add(new FeelingWord("placeres", 1));
            feelingWords.Add(new FeelingWord("positivamente", 1));
            feelingWords.Add(new FeelingWord("positivo", 1));
            feelingWords.Add(new FeelingWord("precioso", 3));
            feelingWords.Add(new FeelingWord("precisamente", 1));
            feelingWords.Add(new FeelingWord("preferible", 1));
            feelingWords.Add(new FeelingWord("prefiere", 2));
            feelingWords.Add(new FeelingWord("preferido", 2));
            feelingWords.Add(new FeelingWord("premio", 2));
            feelingWords.Add(new FeelingWord("premios", 2));
            feelingWords.Add(new FeelingWord("prestigio", 2));
            feelingWords.Add(new FeelingWord("privilegio", 2));
            feelingWords.Add(new FeelingWord("productiva", 1));
            feelingWords.Add(new FeelingWord("productivo", 1));
            feelingWords.Add(new FeelingWord("progreso", 2));
            feelingWords.Add(new FeelingWord("promesa", 1));
            feelingWords.Add(new FeelingWord("prometedora", 2));
            feelingWords.Add(new FeelingWord("próspera", 1));
            feelingWords.Add(new FeelingWord("prosperidad", 1));
            feelingWords.Add(new FeelingWord("próspero", 1));
            feelingWords.Add(new FeelingWord("prudente", 1));
            feelingWords.Add(new FeelingWord("puntual", 1));
            feelingWords.Add(new FeelingWord("puntualidad", 1));
            feelingWords.Add(new FeelingWord("puntualmente", 1));
            feelingWords.Add(new FeelingWord("querido", 2));
            feelingWords.Add(new FeelingWord("rápidamente", 1));
            feelingWords.Add(new FeelingWord("rapidez", 1));
            feelingWords.Add(new FeelingWord("rápido", 1));
            feelingWords.Add(new FeelingWord("razón", 2));
            feelingWords.Add(new FeelingWord("razonable", 2));
            feelingWords.Add(new FeelingWord("radiante", 2));
            feelingWords.Add(new FeelingWord("realidad", 1));
            feelingWords.Add(new FeelingWord("realista", 1));
            feelingWords.Add(new FeelingWord("recomendable", 2));
            feelingWords.Add(new FeelingWord("recomendado", 2));
            feelingWords.Add(new FeelingWord("recomendar", 2));
            feelingWords.Add(new FeelingWord("recompensa", 2));
            feelingWords.Add(new FeelingWord("reconciliación", 1));
            feelingWords.Add(new FeelingWord("reconfortante", 3));
            feelingWords.Add(new FeelingWord("reconocido", 1));
            feelingWords.Add(new FeelingWord("recomendación", 2));
            feelingWords.Add(new FeelingWord("reconocimiento", 1));
            feelingWords.Add(new FeelingWord("récord", 1));
            feelingWords.Add(new FeelingWord("reconocimientos", 1));
            feelingWords.Add(new FeelingWord("recuperar", 1));
            feelingWords.Add(new FeelingWord("reembolso", 1));
            feelingWords.Add(new FeelingWord("reestructurado", 1));
            feelingWords.Add(new FeelingWord("reestructuración", 1));
            feelingWords.Add(new FeelingWord("refugio", 1));
            feelingWords.Add(new FeelingWord("regalo", 1));
            feelingWords.Add(new FeelingWord("rejuvenecido", 1));
            feelingWords.Add(new FeelingWord("relación", 1));
            feelingWords.Add(new FeelingWord("relajado", 2));
            feelingWords.Add(new FeelingWord("relájese", 2));
            feelingWords.Add(new FeelingWord("relajante", 2));
            feelingWords.Add(new FeelingWord("reluciente", 2));
            feelingWords.Add(new FeelingWord("remunerado", 2));
            feelingWords.Add(new FeelingWord("remunerar", 2));
            feelingWords.Add(new FeelingWord("renovar", 1));
            feelingWords.Add(new FeelingWord("reputación", 2));
            feelingWords.Add(new FeelingWord("respeto", 2));
            feelingWords.Add(new FeelingWord("respetable", 2));
            feelingWords.Add(new FeelingWord("respetuoso", 2));
            feelingWords.Add(new FeelingWord("respetuosamente", 2));
            feelingWords.Add(new FeelingWord("respiro", 1));
            feelingWords.Add(new FeelingWord("responsabilidad", 1));
            feelingWords.Add(new FeelingWord("revolucionará", 1));
            feelingWords.Add(new FeelingWord("rindas", -1));
            feelingWords.Add(new FeelingWord("risa", 2));
            feelingWords.Add(new FeelingWord("romántico", 3));
            feelingWords.Add(new FeelingWord("riqueza", 1));
            feelingWords.Add(new FeelingWord("saber", 2));
            feelingWords.Add(new FeelingWord("sabroso", 2));
            feelingWords.Add(new FeelingWord("saborear", 2));
            feelingWords.Add(new FeelingWord("salud", 2));
            feelingWords.Add(new FeelingWord("sano", 2));
            feelingWords.Add(new FeelingWord("satisface", 2));
            feelingWords.Add(new FeelingWord("satisfacer", 2));
            feelingWords.Add(new FeelingWord("satisfactoria", 2));
            feelingWords.Add(new FeelingWord("satisfecho", 2));
            feelingWords.Add(new FeelingWord("segura", 2));
            feelingWords.Add(new FeelingWord("seguridad", 2));
            feelingWords.Add(new FeelingWord("seguro", 1));
            feelingWords.Add(new FeelingWord("sensacional", 2));
            feelingWords.Add(new FeelingWord("satisfactoriamente", 2));
            feelingWords.Add(new FeelingWord("servir", 2));
            feelingWords.Add(new FeelingWord("sincero", 2));
            feelingWords.Add(new FeelingWord("sinceramente", 2));
            feelingWords.Add(new FeelingWord("sobresalientemente", 2));
            feelingWords.Add(new FeelingWord("sobreviviente", 1));
            feelingWords.Add(new FeelingWord("solidaridad", 2));
            feelingWords.Add(new FeelingWord("solidez", 1));
            feelingWords.Add(new FeelingWord("soñador", 2));
            feelingWords.Add(new FeelingWord("sonriendo", 2));
            feelingWords.Add(new FeelingWord("sonriente", 2));
            feelingWords.Add(new FeelingWord("sonrisa", 2));
            feelingWords.Add(new FeelingWord("sonrisas", 2));
            feelingWords.Add(new FeelingWord("sorprendente", 2));
            feelingWords.Add(new FeelingWord("sueña", 2));
            feelingWords.Add(new FeelingWord("suerte", 1));
            feelingWords.Add(new FeelingWord("suficiente", 2));
            feelingWords.Add(new FeelingWord("suficientemente", 2));
            feelingWords.Add(new FeelingWord("superado", 2));
            feelingWords.Add(new FeelingWord("superadas", 2));
            feelingWords.Add(new FeelingWord("superar", 2));
            feelingWords.Add(new FeelingWord("superó", 2));
            feelingWords.Add(new FeelingWord("superando", 2));
            feelingWords.Add(new FeelingWord("terapia", 1));
            feelingWords.Add(new FeelingWord("tesoro", 2));
            feelingWords.Add(new FeelingWord("tolerancia", 1));
            feelingWords.Add(new FeelingWord("trabajador", 2));
            feelingWords.Add(new FeelingWord("trabajo", 2));
            feelingWords.Add(new FeelingWord("tradición", 1));
            feelingWords.Add(new FeelingWord("tranquila", 1));
            feelingWords.Add(new FeelingWord("tranquilidad", 1));
            feelingWords.Add(new FeelingWord("transparente", 1));
            feelingWords.Add(new FeelingWord("trascendental", 1));
            feelingWords.Add(new FeelingWord("tremendamente", 2));
            feelingWords.Add(new FeelingWord("triunfal", 2));
            feelingWords.Add(new FeelingWord("triunfante", 2));
            feelingWords.Add(new FeelingWord("triunfo", 2));
            feelingWords.Add(new FeelingWord("trofeo", 2));
            feelingWords.Add(new FeelingWord("útil", 1));
            feelingWords.Add(new FeelingWord("útiles", 1));
            feelingWords.Add(new FeelingWord("valentía", 1));
            feelingWords.Add(new FeelingWord("válido", 1));
            feelingWords.Add(new FeelingWord("valiente", 2));
            feelingWords.Add(new FeelingWord("valientemente", 2));
            feelingWords.Add(new FeelingWord("valioso", 2));
            feelingWords.Add(new FeelingWord("valor", 2));
            feelingWords.Add(new FeelingWord("valores", 1));
            feelingWords.Add(new FeelingWord("venerar", 1));
            feelingWords.Add(new FeelingWord("ventaja", 2));
            feelingWords.Add(new FeelingWord("ventajas", 2));
            feelingWords.Add(new FeelingWord("ventajoso", 3));
            feelingWords.Add(new FeelingWord("verdad", 2));
            feelingWords.Add(new FeelingWord("veraz", 1));
            feelingWords.Add(new FeelingWord("victoria", 2));
            feelingWords.Add(new FeelingWord("victorioso", 2));
            feelingWords.Add(new FeelingWord("vivo", 1));
            feelingWords.Add(new FeelingWord("voluntad", 1));
            feelingWords.Add(new FeelingWord("voluntariamente", 1));
            feelingWords.Add(new FeelingWord("vivir", 1));
            //// Add negative words to feeling words list
            feelingWords.Add(new FeelingWord("malo", -2));
            feelingWords.Add(new FeelingWord("horrible", -3));
            feelingWords.Add(new FeelingWord("no", -2));
            feelingWords.Add(new FeelingWord("triste", -3));
            feelingWords.Add(new FeelingWord("enojado", -3));
            feelingWords.Add(new FeelingWord("hambre", -1));
            feelingWords.Add(new FeelingWord("perdió", -2));
            feelingWords.Add(new FeelingWord("menos", -2));
            feelingWords.Add(new FeelingWord("ninguna", -3));
            feelingWords.Add(new FeelingWord("nadie", -2));
            feelingWords.Add(new FeelingWord("nada", -1));
            feelingWords.Add(new FeelingWord("ninguno", -1));
            feelingWords.Add(new FeelingWord("ningún", -2));
            feelingWords.Add(new FeelingWord("nunca", -2));
            feelingWords.Add(new FeelingWord("jamás", -2));
            feelingWords.Add(new FeelingWord("tampoco", -2));
            feelingWords.Add(new FeelingWord("ni", -1));
        }

        protected void insertIntoEnhancers()
        {
            enhancers.Add(new Enhancer("extremadamente", 2));
            enhancers.Add(new Enhancer("tan", 2));
            enhancers.Add(new Enhancer("demasiado", 2));
            enhancers.Add(new Enhancer("muy", 2));
            enhancers.Add(new Enhancer("bastante", 2));
            enhancers.Add(new Enhancer("aumenta", 1));
            enhancers.Add(new Enhancer("más", 1));
            enhancers.Add(new Enhancer("todo", 1));
            enhancers.Add(new Enhancer("mucho", 2));
            enhancers.Add(new Enhancer("suficientemente", 1));
            enhancers.Add(new Enhancer("algo", 1));
            enhancers.Add(new Enhancer("mejor", 2));
            enhancers.Add(new Enhancer("mejores", 3));
            enhancers.Add(new Enhancer("mucho", 2));
            enhancers.Add(new Enhancer("gran", 2));
            enhancers.Add(new Enhancer("menos", -1));
            enhancers.Add(new Enhancer("ligeramente", -1));
            enhancers.Add(new Enhancer("poco", -1));
            enhancers.Add(new Enhancer("disminuye", -1));
            enhancers.Add(new Enhancer("nunca", -1));
            enhancers.Add(new Enhancer("tampoco", -1));
            enhancers.Add(new Enhancer("contra", -1));
            enhancers.Add(new Enhancer("poco", -1));
            enhancers.Add(new Enhancer("menos", -2));
            enhancers.Add(new Enhancer("casi", -1));
            enhancers.Add(new Enhancer("peor", -3));
            enhancers.Add(new Enhancer("pores", -3));
            enhancers.Add(new Enhancer("mal", -2));
            enhancers.Add(new Enhancer("malo", -3));
            enhancers.Add(new Enhancer("no", -2));
        }

        protected bool existInStopWords(string word)
        {
            foreach (string sw in stopWords)
            {
                if (sw.Equals(word))
                {
                    return true;
                }
            }

            return false;
        }

        protected List<string> removeStopWords()
        {
            string[] words = this.text.ToLower().Replace(",", "").Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Split(' ');
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

        protected FeelingWord existFeelingWord(string word)
        {
            foreach (FeelingWord fw in feelingWords)
            {
                if (fw.word.Equals(word))
                {
                    return fw;
                }
            }

            return null;
        }

        protected Emoji existEmoji(string emoji)
        {
            foreach (Emoji e in emojis)
            {
                if (e.emoji.Equals(emoji))
                {
                    return e;
                }
            }

            return null;
        }

        protected FeelingWord getNewScore(Enhancer e, FeelingWord fw)
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

        protected FeelingWord validateWithEnhancers(string wordBefore, FeelingWord feelingWord)
        {
            foreach (Enhancer e in enhancers)
            {
                if (e.word.Equals(wordBefore))
                {
                    return getNewScore(e, feelingWord);
                }
            }

            return feelingWord;
        }

        public string showScores()
        {
            string result = "---> Palabras y emojis encontrados";

            foreach (FeelingWord fw in feelingWordsList)
            {
                result += "\n" + fw.word + " -> " + fw.score;
            }

            foreach (Emoji e in emojisList)
            {
                result += "\n" + e.emoji + " -> " + e.score;
            }

            return result;
        }

        protected int makeMeasurement()
        {
            int measurement = 0;

            foreach (FeelingWord fw in feelingWordsList)
            {
                measurement += (int)fw.score;
            }

            foreach (Emoji e in emojisList)
            {
                measurement += (int)e.score;
            }

            return measurement;
        }

        protected string giveResult(int result)
        {
            string res = "---> Medicion";

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

            //resultBox.Text = resultBox.Text + res;
            return res + "\n\n";
        }

        public string giveProbabilities()
        {
            string[] words = this.text.ToLower().Replace(",", "").Replace(".", "").Replace("!", "").Replace("¡", "").Replace("¿", "").Replace("?", "").Split(' ');
            float totalWords = words.Count(),
                  totalPositives = 0,
                  totalNegatives = 0,
                  scorePositive = 0,
                  scoreNegative = 0;
            string result = "";

            foreach (FeelingWord fw in this.feelingWordsList)
            {
                if (fw.score > 0)
                {
                    totalPositives++;
                    scorePositive += (float)fw.score;
                }
                else
                {
                    totalNegatives++;
                    scoreNegative -= (float)fw.score;
                }
            }

            foreach (Emoji e in this.emojisList)
            {
                if (e.score > 0)
                {
                    totalPositives++;
                    scorePositive += (float)e.score;
                }
                else
                {
                    totalNegatives++;
                    scoreNegative -= (float)e.score;
                }
            }

            result += "\n\n---> Estadísticas";
            result += "\nTotal de palabras: " + totalWords.ToString();
            result += "\nTotal de palabras y emojis positivos: " + totalPositives + " -> " + (totalPositives / totalWords) * 100 + "%";
            result += "\nTotal de palabras y emojis negativos: " + totalNegatives + " -> " + (totalNegatives / totalWords) * 100 + "%";
            result += "\nTotal de palabras desconocidas: " + (totalWords - (totalPositives + totalNegatives)) + " -> " + ((totalWords - (totalPositives + totalNegatives)) / totalWords) * 100 + "%";
            result += "\nEl texto es positivo en un: " + (totalPositives / (totalPositives + totalNegatives)) * 100 + "%";
            result += "\nEl texto es negativo en un: " + (totalNegatives / (totalPositives + totalNegatives)) * 100 + "%";

            this.porcentajes = new float[3] { (totalPositives / (totalPositives + totalNegatives)) * 100, (totalNegatives / (totalPositives + totalNegatives)) * 100, (totalWords - (totalPositives + totalNegatives)) };
            this.scores = new float[2] { scorePositive, scoreNegative };
            return result;
        }

        public string sentimentAnalysis()
        {
            string pieceBefore = "";
            List<string> content = removeStopWords();
            FeelingWord fw;
            Emoji e;

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

            //showScores();
            return giveResult(makeMeasurement());
            //giveProbabilities();
        }
    }
}