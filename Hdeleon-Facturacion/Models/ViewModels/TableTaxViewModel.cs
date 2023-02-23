using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class TableTaxViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Tasa { get; set; }
        public string TipoFactor { get; set; }
        public string Type { get; set; }
    }
}