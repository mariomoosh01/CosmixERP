using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class FiscalDataController : BaseController
    {
        Models.Entities db;
        Models.SAT.satEntities dbSat;
        public FiscalDataController()
        {
            db = new Models.Entities();
            dbSat = new Models.SAT.satEntities();
        }

        public ActionResult Index()
        {
            GetSession();
            GetComponent();
            FiscalDataViewModel model = new FiscalDataViewModel();

            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            var oUser = db.user.Find(User.id);
            model.CodeTaxRegime = oUser.CodeTaxRegime;
            model.FiscalName = oUser.fiscal_name;
            model.RFC = oUser.rfc;
            model.Email = oUser.email;
            model.UserPacFC = oUser.UserPacFC;
            model.PassPacFC = oUser.PassPacFC;
            model.CP = oUser.cp;

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(FiscalDataViewModel model)
        {
            try
            {
                //GetSession();

                if (!ModelState.IsValid)
                {
                    return View("Index",model);
                }

                var oUser = db.user.Find(User.id);
                oUser.email = model.Email;
                oUser.CodeTaxRegime = model.CodeTaxRegime;
                oUser.rfc = model.RFC;
                oUser.fiscal_name = model.FiscalName;
                oUser.PassPacFC = model.PassPacFC;
                oUser.UserPacFC = model.UserPacFC;
                oUser.cp = model.CP;
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
                return View("Index",model);
            }
        }

        #region HELPERS
        private void GetComponent()
        {
            var lstTaxRegime = (from d in dbSat.f4_c_regimenfiscal
                              orderby d.Descripcion
                              select d).ToList();

            List<SelectListItem> slcTaxRegime = lstTaxRegime.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Descripcion.ToString(),
                    Value = d.c_RegimenFiscal.ToString(),
                    Selected = false
                };
            });

            ViewBag.slcTaxRegime = slcTaxRegime;

        }
        #endregion
    }
}