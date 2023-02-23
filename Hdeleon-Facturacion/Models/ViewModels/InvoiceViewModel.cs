using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class InvoiceViewModel
    {
        #region DATOS PRINCIPALES

        public int Id { get; set; } //dato de la base de datos una vez guardado

        public byte[] QR
        {
            get
            {
                byte[] qr = null;

                QRViewModel Model = new QRViewModel();
                qr = Business.Barcode.QR.createBarCode("https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?id=" + UUID +
                    "&re=" + RFCEmisor + "&fe=" + RFCCliente + "&tt=" + Total + "&fe=" + Sello.Substring(Sello.Length - 9, 8));

                return qr;

            }
        }
        public string Logotipo { get; set; }
        public string NoCertificadoSAT { get; set; }
        public string SelloSAT { get; set; }
        public string NoCertificado { get; set; }
        public string Sello { get; set; }
        public string CadenaOriginal { get; set; }
        public string RFCEmisor { get; set; }
        public string RazonSocialEmisor { get; set; }
        public string NombreEmisor { get; set; }
        public string RegimenFiscal { get; set; }
        public string sRegimenFiscal { get; set; }
        public string UUID { get; set; }

        [Display(Name ="Condiciones de pago")]
        [StringLength(1000,ErrorMessage ="Las condiciones de pago no pueden superar los 1000 caracteres",MinimumLength =1)]
        public string CondicionesDePago { get; set; }

        [Display(Name = "Clave privada")]
        public string ClavePrivada { get; set; }

        [Display(Name = "Sucursal:")]
        public string Sucursal { get; set; }

        [Display(Name = "Forma de pago")]
        public string FormaPago { get; set; }
        public string sFormaPago { get; set; }

        [Display(Name = "Método de pago")]
        public string TipoDePago { get; set; }
        public string sTipoDePago { get; set; }

        [Display(Name = "Moneda")]
        public string Moneda { get; set; }

        [Display(Name = "Tipo de cambio")]
        public string TipoDeCambio { get; set; }

        public string LugarExpedicion { get; set; }
        [Display(Name = "Exportacion")]
        public string Exportacion { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        public DateTime? FechaTimbre { get; set; }

        [Display(Name = "Hora")]
        public string Hora { get; set; }

        [Display(Name = "Minuto")]
        public string Minuto { get; set; }

        [Display(Name = "Serie")]
        [Required]
        [StringLength(25, ErrorMessage = "La serie no puede superar 25 caracteres", MinimumLength = 1)]
        public string sSerie { get; set; }
        public string nombreSerie { get; set; }

       [Required]
        public int? Folio { get; set; }

        [Display(Name = "Uso del CFDI")]
        public string UsoCFDI { get; set; }
        public string sUsoCFDI { get; set; }


        public string tipoComprobante { get; set; }//dato calculado
        public string sTipoComprobante
        {
            get
            {
                string sTipoComprobante = "";

                switch (tipoComprobante)
                {
                    case "I":
                        sTipoComprobante = "Ingreso";
                        break;
                    case "E":
                        sTipoComprobante = "Egreso";
                        break;
                    case "T":
                        sTipoComprobante = "Traslado";
                        break;
                    case "N":
                        sTipoComprobante = "Nómina";
                        break;
                    case "P":
                        sTipoComprobante = "Pago";
                        break;

                }

                return sTipoComprobante;
            }
        }


        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        [Display(Name = "Tipo de relación de CFDIs")]
        public string TipoRelacion { get; set; }
        [Display(Name = "UUID relacionados")]
        public string UUIDRelacionados { get; set; }



        #endregion

        #region Información Global
        [Display(Name ="Tiene información global")]
        public bool hasInformacionGlobal { get; set; }
        public string Periodicidad { get; set; }
        public string Meses { get; set; }

        public short Ano { get; set; }
        #endregion

        #region DATOS DOMICILIO FISCAL
        public string CalleFiscal { get; set; }
        public string NoExteriorFiscal { get; set; }
        public string NoInteriorFiscal { get; set; }
        public string ColoniaFiscal { get; set; }
        public string CPFiscal { get; set; }
        public string PaisFiscal { get; set; }

        public string EstadoFiscal { get; set; }

        public string MunicipioFiscal { get; set; }
        #endregion

        #region DATOS RECEPTOR

        //   [Display(Name = "Buscador")]
        // public int idCliente { get; set; }

        [Display(Name = "RFC")]
        [RegularExpression(@"^[A-Za-zÑñ&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Za-z0-9]{2}[0-9Aa]$", ErrorMessage = "El rfc no tiene formato valido")]
        [Required]
        public string RFCCliente { get; set; }

        [Display(Name = "Razón social")]
        [Required]
        public string RazonSocial { get; set; }

        [Display(Name="Régimen fiscal")]
        public string RegimenFiscalCliente { get; set; }

        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Display(Name = "No. Exterior")]
        public string NoExterior { get; set; }

        [Display(Name = "No. Interior")]
        public string NoInterior { get; set; }

        [Display(Name = "Colonia")]
        public string Colonia { get; set; }

        [Display(Name = "Localidad")]
        public string Localidad { get; set; }


        [Display(Name = "Código postal")]
        public string CP { get; set; }

        [Display(Name = "País")]
        public string Pais { get; set; }

        [Display(Name = "Estado")]
        public int Estado { get; set; }
        public string sEstado { get; set; }

        [Display(Name = "Municipio")]
        public int Municipio { get; set; }
        public string sMunicipio { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Correo electrónico")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Referencia")]
        public string Referencia { get; set; }

        #endregion

        #region DATOS MONTOS
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        public decimal TotalDescuento { get; set; }

        public decimal TotalFederalTraslado { get; set; }
        public decimal TotalFederalRetenido { get; set; }
        public decimal TotalLocalTraslado { get; set; }
        public decimal TotalLocalRetenido { get; set; }

        public string sTotalLocalTraslado
        {
            get
            {
                if (TotalLocalTraslado > 0)
                    return TotalLocalTraslado.ToString("#.00");
                return "0.00";
            }
        }

        public string sTotalLocalRetenido
        {
            get
            {
                if (TotalLocalRetenido > 0)
                    return TotalLocalRetenido.ToString("#.00");
                return "0.00";
            }
        }


        #region impuestos separados
        public decimal totalIvaTrasladado { get; set; }
        public decimal totalIEPSTrasladado { get; set; }
        public decimal totalIEPSRetenido { get; set; }
        public decimal totalIVARetenido { get; set; }
        public decimal totalISRRetenido { get; set; }

        public decimal tasaIvaTrasladado { get; set; }
        public decimal tasaIEPSTrasladado { get; set; }
        public decimal tasaIEPSRetenido { get; set; }
        public decimal tasaIVARetenido { get; set; }
        public decimal tasaISRRetenido { get; set; }

        public string sTotalIVATrasladado
        {
            get
            {
                if (totalIvaTrasladado > 0)
                    return totalIvaTrasladado.ToString("0,0.00");
                return "0.00";
            }
        }
        public string sTotalIEPSTrasladado
        {
            get
            {
                if (totalIEPSTrasladado > 0)
                    return totalIEPSTrasladado.ToString("0,0.00");
                return "0.00";
            }
        }
        public string sTotalIEPSRetenido
        {
            get
            {
                if (totalIEPSRetenido > 0)
                    return totalIEPSRetenido.ToString("0,0.00");
                return "0.00";
            }
        }
        public string sTotalIVARetenido
        {
            get
            {
                if (totalIVARetenido > 0)
                    return totalIVARetenido.ToString("0,0.00");
                return "0.00";
            }
        }
        public string sTotalISRRetenido
        {
            get
            {
                if (totalISRRetenido > 0)
                    return totalISRRetenido.ToString("0,0.00");
                return "0.00";
            }
        }
        #endregion

      
        #endregion



        [CheckListExist(ErrorMessage = "Debe existir por lo menos 1 concepto")]
        public List<Concepto> conceptos { get; set; }


        public InvoiceViewModel()
        {
            conceptos = new List<Concepto>();
            TotalFederalRetenido = 0;
            TotalFederalTraslado = 0;
            TotalLocalRetenido = 0;
            TotalLocalTraslado = 0;
        }

        public class Concepto
        {
            public int numConcepto { get; set; }
            public string claveProducto { get; set; }
            //public string codigo { get; set; }
            public decimal cantidad { get; set; }
            public string claveUnidad { get; set; }
            public string unidad { get; set; }
            public string descripcion { get; set; }
            public decimal? descuento { get; set; }
            public decimal precioUnitario { get; set; }

            public string cuentaPredial { get; set; }

            public string objetoImp { get; set; }
            public List<Impuesto> impuestos { get; set; }
            public List<int> Taxes { get; set; } //solo sirve para el html al agregar
            public Concepto()
            {
                impuestos = new List<Impuesto>();
            }
        }

        public class Impuesto
        {
         
            public string nombre { get; set; }
            public decimal tasa { get; set; }
            public string ambito { get; set; }
            public string tipo { get; set; }

            public string factor { get; set; }
            //valor que se utiliza en el pdf, se llena al hacer la factura, es decir no viene en el formulario
            public decimal importe { get; set; }
            public string claveImpuesto { get; set; }

        }

    }

    public class CheckListExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as System.Collections.IList;
            if (list != null)
            {
                return list.Count > 0;
            }
            return false;
        }
    }
}