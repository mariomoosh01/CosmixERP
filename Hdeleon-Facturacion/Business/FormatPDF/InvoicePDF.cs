using Hdeleon_Facturacion.Controllers;
using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Hdeleon_Facturacion.Business.FormatPDF
{
    public class InvoicePDF : Format
    {
        InvoiceViewModel oFacturaVM;
        public InvoicePDF(string root, InvoiceViewModel oFacturaVM) : base(root)
        {
            this.oFacturaVM = oFacturaVM;
        }

        public override void Create()
        {
                PDFController controller = new PDFController();
                RouteData route = new RouteData();
                route.Values.Add("action", metodo); // ActionName
                route.Values.Add("controller", nameController); // Controller Name
                System.Web.Mvc.ControllerContext newContext = new System.Web.Mvc.ControllerContext(new HttpContextWrapper(System.Web.HttpContext.Current), route, controller);
                controller.ControllerContext = newContext;
                controller.InvoicePDF(root, oFacturaVM);

                
        }
    }
}