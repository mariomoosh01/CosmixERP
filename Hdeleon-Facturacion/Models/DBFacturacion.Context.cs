﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hdeleon_Facturacion.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=facturacionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<cmunicipio> cmunicipio { get; set; }
        public DbSet<cstate> cstate { get; set; }
        public DbSet<cstate_invoice> cstate_invoice { get; set; }
        public DbSet<ctype_invoice> ctype_invoice { get; set; }
        public DbSet<customer> customer { get; set; }
        public DbSet<destado> destado { get; set; }
        public DbSet<invoice> invoice { get; set; }
        public DbSet<product> product { get; set; }
        public DbSet<product_tax> product_tax { get; set; }
        public DbSet<tax> tax { get; set; }
        public DbSet<user> user { get; set; }
    }
}