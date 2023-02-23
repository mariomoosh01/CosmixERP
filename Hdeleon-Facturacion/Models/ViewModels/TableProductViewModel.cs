using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class TableProductViewModel
    {
        public int Id { get; set; }
        public string ClaveProdServ { get; set; }
        public string ClaveUnidad { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }

    }
}