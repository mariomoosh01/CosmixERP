using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class TableCustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CommercialName { get; set; }
        public string RFC { get; set; }
    }
}