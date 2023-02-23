using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class PasswordViewModel
    {
        [Required]
        [MaxLength(100,ErrorMessage ="La nueva contraseña no debe superar los 100 caracteres")]
        [DisplayName("Nueva contraseña")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [MaxLength(100, ErrorMessage = "El confirmar contraseña no debe superar los 100 caracteres")]
        [DisplayName("Confirmar contraseña")]
        public string Password2 { get; set; }
    }
}