using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class LogoViewModel
    {
        [Required]
        [DisplayName("Archivo cer")]
        public HttpPostedFileBase Logo { get; set; }
    }
}