using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class ShowInvoiceViewModel
    {
        public int Id { get; set; }
        public string UUID { get; set; }
        public string NameXML { get; set; }
        public string NamePdf
        {
            get
            {
                if(NameXML!=null)
                return NameXML.Replace(".xml", ".pdf");
                return "";
            }
        }
    }
}