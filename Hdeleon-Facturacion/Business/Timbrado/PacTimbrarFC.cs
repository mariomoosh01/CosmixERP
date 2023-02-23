using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Business.Timbrado
{
    public class PacTimbrarFC : PacTrimbrar
    {
        public PacTimbrarFC(string UserPac, string PassPac)
        {
            this.UserPac = UserPac;
            this.PassPac = PassPac;
        }
        public override void Timbrar(byte[] bXml)
        {

            ServiceReference1.RespuestaCFDi respuestaCFDI = new ServiceReference1.RespuestaCFDi();

            ServiceReference1.TimbradoClient oTimbrado = new ServiceReference1.TimbradoClient();
            if(Business.Constants.TEST)
                respuestaCFDI = oTimbrado.TimbrarTest(UserPac,PassPac, bXml); //prueba
            else
                respuestaCFDI = oTimbrado.Timbrar(UserPac, PassPac, bXml); //producción

            if (respuestaCFDI.Documento == null)
                 throw new Exception(respuestaCFDI.Mensaje);
            else
                XMLTimbrado = respuestaCFDI.Documento;

        }
    }
}