using System;
using System.Linq;
using System.Web.Mvc;
using Hdeleon_Facturacion.Models;
using Hdeleon_Facturacion.Utils;

namespace Hdeleon_Facturacion.Controllers
{
    public class AccessController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email,string password)
        {
            try
            {
                using (Entities db = new Entities())
                {
                    string ePass = Utils.Encrypt.GetSHA1(password);
                    var lstUser = from d in db.user
                                where d.email == email && d.idState == 1
                                && d.password==ePass
                                select d;

                    if (lstUser.Count() > 0)
                    {
                        var oUser = lstUser.First();
                        
                        Session["User"] = oUser;                        
                        GlobalParams.User = oUser;

                        return Content("1");
                    }else
                    {
                        return Content("Usuario o contraseña invalido");
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("Ocurrio un error");
            }
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session["User"] = null;

            return Content("1");
        }
    }
}