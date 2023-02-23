using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class TaxViewModel
    {
        public int Id { get; set; }
        [DisplayName("Nombre")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Factor")]
        [Required]
        public string Factor { get; set; }
        [DisplayName("Tipo")]
        [Required]
        public string Type { get; set; }
        [DisplayName("Tasa")]
        [Required]
        public decimal Tasa { get; set; }
        [DisplayName("Impuesto")]
        [Required]
        public string Type_Tax { get; set; }
        
    }
}