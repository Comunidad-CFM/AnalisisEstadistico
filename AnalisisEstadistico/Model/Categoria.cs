using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalisisEstadistico.Model
{
    public class Categoria
    {
        public Categoria()
        {
            nombreCategoria = "";
            porcentaje = 0.0;
            palabrasEnCategoria = new List<string>();
            palabrasAgregarCategoria = new List<string>();
            idioma = "";

        }

        private string nombreCategoria;

        public string NombreCategoria
        {
            get { return nombreCategoria; }
            set { nombreCategoria = value; }
        }
        private double porcentaje;

        public double Porcentaje
        {
            get { return porcentaje; }
            set { porcentaje = value; }
        }
        private List<string> palabrasEnCategoria;

        public List<string> PalabrasEnCategoria
        {
            get { return palabrasEnCategoria; }
            set { palabrasEnCategoria = value; }
        }

        private List<string> palabrasAgregarCategoria;

        public List<string> PalabrasAgregarCategoria
        {
            get { return palabrasAgregarCategoria; }
            set { palabrasAgregarCategoria = value; }
        }

        private string idioma;

        public string Idioma
        {
            get { return idioma; }
            set { idioma = value; }
        }
    }
}