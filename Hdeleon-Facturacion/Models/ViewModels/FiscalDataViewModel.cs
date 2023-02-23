using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class FiscalDataViewModel
    {
        [DisplayName("RFC")]
        [Required]
        [StringLength(maximumLength: 13, ErrorMessage = "El RFC no debe tener más de 13 caracteres")]
        [RegularExpression(@"^([A-Z,Ñ,&]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])[A-Z|\d]{3})$", ErrorMessage = "Formato de RFC Invalido")]
        public string RFC { get; set; }
        [DisplayName("Correo electrónico")]
        [EmailAddress]
        [StringLength(maximumLength: 100, ErrorMessage = "El correo electrónico no debe tener más de 100 caracteres")]
        public string Email { get; set; }
        [DisplayName("Razón social")]
        [Required]
        [StringLength(maximumLength: 200, ErrorMessage = "La razón social no debe tener más de 200 caracteres")]
        public string FiscalName { get; set; }
        [Required]
        [DisplayName("Regimen fiscal")]
        public string CodeTaxRegime { get; set; }
        [DisplayName("Usuario PAC")]
        public string UserPacFC { get; set; }
        [DisplayName("Contraseña PAC")]
        public string PassPacFC { get; set; }

        [DisplayName("Código postal")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El código postal debe ser numérico")]
        public string CP { get; set; }


    }
}