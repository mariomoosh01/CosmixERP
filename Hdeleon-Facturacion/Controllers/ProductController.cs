using Hdeleon_Facturacion.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using Hdeleon_Facturacion.Models;

namespace Hdeleon_Facturacion.Controllers
{
    public class ProductController : BaseCrudController<Models.ViewModels.TableProductViewModel>
    {
        // GET: Product
        public ActionResult Index()
        {
            if(TempData["Message"]!=null)
            ViewBag.Message = TempData["Message"].ToString();

            return View();
        }

        public ActionResult Json()
        {
            try
            {
                using (Models.Entities db = new Entities())
                {
                    IQueryable<Models.ViewModels.TableProductViewModel> lst = from d in db.product
                                                                              where d.idState==1
                                                                              orderby d.id descending
                                                                              select new Models.ViewModels.TableProductViewModel
                                                                              {
                                                                                  Id = d.id,
                                                                                  ClaveProdServ= d.claveProdServ,
                                                                                  ClaveUnidad= d.claveUnidad,
                                                                                  Descripcion= d.Descripcion,
                                                                                  Precio = d.Precio
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
            return View();
        }

        [HttpPost]
        public ActionResult Add(ProductViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                using (Entities db = new Entities())
                {
                    product oProduct = new product();
                    oProduct.date_create = DateTime.Now;
                    oProduct.Descripcion = model.Descripcion;
                    oProduct.claveProdServ = model.ClaveProdServ;
                    oProduct.claveUnidad = model.ClaveUnidad;
                    oProduct.Precio = model.Precio;
                    oProduct.idState = 1;
                    oProduct.Unidad = model.Unidad;
                    oProduct.NoIdentificacion = model.NoIdentificacion;
                    db.product.Add(oProduct);
                    db.SaveChanges();
                }
                @TempData["Message"] = "Producto agregado con éxito";
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
            ProductViewModel model = new ProductViewModel();
            using (Entities db = new Entities())
            {
                var oProduct = db.product.Find(Id);
                model.Id = Id;
                model.Descripcion = oProduct.Descripcion;
                model.NoIdentificacion = oProduct.NoIdentificacion;
                model.ClaveProdServ = oProduct.claveProdServ;
                model.ClaveUnidad = oProduct.claveUnidad;
                model.Precio = oProduct.Precio;
                model.Unidad = oProduct.Unidad;
            }
            //obtenemos claves sat
            using (Models.SAT.satEntities db= new Models.SAT.satEntities())
            {
                var oClaveProdServ = db.f4_c_claveprodserv.Where(d => d.c_ClaveProdServ == model.ClaveProdServ).FirstOrDefault();
                var oClaveUnidad = db.f4_c_claveunidad.Where(d => d.c_ClaveUnidad == model.ClaveUnidad).FirstOrDefault();

                ViewBag.ClaveProdServ = oClaveProdServ !=null? oClaveProdServ.c_ClaveProdServ + " - "+oClaveProdServ.Descripcion :  "";
                ViewBag.ClaveUnidad = oClaveUnidad !=null? oClaveUnidad.c_ClaveUnidad + " - "+oClaveUnidad.Descripcion :  "";

            }

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(ProductViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                using (Entities db = new Entities())
                {
                    product oProduct = db.product.Find(model.Id);
                    oProduct.Descripcion = model.Descripcion;
                    oProduct.claveProdServ = model.ClaveProdServ;
                    oProduct.claveUnidad = model.ClaveUnidad;
                    oProduct.Precio = model.Precio;
                    oProduct.Unidad = model.Unidad;
                    oProduct.NoIdentificacion = model.NoIdentificacion;
                    db.Entry(oProduct).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }

                TempData["Message"] = "Producto modificado con éxito";
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
                    product oProduct = db.product.Find(Id);
                    oProduct.idState = 3;
                    db.Entry(oProduct).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("Error de sistema: " + ex.Message);
            }
        }

    }
}