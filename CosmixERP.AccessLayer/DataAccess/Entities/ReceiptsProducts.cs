using System;

namespace CosmixERP.AccessLayer.DataAccess.Entities
{
    public class ReceiptsProducts
    {
        public long Id { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public int Saldo { get; set; }        

        public Products Product { get; set;  }
    }
}
