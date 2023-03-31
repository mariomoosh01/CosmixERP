using Hdeleon_Facturacion.Models;
using System.Web.Mvc;

namespace Hdeleon_Facturacion.Controllers
{
    public class BaseController : Controller
    {
        private user user { get; set; }

        public user User => user;
        public void GetSession()
        {
            user = (user)Session["user"];   
        }               

        /// <summary>
        /// Obtiene los errores del modelo
        /// </summary>
        /// <returns></returns>
        public string getModelErrors()
        {
            string errores = "";
            foreach (ModelState modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errores += error.ErrorMessage + "<br>";
                }
            }

            return errores;
        }


    }
}