using Hdeleon_Facturacion.Models;
using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class CustomerController : BaseCrudController<Models.ViewModels.TableCustomerViewModel>
    {
        Models.Entities db;
        Models.SAT.satEntities db2;
        
        public CustomerController()
        {
             db= new Models.Entities();
             db2 = new Models.SAT.satEntities();
        }
        // GET: Customer
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
                    IQueryable<Models.ViewModels.TableCustomerViewModel> lst = from d in db.customer
                                                                               where d.idState==1
                                                                              orderby d.id descending
                                                                              select new Models.ViewModels.TableCustomerViewModel
                                                                              {
                                                                                  Id = d.id,
                                                                                  Name = d.name,
                                                                                  CommercialName = d.comercial_name,
                                                                                  RFC= d.RFC
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
        public ActionResult Add(Models.ViewModels.CustomerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetComponentData();
                    return View(model);
                }

                using (Entities db = new Entities())
                {
                    customer customer = new customer();
                    customer.idState = 1;
                    customer.create_date = DateTime.Now;
                    customer.name = model.Name;
                    customer.comercial_name = model.CommercialName;
                    customer.CP = model.CP;
                    customer.email = model.Email;
                    customer.Street = model.Street;
                    customer.NoExt = model.NoExt;
                    customer.NoInt = model.NoInt;
                    customer.idEstado = model.idEstado;
                    customer.idMunicipio = model.IdMunicipio;
                    customer.RFC = model.RFC;
                    customer.CodeTaxRegime = model.CodeTaxRegime;
                    db.customer.Add(customer);
                    db.SaveChanges();
                }
                @TempData["Message"] = "Cliente agregado con éxito";
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
            CustomerViewModel model = new CustomerViewModel();
            using (Entities db = new Entities())
            {
               
                var customer = db.customer.Find(Id);
                model.Id = Id;
                model.Name = customer.name;
                model.CommercialName = customer.comercial_name;
                model.RFC = customer.RFC;
                model.Street = customer.Street;
                model.NoExt = customer.NoExt;
                model.NoInt = customer.NoInt;
                model.CP = customer.CP;
                model.IdMunicipio = customer.idMunicipio;
                model.idEstado = customer.idEstado;
                model.Email = customer.email;
                model.CodeTaxRegime = customer.CodeTaxRegime;
                GetComponentDataEdit(model);


            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(CustomerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetComponentDataEdit(model);
                    return View(model);
                }

                using (Entities db = new Entities())
                {
                    var customer = db.customer.Find(model.Id);
                    customer.name = model.Name;
                    customer.comercial_name = model.CommercialName;
                    customer.RFC = model.RFC;
                    customer.Street = model.Street;
                    customer.NoExt = model.NoExt;
                    customer.NoInt = model.NoInt;
                    customer.CP = model.CP;
                    customer.idMunicipio= model.IdMunicipio;
                    customer.idEstado = model.idEstado;
                    customer.email =model.Email;
                    customer.CodeTaxRegime = model.CodeTaxRegime;
                    db.Entry(customer).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }

                TempData["Message"] = "Cliente modificado con éxito";
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
                using (Entities db = new Entities())
                {
                    customer customer = db.customer.Find(Id);
                    customer.idState = 3;
                    db.Entry(customer).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("Error de sistema: " + ex.Message);
            }
        }

        #region HELPERS
        /// <summary>
        /// obtiene la información de componentes
        /// </summary>
        private void GetComponentData()
        {
            var lstEstados = (from d in db.destado
                                  orderby d.Nombre
                                  select d).ToList();

            List<SelectListItem> slcEstados = lstEstados.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Nombre.ToString(),
                    Value = d.iddEstado.ToString(),
                    Selected = false
                };
            });

            ViewBag.slcEstados = slcEstados;

            ViewBag.slcCodeTaxRegime = 
                new SelectList((from d in db2.f4_c_regimenfiscal
                                select new { code = d.c_RegimenFiscal, name = d.Descripcion }),
                                "code",
                                "name");

        }

        private void GetComponentDataEdit(CustomerViewModel model)
        {
            GetComponentData();
            var lstMunicipios = (from d in db.cmunicipio
                                 where d.iddEstado == model.idEstado
                              orderby d.Municipio
                              select d).ToList();

            List<SelectListItem> slcMunicipios = lstMunicipios.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Municipio.ToString(),
                    Value = d.idcMunicipio.ToString(),
                    Selected = true
                    
                };
            });

            ViewBag.slcMunicipios = slcMunicipios;

        }
        #endregion
    }
}