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
    
    public partial class cmunicipio
    {
        public cmunicipio()
        {
            this.customer = new HashSet<customer>();
        }
    
        public int idcMunicipio { get; set; }
        public int iddEstado { get; set; }
        public string Clave { get; set; }
        public string Municipio { get; set; }
        public string CabeceraMunicipal { get; set; }
    
        public virtual destado destado { get; set; }
        public virtual ICollection<customer> customer { get; set; }
    }
}