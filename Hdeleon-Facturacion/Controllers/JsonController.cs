using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class JsonController : Controller
    {
        public class ElementJson
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }
        public class ElementJsonIntKey
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }
        public class ElementJsonObjectKey
        {
            public object Id { get; set; }
            public string Value { get; set; }
        }

        [HttpPost]
        public JsonResult ProductosServicios(string cad)
        {
            List<ElementJson> lst = new List<ElementJson>();
            using (Hdeleon_Facturacion.Models.SAT.satEntities db =
                new Models.SAT.satEntities())
            {
                lst = (from d in db.f4_c_claveprodserv
                       where d.Descripcion.Contains(cad) || d.c_ClaveProdServ.Contains(cad)
                       select new ElementJson
                       {
                           Id = d.c_ClaveProdServ,
                           Value = (d.c_ClaveProdServ + " - " + d.Descripcion)
                       }).Take(20).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public JsonResult ClaveUnidad(string cad)
        {
            List<ElementJson> lst = new List<ElementJson>();
            using (Hdeleon_Facturacion.Models.SAT.satEntities db =
                new Models.SAT.satEntities())
            {
                lst = (from d in db.f4_c_claveunidad
                       where d.Nombre.Contains(cad) || d.c_ClaveUnidad.Contains(cad)
                       select new ElementJson
                       {
                           Id = d.c_ClaveUnidad,
                           Value = (d.c_ClaveUnidad + " - " + d.Nombre)
                       }).Take(20).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Municipios(int IdEstado)
        {
            List<ElementJsonIntKey> lst = new List<ElementJsonIntKey>();
            using (Hdeleon_Facturacion.Models.Entities db =
                new Models.Entities())
            {
                lst = (from d in db.cmunicipio
                       where d.iddEstado==IdEstado
                       select new ElementJsonIntKey
                       {
                           Id = d.idcMunicipio,
                           Value = d.Municipio
                       }).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Customer(string cad)
        {
            List<ElementJsonObjectKey> lst = new List<ElementJsonObjectKey>();
            using (Hdeleon_Facturacion.Models.Entities db =
                new Models.Entities())
            {
                lst = (from d in db.customer
                       where d.name.ToUpper().Contains(cad.ToUpper()) || d.RFC.ToUpper().Contains(cad.ToUpper())
                       select new ElementJsonObjectKey
                       {
                           Id = new Models.ViewModels.CustomerViewModel { Id=d.id, Name= d.name, Street= d.Street,
                                                                          idEstado= d.idEstado, IdMunicipio= d.idMunicipio,
                                                                          CP= d.CP, NoExt=d.NoExt, NoInt=d.NoInt,Email=d.email,
                                                                          RFC=d.RFC, CommercialName=d.comercial_name,
                                                                          CodeTaxRegime = d.CodeTaxRegime
                           },
                           Value = (d.RFC + " - " + d.name)
                       }).Take(20).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult Product(string cad)
        {
            List<ElementJsonObjectKey> lst = new List<ElementJsonObjectKey>();
            using (Hdeleon_Facturacion.Models.Entities db =
                new Models.Entities())
            {
                lst = (from d in db.product
                       where d.Descripcion.ToUpper().Contains(cad.ToUpper()) || d.claveProdServ.ToUpper().Contains(cad.ToUpper())
                       select new ElementJsonObjectKey
                       {
                           Id = new Models.ViewModels.ProductViewModel
                           {
                               Id = d.id,
                               ClaveUnidad = d.claveUnidad,
                               Descripcion= d.Descripcion,
                               ClaveProdServ= d.claveProdServ,
                               Precio= d.Precio
                               
                           },
                           Value = (d.claveProdServ + " - " + d.Descripcion)
                       }).Take(20).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);

        }
    }
}