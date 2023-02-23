using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class TaxController : BaseCrudController<Models.ViewModels.TableTaxViewModel>
    {
        Models.Entities db;
        public TaxController()
        {
            db = new Models.Entities();
        }

        // GET: Tax
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            return View();
        }

        public ActionResult Json()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    IQueryable<Models.ViewModels.TableTaxViewModel> lst = from d in db.tax
                                                                               where d.idState == 1
                                                                               orderby d.id descending
                                                                               select new Models.ViewModels.TableTaxViewModel
                                                                               {
                                                                                   Id = d.id,
                                                                                   Name = d.name,
                                                                                   Type = d.type,
                                                                                   Tasa= d.tasa,
                                                                                   TipoFactor=d.tipo_factor
                                                                               };
                    //organizamos la tabla
                    GetParamsTable(lst);

                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public ActionResult Add()
        {
            GetComponentData();
            return View();
        }

        [HttpPost]
        public ActionResult Add(Models.ViewModels.TaxViewModel model)
        {
            try
            {
                GetSession();
                if (!ModelState.IsValid)
                {
                    GetComponentData();
                    return View(model);
                }


                Models.tax oTax = new Models.tax();
                oTax.date_create = DateTime.Now;
                oTax.idState = 1;
                oTax.name = model.Name;
                oTax.type_ambit = "FEDERAL";
                oTax.type = model.Type;
                oTax.tasa = model.Tasa;
                oTax.claveImpuesto = model.Type_Tax;
                oTax.tipo_factor = model.Factor;
                oTax.idUser = User.id;
                db.tax.Add(oTax);
                db.SaveChanges();
                
                @TempData["Message"] = "Impuesto agregado con éxito";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error de sistema: " + ex.Message;
                return View(model);
            }
        }


        public ActionResult Edit(int Id)
        {
            TaxViewModel model = new TaxViewModel();
            GetComponentData();

            var oTax = db.tax.Find(Id);
            model.Id = Id;
            model.Name = oTax.name;
            model.Factor = oTax.tipo_factor;
            model.Tasa = oTax.tasa;
            model.Type_Tax = oTax.claveImpuesto;
            model.Type = oTax.type;
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(TaxViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetComponentData();
                    return View(model);
                }


                var oTax = db.tax.Find(model.Id);
                oTax.name = model.Name;
                oTax.type = model.Type;
                oTax.claveImpuesto = model.Type_Tax;
                oTax.tasa = model.Tasa;
                oTax.tipo_factor = model.Factor;
                db.Entry(oTax).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                

                TempData["Message"] = "Impuesto modificado con éxito";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error de sistema: " + ex.Message;
                return View(model);
            }
        }

        public ActionResult Delete(int Id)
        {
            try
            {

                 Models.tax oTax= db.tax.Find(Id);
                oTax.idState = 3;
                db.Entry(oTax).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("Error de sistema: " + ex.Message);
            }
        }



        #region HELPERS
        private void GetComponentData()
        {
            List<SelectListItem> lstTax = new List<SelectListItem>();
            lstTax.Add(new SelectListItem() { Text = "ISR", Value = "001" });
            lstTax.Add(new SelectListItem() { Text = "IVA", Value = "002" });
            lstTax.Add(new SelectListItem() { Text = "IEPS", Value = "003" });
            ViewBag.slcTax = new SelectList(lstTax, "Value", "Text");

            List<SelectListItem> lstType = new List<SelectListItem>();
            lstType.Add(new SelectListItem() { Text = "Traslado", Value = "Traslado" });
            lstType.Add(new SelectListItem() { Text = "Retención", Value = "Retención" });
            ViewBag.slcType = new SelectList(lstType, "Value", "Text");

            List<SelectListItem> lstFactor = new List<SelectListItem>();
            lstFactor.Add(new SelectListItem() { Text = "Tasa", Value = "Tasa" });
            lstFactor.Add(new SelectListItem() { Text = "Cuota", Value = "Cuota" });
            lstFactor.Add(new SelectListItem() { Text = "Exento", Value = "Exento" });
            ViewBag.slcFactor = new SelectList(lstFactor, "Value", "Text");

        }
        #endregion

    }
}