using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class CancelInvoiceViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Rfc del receptor")]
        public string RFCReceptor { get; set; }
        public decimal Total { get; set; }

        public string UUID{get;set;}
        [Required]
        public string Motivo { get; set; }
        [Display(Name ="Sustitución")]
        public string Sustitucion { get; set; }
    }
}