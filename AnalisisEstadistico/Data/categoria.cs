//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnalisisEstadistico.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class categoria
    {
        public categoria()
        {
            this.palabras = new HashSet<palabra>();
        }
    
        public int id { get; set; }
        public string nombreCategoria { get; set; }
    
        public virtual ICollection<palabra> palabras { get; set; }
    }
}