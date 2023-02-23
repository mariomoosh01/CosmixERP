using Hdeleon_Facturacion.Models.ViewModels;
using Hdeleon_Facturacion.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Hdeleon_Facturacion.Models.ViewModels.InvoiceViewModel;

namespace Hdeleon_Facturacion.Business
{
    public class Invoice
    {
        public InvoiceViewModel factura;
        Comprobante comprobante;
        private string error = "";
        
        private string PathApp = "";
        private string UserPac, PassPac, CP;
        public string NameXML = "";
        private string PathCadenaOriginal
        {
            get
            {
                return PathApp+ "/xslt4/cadenaoriginal_4_0.xslt";
            }
        }
        private string PathXml
        {
            get
            {
                return PathApp + "/Xml/temp"+NameXML; 
            }
        }
        private string PathXmlTimbrado
        {
            get
            {
                return PathApp + "/Xml/"+NameXML;
            }
        }
        private string PathCer
        {
            get
            {
                return PathApp + "/" + Business.Constants.PathCer;
            }
        }
        private string PathKey
        {
            get
            {
                return PathApp + "/" + Business.Constants.PathKey;
            }
        }
        private string RFCEmisor, NameEmisor, CodeTaxRegime;
        #region Propiedades
        public string Error
        {
            get
            {
                return Error;
            }
        }
        public bool IsError
        {
            get
            {
                if (error != "")
                    return true;
                return false;
            }
        }
        #endregion

        public Invoice(InvoiceViewModel oFactura, string RFCEmisor,string NameEmisor,string CodeTaxRegime,
            string UserPac, string PassPac,string PathApp, string CP)
        {
            this.factura = oFactura;
            this.RFCEmisor = RFCEmisor;
            this.NameEmisor = NameEmisor;
            this.CodeTaxRegime = CodeTaxRegime;
            this.UserPac = UserPac;
            this.PassPac = PassPac;
            this.PathApp = PathApp; //ruta del sistema web
            this.CP = CP;
            //Se genera el nombre aleatorio para el xml
            this.NameXML = Guid.NewGuid().ToString()+".xml";
            comprobante = new Comprobante();
        }

        public void Create()
        {
            CreateXML();
            SellarXML();
            TimbrarXML();
            GetDataXML(); //obtiene datos en el modelo factura que solo se obtienen posterior al timbrado
            GetDataPDF(); //obtiene datos para ser mostrados en el pdf de catalogos.
            Clean();

        }

        private void CreateXML()
        {
            decimal totalDescuento = 0,totalConceptos=0;
            string numeroCertificado, aa, b, c;
            SelloDigital.leerCER(PathCer, out aa, out b, out c, out numeroCertificado);

            comprobante.Version = "4.0";
            comprobante.Serie = factura.sSerie;
            comprobante.Folio = factura.Folio.ToString();
            comprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            comprobante.FormaPago = factura.FormaPago.Length==1 ? "0"+factura.FormaPago : factura.FormaPago;
            comprobante.NoCertificado = numeroCertificado;
            comprobante.Moneda = factura.Moneda;
            comprobante.TipoDeComprobante = "I";
            comprobante.MetodoPago = factura.TipoDePago;
            comprobante.LugarExpedicion = CP;
            comprobante.Exportacion = factura.Exportacion; 

            ComprobanteEmisor emisor = new ComprobanteEmisor();
            emisor.Rfc = RFCEmisor;
            emisor.Nombre = NameEmisor;
            emisor.RegimenFiscal = CodeTaxRegime;
            

            ComprobanteReceptor receptor = new ComprobanteReceptor();
            receptor.Nombre = factura.RazonSocial;
            receptor.Rfc = factura.RFCCliente;
            receptor.UsoCFDI = factura.UsoCFDI;
            receptor.RegimenFiscalReceptor = factura.RegimenFiscalCliente; 
            receptor.DomicilioFiscalReceptor = factura.CP;

            //asigno emisor y receptor
            comprobante.Emisor = emisor;
            comprobante.Receptor = receptor;

            //información global
            if (factura.hasInformacionGlobal)
            {
                comprobante.InformacionGlobal = new ComprobanteInformacionGlobal()
                {
                    Periodicidad = factura.Periodicidad,
                    Año = factura.Ano,
                    Meses = factura.Meses
                };
            }


            List<ComprobanteConcepto> lstConceptos = new List<ComprobanteConcepto>();
            //arreglos impuestos comprobante
            List<ComprobanteImpuestosTraslado> lstComprobanteImpuestoTraslados = new List<ComprobanteImpuestosTraslado>(); //nodo comprobante
            List<ComprobanteImpuestosRetencion> lstComprobanteImpuestoRetenidos = new List<ComprobanteImpuestosRetencion>(); //nodo comprobante
            foreach (Concepto conceptoVM in factura.conceptos) {

                decimal importeTotal = Math.Round( conceptoVM.cantidad * conceptoVM.precioUnitario);
                decimal totalConceptoSinDescuento = importeTotal;
                if (conceptoVM.descuento > 0)
                {
                    totalConceptoSinDescuento -= (decimal)conceptoVM.descuento; //le restamos el descuento
                }

                ComprobanteConcepto concepto = new ComprobanteConcepto();
               
                concepto.ClaveProdServ = conceptoVM.claveProducto.Split('-')[0].Trim();
                concepto.Cantidad = conceptoVM.cantidad;
                concepto.ClaveUnidad = conceptoVM.claveUnidad.Split('-')[0].Trim();
                concepto.Descripcion = conceptoVM.descripcion;
                concepto.ValorUnitario = conceptoVM.precioUnitario;
                if(conceptoVM.descuento!=null && conceptoVM.descuento>0)
                     concepto.Descuento =(decimal)conceptoVM.descuento;
                concepto.Importe = (conceptoVM.cantidad*conceptoVM.precioUnitario);
                concepto.ObjetoImp = conceptoVM.objetoImp; 

                #region IMPUESTOS
                if(conceptoVM.impuestos!=null && conceptoVM.impuestos.Count>0) //si es que existen impuestos
                {
                  
                    concepto.Impuestos = new ComprobanteConceptoImpuestos(); //inicializamos el objeto
                    //arreglos impuestos concepto
                    List<ComprobanteConceptoImpuestosTraslado> lstImpuestosTrasladados = new List<ComprobanteConceptoImpuestosTraslado>(); //nodo concepto
                    List<ComprobanteConceptoImpuestosRetencion> lstImpuestosRetenidos = new List<ComprobanteConceptoImpuestosRetencion>(); //nodo concepto

                    foreach (var impuestoVM in conceptoVM.impuestos)
                    {
                        if (impuestoVM.ambito == Business.Constants.AMBITO.FEDERAL)
                        {

                            decimal importeImpuesto = Math.Round(impuestoVM.tasa * totalConceptoSinDescuento, 2);
                            impuestoVM.importe = importeImpuesto;//para que aparezca en el pdf
                            string tipoFactor = impuestoVM.factor;

                           // if (oImpuestoVM.tasa == 0)
                             //   tipoFactor = "Exento";

                            //IMPUESTO TRASLADADO
                            if (impuestoVM.tipo.Equals(Business.Constants.TIPO_IMPUESTO.TRASLADO))
                            {
                                ComprobanteConceptoImpuestosTraslado oImpuestoTrasladado = new ComprobanteConceptoImpuestosTraslado();
                                if (tipoFactor.Equals("Exento")) //si es exento no va ni importe ni tasa
                                {
                                    oImpuestoTrasladado.Base = totalConceptoSinDescuento;
                                    oImpuestoTrasladado.TipoFactor = impuestoVM.factor;
                                    oImpuestoTrasladado.Impuesto = impuestoVM.claveImpuesto;
                                    lstImpuestosTrasladados.Add(oImpuestoTrasladado);
                                }
                                else
                                {
                                    oImpuestoTrasladado.Base = totalConceptoSinDescuento;
                                    oImpuestoTrasladado.TasaOCuota = impuestoVM.tasa;
                                    oImpuestoTrasladado.TipoFactor = impuestoVM.factor;
                                    oImpuestoTrasladado.Impuesto = impuestoVM.claveImpuesto;
                                    oImpuestoTrasladado.Importe = importeImpuesto;
                                    lstImpuestosTrasladados.Add(oImpuestoTrasladado);
                                }


                                factura.TotalFederalTraslado += importeImpuesto; //sumamos total

                                //se agrega al nodo impuestos, solo si no es exento
                                if (!tipoFactor.Equals("Exento")) 
                                {
                                    ComprobanteImpuestosTraslado oIT = new ComprobanteImpuestosTraslado();
                                    oIT.Impuesto = impuestoVM.claveImpuesto;
                                    oIT.TipoFactor = impuestoVM.factor;
                                    oIT.TasaOCuota = impuestoVM.tasa;
                                    oIT.Importe = impuestoVM.importe;
                                    oIT.Base = concepto.Importe;
                                    AddTraslado(lstComprobanteImpuestoTraslados, oIT);

                                }

                                //acumular totales
                                if (impuestoVM.nombre == "IVA")
                                    factura.totalIvaTrasladado += importeImpuesto;
                                if (impuestoVM.nombre == "IEPS")
                                    factura.totalIEPSTrasladado += importeImpuesto;
                            }
                            //RETENCIÓN
                            else if (impuestoVM.tipo.Equals(Business.Constants.TIPO_IMPUESTO.RETENCION))
                            {
                                ComprobanteConceptoImpuestosRetencion oImpuestoRetencion = new ComprobanteConceptoImpuestosRetencion();
                                if (tipoFactor.Equals("Exento")) //si es exento no va ni importe ni tasa
                                {
                                    oImpuestoRetencion.Base = totalConceptoSinDescuento;
                                    oImpuestoRetencion.TipoFactor = impuestoVM.factor;
                                    oImpuestoRetencion.Impuesto = impuestoVM.claveImpuesto;
                                    lstImpuestosRetenidos.Add(oImpuestoRetencion);
                                }
                                else
                                {
                                    oImpuestoRetencion.Base = totalConceptoSinDescuento;
                                    oImpuestoRetencion.TasaOCuota = impuestoVM.tasa;
                                    oImpuestoRetencion.TipoFactor = impuestoVM.factor;
                                    oImpuestoRetencion.Impuesto = impuestoVM.claveImpuesto;
                                    oImpuestoRetencion.Importe = importeImpuesto;
                                    lstImpuestosRetenidos.Add(oImpuestoRetencion);
                                }

                                factura.TotalFederalRetenido += importeImpuesto; //sumamos total

                                //se agrega al nodo impuestos
                                ComprobanteImpuestosRetencion oIR = new ComprobanteImpuestosRetencion();
                                oIR.Impuesto = impuestoVM.claveImpuesto;
                                oIR.Importe = impuestoVM.importe;
                                AddRetencion(lstComprobanteImpuestoRetenidos, oIR);

                                //acumular totales
                                if (impuestoVM.nombre == "IVA")
                                    factura.totalIVARetenido += importeImpuesto;
                                if (impuestoVM.nombre == "IEPS")
                                    factura.totalIEPSRetenido += importeImpuesto;
                                if (impuestoVM.nombre == "ISR")
                                    factura.totalISRRetenido += importeImpuesto;
                            }
                        }

                    }

                    if (lstImpuestosTrasladados.Count > 0)
                        concepto.Impuestos.Traslados = lstImpuestosTrasladados.ToArray();
                    if (lstImpuestosRetenidos.Count > 0)
                        concepto.Impuestos.Retenciones = lstImpuestosRetenidos.ToArray();

                  
                }
                #endregion

                lstConceptos.Add(concepto);

                totalConceptos += importeTotal;
                if (conceptoVM.descuento!=null && conceptoVM.descuento > 0)
                    totalDescuento += (decimal)conceptoVM.descuento;
            }

            //agregamso al nodo principal de impuestos
            if(lstComprobanteImpuestoTraslados.Count > 0 || lstComprobanteImpuestoRetenidos.Count>0) //si hay impuestos creamos este objeto
                comprobante.Impuestos = new ComprobanteImpuestos(); //inicializamos el objeto
            if (lstComprobanteImpuestoTraslados.Count > 0) //se agrega al nodo principal traslados
            {
                comprobante.Impuestos.Traslados = lstComprobanteImpuestoTraslados.ToArray();
                comprobante.Impuestos.TotalImpuestosTrasladados = factura.TotalFederalTraslado;
            }
            if (lstComprobanteImpuestoRetenidos.Count > 0) //se agrega al nodo principal retenidos
            {
                comprobante.Impuestos.Retenciones = lstComprobanteImpuestoRetenidos.ToArray();
                comprobante.Impuestos.TotalImpuestosRetenidos = factura.TotalFederalRetenido;
            }

            comprobante.Conceptos = lstConceptos.ToArray();

            if (totalDescuento > 0)
                comprobante.Descuento = totalDescuento;
            comprobante.SubTotal = totalConceptos;
            comprobante.Total = totalConceptos - totalDescuento + (factura.TotalFederalTraslado - factura.TotalFederalRetenido);
            
            //anexamos valores para el archivo pdf
            factura.Total = comprobante.Total;
            factura.Subtotal = comprobante.SubTotal;
            factura.TotalDescuento = comprobante.Descuento;

            CreateXMLFile();
        }

        private void SellarXML()
        {
            string CadenaOriginal = "";
            System.Xml.Xsl.XslCompiledTransform transformador = new System.Xml.Xsl.XslCompiledTransform(true);
            transformador.Load(PathCadenaOriginal);

            using (StringWriter sw = new StringWriter())
            using (XmlWriter xwo = XmlWriter.Create(sw, transformador.OutputSettings))
            {
                transformador.Transform(PathXml, xwo);
                CadenaOriginal = sw.ToString();
            }
            SelloDigital oSelloDigital = new SelloDigital();
            comprobante.Certificado = oSelloDigital.Certificado(PathCer);
            comprobante.Sello = oSelloDigital.Sellar(CadenaOriginal, PathKey, factura.ClavePrivada);

            CreateXMLFile();

            
        }

        private void TimbrarXML()
        {
            byte[] bXml = System.IO.File.ReadAllBytes(PathXml);
            Timbrado.Timbrado oTimbrar = new Timbrado.Timbrado(UserPac,PassPac);
            oTimbrar.Timbrar(bXml);
            System.IO.File.WriteAllBytes(PathXmlTimbrado,oTimbrar.XMLTimbrado);
        }

        private void Clean()
        {
            try
            {
                //eliminamos el xml temporal
                File.Delete(PathXml);
            }
            catch { }
        }
        #region Helpers
        private  void CreateXMLFile()
        {
            //SERIALIZAMOS.-------------------------------------------------

            XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add("cfdi", "http://www.sat.gob.mx/cfd/4");
            xmlNameSpace.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            xmlNameSpace.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");


            XmlSerializer oXmlSerializar = new XmlSerializer(typeof(Comprobante));

            string sXml = "";

            using (var sww = new Utils.StringWriterWithEncoding(Encoding.UTF8))
            {

                using (XmlWriter writter = XmlWriter.Create(sww))
                {

                    oXmlSerializar.Serialize(writter, comprobante, xmlNameSpace);
                    sXml = sww.ToString();
                }

            }

            //guardamos el string en un archivo
            System.IO.File.WriteAllText(PathXml, sXml);
        }

        public void GetDataXML()
        {
            XNamespace cfdi = @"http://www.sat.gob.mx/cfd/4";
            XNamespace tfd = @"http://www.sat.gob.mx/TimbreFiscalDigital";
            var xdoc = XDocument.Load(PathXmlTimbrado);

            // Walk down the XML tree to tfd:TimbreFiscalDigital
            var elt = xdoc.Element(cfdi + "Comprobante")
                          .Element(cfdi + "Complemento")
                          .Element(tfd + "TimbreFiscalDigital");
            var comprobante = xdoc.Element(cfdi + "Comprobante");

            // Alternately
            // var elt = xdoc.Descendants(tfd + "TimbreFiscalDigital")
            //               .First();

            factura.UUID = (string)elt.Attribute("UUID");
            factura.Sello = (string)comprobante.Attribute("Sello");
            factura.Fecha = (DateTime)comprobante.Attribute("Fecha");
            factura.FechaTimbre = (DateTime)elt.Attribute("FechaTimbrado");
            factura.tipoComprobante = (string)comprobante.Attribute("TipoDeComprobante");
            factura.NoCertificadoSAT = (string)elt.Attribute("NoCertificadoSAT");
            factura.SelloSAT = (string)elt.Attribute("SelloSAT");
            factura.NoCertificado = (string)comprobante.Attribute("NoCertificado");
        }
        private void GetDataPDF()
        {
            using (Models.SAT.satEntities db2 = new Models.SAT.satEntities()) {
                //regimen fiscal
                factura.sRegimenFiscal = db2.f4_c_regimenfiscal.Where(d => d.c_RegimenFiscal == factura.RegimenFiscal).First().Descripcion;
                //forma pago
                factura.sFormaPago = db2.f4_c_formapago.Where(d => d.c_FormaPago == factura.FormaPago).First().Descripcion;
                //forma pago
                factura.sTipoDePago = db2.f4_c_metodopago.Where(d => d.c_MetodoPago == factura.TipoDePago).First().Descripcion;
                //usocfdi
                factura.sUsoCFDI = db2.f4_c_usocfdi.Where(d => d.c_UsoCFDI == factura.UsoCFDI).First().Descripcion;
            }
            using (Models.Entities  db = new Models.Entities())
            {
                //estado
                if(factura.Estado!=0)
                factura.sEstado = db.destado.Where(d => d.iddEstado == factura.Estado).First().Nombre;
                //municipio
                if(factura.Municipio!=0)
                factura.sMunicipio = db.cmunicipio.Where(d => d.idcMunicipio == factura.Municipio).First().Municipio;
            }
        }

        private void AddTraslado(List<ComprobanteImpuestosTraslado> lst, ComprobanteImpuestosTraslado impuesto)
        {
            bool exist = false;
            if(lst!=null)
            foreach (var imp in lst)
            {
                    if (imp.Impuesto == impuesto.Impuesto)
                    {
                        imp.Importe += impuesto.Importe;
                        imp.Base += impuesto.Base;
                        exist = true;
                    }
            }
            if (!exist)
                lst.Add(impuesto);
        }
        private void AddRetencion(List<ComprobanteImpuestosRetencion> lst, ComprobanteImpuestosRetencion oImpuesto)
        {
            bool exist = false;
            if (lst != null)
                foreach (var oImp in lst)
                {
                    if (oImp.Impuesto == oImpuesto.Impuesto)
                    {
                        oImp.Importe += oImpuesto.Importe;
                        exist = true;
                    }
                }
            if (!exist)
                lst.Add(oImpuesto);
        }
        #endregion
    }
}