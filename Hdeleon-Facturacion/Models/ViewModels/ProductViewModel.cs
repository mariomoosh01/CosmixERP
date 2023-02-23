using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Hdeleon_Facturacion.Models;

namespace Hdeleon_Facturacion.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500,ErrorMessage ="La descripción no debe superar los 500 caracteres")]
        public string Descripcion { get; set; }

        [Required]
        [DisplayName("Clave de producto-servicio")]
        [ExistClaveProd]
        [MaxLength(10, ErrorMessage = "La clave de producto-servicio no debe tener más de 10 caracteres")]
        public string ClaveProdServ { get; set; }
        [Required]
        [DisplayName("Clave de unidad")]
        [ExistClaveUnidad]
        [MaxLength(10, ErrorMessage = "La clave de unidad no debe superar los 10 caracteres")]
        public string ClaveUnidad { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [DisplayName("Número de identificación")]
        [MaxLength(100, ErrorMessage = "El numéro de identificación no debe superar los 100 caracteres")]
        public string NoIdentificacion { get; set; }

        [DisplayName("Unidad")]
        [MaxLength(100, ErrorMessage = "La descripción no debe superar los 100 caracteres")]
        public string Unidad { get; set; }

        public class ExistClaveProdAttribute : ValidationAttribute
        {

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {

                string  clave= (string)value;
                using (Models.SAT.satEntities db = new Models.SAT.satEntities())
                {
                      if (db.f4_c_claveprodserv.Where(d =>d.c_ClaveProdServ == clave).Count() <= 0)
                            return new ValidationResult("La clave producto servicio no existe en el cátalogo del sat");
                }

                return ValidationResult.Success;
            }


        }
        public class ExistClaveUnidadAttribute : ValidationAttribute
        {

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {

                string clave = (string)value;
                using (Models.SAT.satEntities db = new Models.SAT.satEntities())
                {
                    if (db.f4_c_claveunidad.Where(d => d.c_ClaveUnidad == clave).Count() <= 0)
                        return new ValidationResult("La clave de unidad no existe en el cátalogo del sat");
                }

                return ValidationResult.Success;
            }


        }


    }
}