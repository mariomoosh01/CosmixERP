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
    
    public partial class invoice
    {
        public int id { get; set; }
        public string UUID { get; set; }
        public string RFCReceptor { get; set; }
        public decimal Total { get; set; }
        public string Serie { get; set; }
        public int Folio { get; set; }
        public Nullable<int> idUser { get; set; }
        public Nullable<int> idType { get; set; }
        public System.DateTime date_create { get; set; }
        public Nullable<int> idStateInvoice { get; set; }
        public string xml { get; set; }
    
        public virtual ctype_invoice ctype_invoice { get; set; }
        public virtual user user { get; set; }
    }
}
