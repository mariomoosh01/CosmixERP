//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class product_tax
    {
        public int id { get; set; }
        public Nullable<int> idProduct { get; set; }
        public Nullable<int> idTax { get; set; }
    
        public virtual product product { get; set; }
        public virtual tax tax { get; set; }
    }
}
