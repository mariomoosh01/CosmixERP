using Hdeleon_Facturacion.Business.Cancel;
using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class FiscalFilesController : BaseController
    {
        Models.Entities db;
        public FiscalFilesController()
        {
            db = new Models.Entities();
        }

        // GET: FiscalFiles
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            return View();
        }

        [HttpPost]
        public ActionResult Save(FiscalFilesViewModel model)
        {
            try
            {
                GetSession();
                string PathCer = Path.Combine(Server.MapPath("~/"), Business.Constants.PathCer);
                string PathKey = Path.Combine(Server.MapPath("~/"), Business.Constants.PathKey);
                string PathPfx = Path.Combine(Server.MapPath("~/"), Business.Constants.PathPfx);

                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }

                model.Cer.SaveAs(PathCer);
                model.Key.SaveAs(PathKey);

                //generamos el pfx
                PFX oPFX = new PFX(PathCer, PathKey, model.PrivateCode, PathPfx, Server.MapPath("~/Files/"));
                if (!oPFX.Create())
                    throw new Exception(oPFX.error);

                // guardamos
                var oUser = db.user.Find(User.id);
                oUser.PrivateCode = model.PrivateCode;
                db.Entry(oUser).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                

                @TempData["Message"] = "Archivos y clave privada guardados con éxito";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error de sistema: " + ex.Message;
                return View("Index", model);
            }
        }

    }
}