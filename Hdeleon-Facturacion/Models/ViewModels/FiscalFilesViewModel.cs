using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class FiscalFilesViewModel
    {

        [Required]
        [DisplayName("Archivo cer")]
        public HttpPostedFileBase Cer { get; set; }

        [Required]
        [DisplayName("Archivo key")]
        public HttpPostedFileBase Key { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage ="La clave privada no debe superar los 100 caracteres")]
        [DisplayName("Clave privada")]
        public string PrivateCode { get; set; }
    }
}