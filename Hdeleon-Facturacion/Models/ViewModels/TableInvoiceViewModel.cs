using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class TableInvoiceViewModel
    {
        public int Id { get; set; }
        public string UUID { get; set; }
        public decimal Total { get; set; }
        public string Serie { get; set; }
        public int Folio { get; set; }
        public string RFCReceptor { get; set; }
        public DateTime DateCreated { get; set; }
        public string sDateCreated
        {
            get
            {
                return DateCreated.ToString("dd-MM-yyyy HH:mm:ss");
            }
        }
        public int IdStatusInvoice { get; set; }
        public string Status
        {
            get
            {
                if (IdStatusInvoice == 1)
                {
                    return "Vigente";
                }else
                {
                    return "Cancelada";
                }
            }
        }
    }
}