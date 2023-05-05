using System;

namespace CosmixERP.AccessLayer.DataAccess.Entities
{
    public class ReceiptsProducts
    {
        private const decimal IVA = 1.16m;

        private decimal _precio;
        public long Id { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public int Saldo { get; set; }
        public decimal Precio {
            get => _precio;
            set => _precio = Math.Round(value / IVA, 2);
        }

        public Products Product { get; set;  }
    }
}
