using CosmixERP.AccessLayer.DataAccess.Entities;
using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmixERP.AccessLayer.Services
{
    public class InvoicesServices: AccessDbContext
    {
        public ILogger log;
        public InvoicesServices()
        {
            
        }

        public async Task<List<Receipts>> GetUnInvoicedReceipts()
        {
            var receipts = await connection.QueryAsync<Receipts>("select * from TblCtr_SaldosVentas");

            return receipts.ToList();
        }

        public async Task<List<ReceiptsProducts>> GetInvoiceDetail(int invoice)
        {
            try
            {                
                var sql = $"select a.Id, a.Producto, a.Cantidad, a.Fecha, a.Saldo, 1 as Split, b.Id as ProductId, b.Parte, b.Precio1, b.Precio2, b.UnidadSat, b.ClaveSat " +
                        $"from TblCtrl_KardexVentas a " +
                        $"inner join TblCat_Productos b on a.Producto = b.Id " +
                        $"where a.Saldo={invoice}";
                var detail = await connection.QueryAsync<ReceiptsProducts, Products, ReceiptsProducts>(sql, (receiptsProducts, product) =>
                {
                    receiptsProducts.Product = product;
                    return receiptsProducts;
                }, splitOn: "Split");

                return detail.ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);                
            }
            return null;
        }
    }
}
