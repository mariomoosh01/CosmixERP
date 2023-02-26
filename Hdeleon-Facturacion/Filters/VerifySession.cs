using System.Web;
using System.Web.Mvc;
using Hdeleon_Facturacion.Controllers;
using Hdeleon_Facturacion.Models;

namespace Hdeleon_Facturacion.Filters
{
    public class VerifySession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            user oUser = (user)HttpContext.Current.Session["User"];

            if (oUser == null)
            {
                if(filterContext.Controller is AccessController == false)
                {
                    filterContext.HttpContext.Response.Redirect("/Access/Index");
                }
            }

            base.OnActionExecuting(filterContext);
        }

    }
}