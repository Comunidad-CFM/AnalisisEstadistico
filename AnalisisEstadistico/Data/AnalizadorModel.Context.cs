﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AnalizadorBDEntities1 : DbContext
    {
      public AnalizadorBDEntities1()
        : this(false) { }
    
        public AnalizadorBDEntities1(bool proxyCreationEnabled)	    
            : base("name=AnalizadorBDEntities1")
        {
            this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
    
        public AnalizadorBDEntities1(string connectionString)
          : this(connectionString, false) { }
    
        public AnalizadorBDEntities1(string connectionString, bool proxyCreationEnabled)
            : base(connectionString)
        {
            this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }	
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<emoji> emojis { get; set; }
        public DbSet<feelingWord> feelingWords { get; set; }
        public DbSet<stopword> stopwords { get; set; }
    }
}