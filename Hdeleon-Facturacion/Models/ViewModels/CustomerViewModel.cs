using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Razón social")]
        [StringLength(maximumLength: 200, ErrorMessage = "La razón social no debe tener más de 200 caracteres")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Nombre comercial")]
        [StringLength(maximumLength: 100, ErrorMessage = "El nombre comercial no debe tener más de 100 caracteres")]
        public string CommercialName { get; set; }
        [Required]
        [RegularExpression(@"^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$",ErrorMessage = "Formato de RFC Invalido")]
        [Display(Name = "RFC")]
        public string RFC { get; set; }
        [Display(Name = "Código postal")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El código postal debe ser numérico")]
        [StringLength(maximumLength: 5, ErrorMessage = "El código postal no debe tener más de 5 caracteres")]
        public string CP { get; set; }
        [Display(Name = "Calle")]
        [StringLength(maximumLength: 500, ErrorMessage = "La calle no debe tener más de 500 caracteres")]
        public string Street { get; set; }
        [Display(Name = "Municipio")]
        [Required]
        public int IdMunicipio { get; set; }
        [Display(Name = "Estado")]
        [Required]
        public int idEstado { get; set; }
        [Display(Name = "Número exterior")]
        [StringLength(maximumLength: 100, ErrorMessage = "El número exterior no debe tener más de 10 caracteres")]
        public string NoExt { get; set; }
        [Display(Name = "Número interior")]
        [StringLength(maximumLength: 10, ErrorMessage = "El número interior no debe tener más de 10 caracteres")]
        public string NoInt { get; set; }

        [Required]
        [Display(Name = "Régimen fiscal")]
        public string CodeTaxRegime { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="Correo electrónico")]
        [StringLength(maximumLength:100, ErrorMessage ="El correo electrónico no debe tener más de 100 caracteres")]
        public string Email { get; set; }
    }
}