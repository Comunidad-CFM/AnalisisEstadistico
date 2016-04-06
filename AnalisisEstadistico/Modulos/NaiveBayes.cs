using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnalisisEstadistico.Data;
using AnalisisEstadistico.Model;
using AnalisisEstadistico.Modulos;

namespace AnalisisEstadistico.Modulos
{
    /// <summary>
    /// Clase para realizar el análisis de categorias
    /// </summary>
    public class NaiveBayes
    {
        public int idioma = 0;
        public string nombreIdioma = "";

        List<idioma> idiomas;
        List<categoria> categorias;
        List<palabra> palabras;

        int porcentajeDeSimilitud = 0;
        public NaiveBayes()
        {
            idiomas = new List<idioma>();
            categorias = new List<categoria>();
            palabras = new List<palabra>();

            using (var db = new AnalizadorBDEntities())
            {

                var queryIdiomas = from i in db.idiomas select i;
                var queryCategorias = from c in db.categorias select c;
                var queryPalabras = from p in db.palabras select p;

                foreach (var varIdioma in queryIdiomas)
                {
                    idiomas.Add(varIdioma);
                }

                foreach (var varCategoria in queryCategorias)
                {
                    categorias.Add(varCategoria);
                }

                foreach (var varPalabra in queryPalabras)
                {
                    palabras.Add(varPalabra);
                }
            }
        }

        public List<string> dividirTexto(string texto)
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            List<string> words = new List<string>(texto.Split(delimiterChars));
            return words;
        }

        public List<Categoria> clasificar(List<string> texto)
        {
            if (idioma != 0)
            { // Si se sabe cual es el idioma

                List<string> palabrasEnCategoria = new List<string>();
                List<string> palabrasAgregarCategoria = new List<string>();

                List<Categoria> resultado = new List<Categoria>();

                Categoria categoria = new Categoria(); // categoria Model
                double prob = 1 / 6d; // prob. inicial: prob. de categoria
                bool encontro = false;

                foreach (categoria cat in categorias) // categorias Data
                {

                    int palabrasTotalesDeCategoria = getPalabrasTotalesEnCategoria(cat.id);
                    categoria.NombreCategoria = cat.nombreCategoria;
                    double cont = 0;

                    foreach (string pal in texto) // palabras Data
                    {

                        int palabraEnCategoria = getAparicionEnCategoria(pal.ToLower(), cat.id) ? 1 : 0; // Si la palabra esta en la categoria dada

                        if (palabraEnCategoria == 1) ////////////////////////////////////////////////////////////////////////////
                        {
                            encontro = true;
                            bool existePalabra = addPalabraLista(palabrasEnCategoria, pal.ToLower().ToString());
                            //palabrasEnCategoria.Add(pal.ToString());

                            if (existePalabra) // Si existe la palabra el la lista
                                cont += 0.1d;
                            else
                                cont += 1;
                        }
                        else
                        {
                            //Console.WriteLine("{0}%", source.ToLower().CalculateSimilarity(target.ToLower()) * 100);
                            if (porcentajeDeSimilitud > 60 && pal != "")
                            {
                                bool existePalabra = addPalabraLista(palabrasAgregarCategoria, pal.ToString());
                                porcentajeDeSimilitud = 0;
                            }
                        }
                    }

                    if (encontro)
                    {
                        int num = getPalabrasTotalesEnCategoria(cat.id);
                        prob = (num / System.Convert.ToDouble(palabras.Count)) * ((cont / num) * 100);
                        categoria.Porcentaje = Math.Round((prob * 100), 2);
                    }
                    else
                        categoria.Porcentaje = 0.0;

                    encontro = false;

                    prob = 0.0;
                    categoria.PalabrasEnCategoria = palabrasEnCategoria;
                    categoria.PalabrasAgregarCategoria = palabrasAgregarCategoria;
                    categoria.Idioma = nombreIdioma;
                    palabrasEnCategoria = null;
                    palabrasEnCategoria = new List<string>();
                    palabrasAgregarCategoria = null;
                    palabrasAgregarCategoria = new List<string>();

                    resultado.Add(categoria);
                    categoria = null;
                    categoria = new Categoria();

                }
                return resultado;

            }
            return clasificarSinIdioma(texto);
        }

        public List<Categoria> clasificarSinIdioma(List<string> texto)
        {
            List<string> palabrasEnCategoria = new List<string>();
            List<string> palabrasAgregarCategoria = new List<string>();

            List<Categoria> resultado = new List<Categoria>();

            Categoria categoria = new Categoria(); // categoria Model
            double prob = 1 / 6d; // prob. inicial: prob. de categoria
            bool encontro = false;
            foreach (idioma idi in idiomas) // idiomas Data
            {
                idioma = idi.id;
                foreach (categoria cat in categorias) // categorias Data
                {

                    int palabrasTotalesDeCategoria = getPalabrasTotalesEnCategoria(cat.id);
                    categoria.NombreCategoria = cat.nombreCategoria;
                    double cont = 0;

                    foreach (string pal in texto) // palabras Data
                    {

                        int palabraEnCategoria = getAparicionEnCategoria(pal.ToLower(), cat.id) ? 1 : 0; // Si la palabra esta en la categoria dada

                        if (palabraEnCategoria == 1) ////////////////////////////////////////////////////////////////////////////
                        {
                            encontro = true;
                            bool existePalabra = addPalabraLista(palabrasEnCategoria, pal.ToLower().ToString());
                            //palabrasEnCategoria.Add(pal.ToString());

                            if (existePalabra) // Si existe la palabra el la lista
                                cont += 0.1d;
                            else
                                cont += 1;
                        }
                        else
                        {
                            //Console.WriteLine("{0}%", source.ToLower().CalculateSimilarity(target.ToLower()) * 100);
                            if (porcentajeDeSimilitud > 60 && pal != "")
                            {
                                bool existePalabra = addPalabraLista(palabrasAgregarCategoria, pal.ToString());
                                porcentajeDeSimilitud = 0;
                            }
                        }
                    }

                    if (encontro)
                    {
                        int num = getPalabrasTotalesEnCategoria(cat.id);
                        prob = (num / System.Convert.ToDouble(palabras.Count)) * ((cont / num) * 100);
                        categoria.Porcentaje = Math.Round((prob * 100), 2);
                    }
                    else
                        categoria.Porcentaje = 0.0;

                    encontro = false;

                    prob = 0.0;
                    categoria.PalabrasEnCategoria = palabrasEnCategoria;
                    categoria.PalabrasAgregarCategoria = palabrasAgregarCategoria;
                    categoria.Idioma = idi.nombreIdioma;

                    palabrasEnCategoria = null;
                    palabrasEnCategoria = new List<string>();
                    palabrasAgregarCategoria = null;
                    palabrasAgregarCategoria = new List<string>();

                    resultado.Add(categoria);
                    categoria = null;
                    categoria = new Categoria();

                }
            }
            return resultado;
        }

        /// <summary>
        /// Retorna la suma de todas las palabras dada una categoria y el idioma
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns name="resultado"></returns>
        public int getPalabrasTotalesEnCategoria(int categoria)
        {
            int resultado = 0;

            foreach (palabra pal in palabras) // palabras de la bd
            {
                if (pal.idCategoria == categoria && pal.idIdioma == idioma)
                    resultado += 1;
            }

            return resultado;
        }

        /// <summary>
        /// Retorna el numero de veces en que la palabra dada aparece en todas las categorias
        /// </summary>
        /// <param name="palabra"></param>
        /// <returns name="resultado"></returns>
        public int getTotaleAparicionEnCategorias(string palabra)
        {
            int resultado = 0;

            foreach (palabra pal in palabras) // palabras Data
            {
                if (pal.valorPalabra == palabra && pal.idIdioma == idioma)
                    resultado += 1;
            }

            return resultado;
        }

        /// <summary>
        /// Retorna si existe la palabra dada en la categoria dada
        /// </summary>
        /// <param name="palabra"></param>
        /// <returns name="existe"></returns>
        public bool getAparicionEnCategoria(string palabra, int categoria)
        {
            bool existe = false;
            int porcentajeDeSimilitudTemp = 0;
            foreach (palabra pal in palabras) // palabras Data
            {
                porcentajeDeSimilitudTemp = (int)pal.valorPalabra.ToLower().CalculateSimilarity(palabra.ToLower()) * 100;
                if (porcentajeDeSimilitud < porcentajeDeSimilitudTemp)
                    porcentajeDeSimilitud = porcentajeDeSimilitudTemp;

                if (pal.valorPalabra == palabra && pal.idCategoria == categoria && pal.idIdioma == idioma)
                {
                    existe = true;
                    break;
                }
            }
            return existe;
        }

        /// <summary>
        /// Agrega la palabra dada a la lista dada, sin repeticiones. Retorna true si la palabra existe en la lista
        /// </summary>
        /// <param name="listaPalabras"></param>
        /// <param name="palabra"></param>
        /// <returns></returns>
        public bool addPalabraLista(List<string> listaPalabras, string palabra)
        {
            if (!listaPalabras.Contains(palabra))
            {
                listaPalabras.Add(palabra);
                return false;
            }

            return true;
        }
    }
}