using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Hdeleon_Facturacion.Controllers
{
    public class BaseCrudController<Lista> : BaseController
    {
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        public List<Lista> data = null;

        /// <summary>
        /// Prepara el listado para ser utilizado en datatable de bootstrap/jquery
        /// </summary>
        /// <param name="lst"></param>
        public void GetParamsTable(IQueryable<Lista> lst)
        {

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;

           // using (Models.facturacionEntities db = new Models.facturacionEntities())
            {

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    lst = lst.OrderBy(sortColumn + " " + sortColumnDir);
                }
                //Search    
                /*if (!string.IsNullOrEmpty(searchValue))
                {
                    lst = lst.Where(m => m.CompanyName == searchValue);
                }*/

                recordsTotal = lst.Count();

                data = lst.Skip(skip).Take(pageSize).ToList();

            }

        }

    }
}
