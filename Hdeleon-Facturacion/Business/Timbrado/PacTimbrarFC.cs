using Hdeleon_Facturacion.ServiceReference1;
using System;

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

            RespuestaCFDi respuestaCFDI = new RespuestaCFDi();

            TimbradoClient oTimbrado = new TimbradoClient();

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