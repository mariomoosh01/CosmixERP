using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Business.FormatPDF
{
    public abstract class Format
    {
        protected string root { get; set; }
        protected string metodo = "";
        protected string nameController = "PDF";

        public Format(string root)
        {
            this.root = root;
        }

        public abstract void Create();


    }
}