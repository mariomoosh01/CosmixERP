using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class CancelInvoiceController : BaseController
    {
        [HttpGet]
        public ActionResult Cancel(int Id)
        {
            CancelInvoiceViewModel model = 
                new CancelInvoiceViewModel(){ Id = Id };

            using (Models.Entities db = new Models.Entities())
            {
                var invoice = db.invoice.Find(Id);
                model.UUID = invoice.UUID;
                model.Total = invoice.Total;
                model.RFCReceptor = invoice.RFCReceptor;
                
            }

            GetComponentData();
            return View(model);
        }

        [HttpPost]
        public ActionResult Cancel(CancelInvoiceViewModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    GetComponentData();

                    return View(model);
                }

                GetSession();

                using (Models.Entities db = new Models.Entities())
                {
                    var oInvoice = db.invoice.Find(model.Id);
                    byte[] bPfx = System.IO.File.ReadAllBytes(Server.MapPath("~/") + Business.Constants.PathPfx);
                    Business.Cancel.CancelInvoice oCancelInvoice =
                        new Business.Cancel.CancelInvoice(User.UserPacFC, User.PassPacFC, oInvoice.UUID, User.PrivateCode, bPfx,
                        (double)oInvoice.Total, User.rfc, oInvoice.RFCReceptor, model.Motivo, model.Sustitucion);
                    oCancelInvoice.Cancel();

                    //editamos el registro en la base de datos
                    oInvoice.idStateInvoice = 2; //cancelar
                    db.Entry(oInvoice).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                }
                @TempData["Message"] = "Factura cancelada con éxito";
                return Redirect("~/Invoice/Index");
            }
            catch (Exception ex)
            {
                GetComponentData();
                ViewBag.Error = "Error de sistema: " + ex.Message;
                return View(model);

            }
        }

        #region HELPERS
        private void GetComponentData()
        {
          
            List<SelectListItem> slcMotivos = new List<SelectListItem>();
            slcMotivos.Add(new SelectListItem()
            {
                Text = "Comprobante emitido con errores con relación",
                Value = "01",
                Selected = false
            });
            slcMotivos.Add(new SelectListItem()
            {
                Text = "Comprobante emitido con errores sin relación",
                Value = "02",
                Selected = false
            });
            slcMotivos.Add(new SelectListItem()
            {
                Text = "No se llevó a cabo la operación",
                Value = "03",
                Selected = false
            });
            slcMotivos.Add(new SelectListItem()
            {
                Text = "Operación nominativa relacionada en una factura global",
                Value = "04",
                Selected = false
            });

            ViewBag.slcMotivos = slcMotivos;

        }

        #endregion
    }
}