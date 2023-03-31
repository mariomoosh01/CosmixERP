using CosmixERP.AccessLayer.Services;
using Hdeleon_Facturacion.Models;
using Hdeleon_Facturacion.Models.Catalogs;
using Hdeleon_Facturacion.Models.ViewModels;
using Hdeleon_Facturacion.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class InvoiceController : BaseCrudController<Models.ViewModels.TableInvoiceViewModel>
    {
        private InvoicesServices _invoicesServices;

        public InvoiceController()
        {
            _invoicesServices = new InvoicesServices();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Json()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    IQueryable<Models.ViewModels.TableInvoiceViewModel> lst = from d in db.invoice
                                                                              orderby d.id descending
                                                                              select new Models.ViewModels.TableInvoiceViewModel
                                                                              {
                                                                                  Id = d.id,
                                                                                  Folio = d.Folio,
                                                                                  Total = d.Total,
                                                                                  DateCreated =d.date_create,
                                                                                  Serie=d.Serie,
                                                                                  RFCReceptor=d.RFCReceptor,
                                                                                  IdStatusInvoice= (int)d.idStateInvoice
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
            GetSession();
            InvoiceViewModel model = new InvoiceViewModel();
            
            model.Fecha = DateTime.Now;

            Combobox();

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(InvoiceViewModel model)
        {
            try
            {
                GetSession();
                var caca = GlobalParams.User;
                //validación
                if(!ModelState.IsValid)
                {
                    return Content("Error: "+getModelErrors());
                }

                //obtenemos datos de bd
                using (Models.Entities db = new Models.Entities())
                {
                    var oUser = db.user.Find(User.id);
                    model.ClavePrivada = oUser.PrivateCode; //asignamos clave privada
                    model.NombreEmisor = oUser.name;
                    model.RazonSocialEmisor = oUser.fiscal_name;
                    model.RFCEmisor = oUser.rfc;
                    model.RegimenFiscal = oUser.CodeTaxRegime;
                    Business.Invoice oInvoice = 
                        new Business.Invoice(model, oUser.rfc, oUser.fiscal_name, oUser.CodeTaxRegime, 
                        oUser.UserPacFC, oUser.PassPacFC, Server.MapPath("~"), oUser.cp);
                    oInvoice.Create();

                    //creamos el pdf
                    Business.FormatPDF.InvoicePDF oFP = 
                        new Business.FormatPDF.InvoicePDF(Server.MapPath("~/Xml/"+oInvoice.NameXML.Replace(".xml",".pdf")), model);
                    oFP.Create();

                    //agregamos la factura a la base de datos
                    var oInvoiceDB = new Models.invoice();
                    oInvoiceDB.xml = oInvoice.NameXML;
                    oInvoiceDB.idType = 1; //1 es factura
                    oInvoiceDB.idUser = oUser.id; //1 es admin
                    oInvoiceDB.RFCReceptor = model.RFCCliente;
                    oInvoiceDB.Serie = model.sSerie;
                    oInvoiceDB.Folio = (int)model.Folio;
                    oInvoiceDB.date_create = DateTime.Now;
                    oInvoiceDB.Total = model.Total;
                    oInvoiceDB.idStateInvoice = 1; //1 es factura sin cancelar 
                    oInvoiceDB.UUID = oInvoice.factura.UUID;
                    db.invoice.Add(oInvoiceDB);
                    db.SaveChanges();

                    //retornamos el id de la factura ya guardada
                    return Content(oInvoiceDB.id.ToString());
                }

             
            }
            catch (Exception ex)
            {
                return Content(Business.Constants.ExcepcionMessage + ex.Message);
            }

        }

        public ActionResult Show(int Id)
        {
            ShowInvoiceViewModel model = new ShowInvoiceViewModel();

            using (Models.Entities db= new Models.Entities())
            {
                var oInvoice = db.invoice.Find(Id);
                model.UUID = oInvoice.UUID;
                model.Id = Id;
                model.NameXML = oInvoice.xml;
                
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Download(int Id, int A) // ACCION =1 xml, 2 pdf
        {
            string nameFile = "";
            using (Models.Entities db= new Models.Entities()) {
                var oInvoice = db.invoice.Find(Id);
                nameFile = oInvoice.xml;
            }

            string path = Server.MapPath("~/") + "/XML/";


            string pathFull = (A == 1) ? path  + nameFile : path + nameFile.Replace(".xml",".pdf");

            Response.AddHeader("Content-Disposition", "attachment;filename=" +  ((A == 1) ? nameFile : nameFile.Replace(".xml", ".pdf")));
            Response.WriteFile(pathFull);
            Response.End();
            return null;

        }

        /*
        [HttpPost]
        public ActionResult Cancel(int Id)
        {
            try
            {
                GetSession();
                using (Models.satEntities db= new Models.satEntities())
                {
                    var oInvoice = db.invoice.Find(Id);
                    byte[] bPfx = System.IO.File.ReadAllBytes(Server.MapPath("~/")+ Business.Constants.PathPfx);
                    Business.Cancel.CancelInvoice oCancelInvoice =
                        new Business.Cancel.CancelInvoice(User.UserPacFC, User.PassPacFC, oInvoice.UUID, User.PrivateCode, bPfx,
                        (double)oInvoice.Total, User.rfc, oInvoice.RFCReceptor);
                    oCancelInvoice.Cancel();

                    //editamos el registro en la base de datos
                    oInvoice.idStateInvoice = 2; //cancelar
                    db.Entry(oInvoice).State = System.Data.EntityState.Modified;
                    db.SaveChanges();

                }
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content(Business.Constants.ExcepcionMessage + ex.Message);
            }
        }
        */

        public async Task<ActionResult> GetTicketrDetail(int ticketNo)
        {
            var detail = await _invoicesServices.GetInvoiceDetail(ticketNo);

            return Json(JsonConvert.SerializeObject(detail),JsonRequestBehavior.AllowGet);
        }

        #region Second ActionResult
        public ActionResult AddConcepto(Models.ViewModels.InvoiceViewModel.Concepto concepto)
        { 
            //llenamos impuestos
            if (concepto.Taxes!=null && concepto.Taxes.Count > 0)
            {
                concepto.impuestos = new List<InvoiceViewModel.Impuesto>(); //inicializamos impuestos
                using (Models.Entities db= new Models.Entities()) {
                    foreach (var idTax in concepto.Taxes)
                    {
                        //buscamos el impuesto y anexamos
                        var oTax = db.tax.Find(idTax);
                        Models.ViewModels.InvoiceViewModel.Impuesto impuesto = new Models.ViewModels.InvoiceViewModel.Impuesto();
                        impuesto.ambito = oTax.type_ambit;
                        impuesto.factor = oTax.tipo_factor;
                        impuesto.nombre = oTax.name;
                        impuesto.tasa = oTax.tasa;
                        impuesto.tipo = oTax.type;
                        impuesto.claveImpuesto = oTax.claveImpuesto;
                        concepto.impuestos.Add(impuesto);
                    }
                }
            }

            return View(concepto);
        }

        #endregion

        #region HELPERS

        // Obteiene la información de todos los combobox
        private void Combobox()
        {
            //forma pago
            Models.SAT.satEntities db2 = new Models.SAT.satEntities();
            Models.Entities db = new Models.Entities();
            var lstFormaPago = from d in db2.f4_c_formapago
                               select new { code = d.c_FormaPago, name = d.Descripcion };
            SelectList slcFormaPago = new SelectList(lstFormaPago,"code","name");

            ViewBag.lstFormaPago = slcFormaPago;

            //forma pago
            var lstMetodoPago = from d in db2.f4_c_metodopago
                               select new { code = d.c_MetodoPago, name = d.Descripcion };
            SelectList slcMetodoPago = new SelectList(lstMetodoPago, "code", "name");

            ViewBag.lstMetodoPago = slcMetodoPago;

            //forma pago
            var lstUsoCfdi = from d in db2.f4_c_usocfdi
                                select new { code = d.c_UsoCFDI, name = d.Descripcion };
            SelectList slcUsoCFDI = new SelectList(lstUsoCfdi, "code", "name");

            ViewBag.lstUsoCfdi = slcUsoCFDI;

            //moneda
            var lstMoneda = from d in db2.f4_c_moneda
                            where d.id==100 //solo mxn
                             select new { code = d.c_Moneda, name = d.Descripcion };
            SelectList slcMoneda = new SelectList(lstMoneda, "code", "name");

            ViewBag.lstMoneda = slcMoneda;

            //regimen fiscal
            var lstRegimenFiscal = from d in db2.f4_c_regimenfiscal
                            select new { code = d.c_RegimenFiscal, name = d.Descripcion };
            SelectList slcRegimenFiscal = new SelectList(lstRegimenFiscal, "code", "name");

            ViewBag.lstRegimenFiscal = slcRegimenFiscal;

            //estados
            var lstEstados = (from d in db.destado
                              orderby d.Nombre
                              select d).ToList();
            SelectList slcEstados = new SelectList(lstEstados, "iddEstado", "Nombre");
            ViewBag.slcEstados = slcEstados;

            //impuestos
            ViewBag.lstTax = (from d in db.tax
                              orderby d.name
                              where d.idState == 1
                              select new TaxViewModel
                              {
                                  Name = d.name,
                                  Id = d.id,
                                  Tasa=d.tasa,
                                  Factor=d.tipo_factor,
                                  Type=d.type
                              }).ToList();

            // exportación
            var lstExportacion = (from d in db2.cexportacion
                                  select new { code = d.clave, name = d.descripcion });
            ViewBag.lstExportacion = new SelectList(lstExportacion, "code", "name");

            // objeto impuesto
            var lstObjetoImp = (from d in db2.cobjetoimpuesto
                                  select new { code = d.clave, name = d.descripcion });
            ViewBag.lstObjetoImp = new SelectList(lstObjetoImp, "code", "name");

            // regimen fiscal
            ViewBag.lstCodeTaxRegime = 
                new SelectList((from d in db2.f4_c_regimenfiscal
                                select new { code = d.c_RegimenFiscal, name = d.Descripcion }),
                                "code",
                                "name");
            // periodicidad
            ViewBag.lstPeriodicidad = 
                new SelectList((from d in db2.c_periodicidad
                                select new
                                {
                                    code = d.clave,
                                    name = d.descripcion
                                }), "code", "name");

            ViewBag.lstMeses =
                new SelectList((from d in db2.c_meses
                                select new
                                {
                                    code = d.clave,
                                    name = d.descripcion
                                }), "code", "name");

            ViewBag.lstAnos = new SelectList((from d in Years.Get()
                                              select new
                                              {
                                                  code = d,
                                                  name = d
                                              }), "code", "name");

        }
        #endregion
    }




}