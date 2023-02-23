using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Business.Timbrado
{
    public abstract class PacTrimbrar
    {

        public string Error { get; set; }
        public string UserPac { get; set; }
        public string PassPac { get; set; }
        
        public byte[] XMLTimbrado { get; set; }
        public abstract void Timbrar(byte[] bXml);

    }
}