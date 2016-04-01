using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnalisisEstadistico.Model
{
    /// <summary>
    /// Clase para modelar las coincidencias de palabras en el texto
    /// apariciones: cantidad de veces que aparece en el código
    /// palabra: palabra correspondiente a la aparición
    /// </summary>
    public class Coincidencia
    {
        public int apariciones;
        public string palabra;
        public Coincidencia(string palabra) {
            this.palabra = palabra;
            this.apariciones = 1;
        }
    }
}