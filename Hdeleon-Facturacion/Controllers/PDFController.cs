using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;

namespace Hdeleon_Facturacion.Controllers
{
    public class PDFController : Controller
    {
        public ActionResult InvoicePDF(string root, Models.ViewModels.InvoiceViewModel oModel)
        {

            var exito = new Rotativa.MVC.ViewAsPdf("Invoice", oModel)
            {
                SaveOnServerPath = root
            }.BuildPdf(this.ControllerContext);

            return Content("1");
        }
    }
}