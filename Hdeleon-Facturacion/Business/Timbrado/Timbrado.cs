using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Business.Timbrado
{
    public class Timbrado : PacTrimbrar
    {
        public Timbrado(string UserPac, string PassPac)
        {
            this.UserPac = UserPac;
            this.PassPac = PassPac;
        }
       
        public override void Timbrar(byte[] bXml)
        {
            //Por si hay más pac agregar los case
            switch (Business.Constants.PAC){
                case 1: //FC Pac
                    {
                        PacTimbrarFC oPacTimbrarFC = new PacTimbrarFC(UserPac,PassPac);
                        oPacTimbrarFC.Timbrar(bXml);
                        XMLTimbrado = oPacTimbrarFC.XMLTimbrado;
                    }
                    break;
            }
        }
    }
}