using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class PasswordController : BaseController
    {
        Models.Entities db;
        public PasswordController()
        {
            db = new Models.Entities();
        }

        // GET: Password
        public ActionResult Index()
        {

            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            return View();
        }

        [HttpPost]
        public ActionResult Save(PasswordViewModel model)
        {
            try
            {
                GetSession();

                if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }

                var oUser = db.user.Find(User.id);
                oUser.password = Utils.Encrypt.GetSHA1(model.Password);
                db.Entry(oUser).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                //asignamos nueva sesion para actualizar datos
                Session["user"] = oUser;

                @TempData["Message"] = "Información guardada con éxito";
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