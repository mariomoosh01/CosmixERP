using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class LogoController : BaseController
    {
        Models.Entities db;
        public LogoController()
        {
            db = new Models.Entities();
        }
        
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            return View();
        }
        [HttpPost]
        public ActionResult Save(LogoViewModel model)
        {
            try
            {
                GetSession();
                string PathLogo = Path.Combine(Server.MapPath("~/"), Business.Constants.PathLogo);

                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }

                model.Logo.SaveAs(PathLogo);

                @TempData["Message"] = "Logotipo guardado con éxito";
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