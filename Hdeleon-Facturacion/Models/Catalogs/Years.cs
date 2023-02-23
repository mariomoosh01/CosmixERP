using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Models.Catalogs
{
    public class Years
    {
        //Año inicial para los catalogos
        public const int ORIGIN_YEAR = 2020;

        public static List<int> Get()
        {
            List<int> lst = new List<int>();
            for (int y = ORIGIN_YEAR; y <= DateTime.Now.Year; y++)
                lst.Add(y);
            return lst;
        }
    }
}