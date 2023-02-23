using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Business.Cancel
{
    public class CancelInvoice
    {
        string UserPac = "", PasswordPac="", UUID="", PFXPassword="", RFCEmisor="", RFCReceptor="";
        byte[] PFX;
        double Total;
        string Motivo, Sustitucion;

        public CancelInvoice(string UserPac, string PasswordPac, string UUID, string PFXPassword, byte[] PFX,
            double Total, string RFCEmisor, string RFCReceptor, string motivo, string sustitucion)
        {
            this.UserPac = UserPac;
            this.PasswordPac = PasswordPac;
            this.UUID = UUID;
            this.PFXPassword = PFXPassword;
            this.PFX = PFX;
            this.Total = Total;
            this.RFCEmisor = RFCEmisor;
            this.RFCReceptor = RFCReceptor;
            this.Motivo = motivo;
            this.Sustitucion = sustitucion;
        }

        public void Cancel()
        {
            ServiceReference1.TimbradoClient oClient = new ServiceReference1.TimbradoClient();
            ServiceReference1.RespuestaCFDi oResponse=
                oClient.CancelarAsincrono(UserPac, PasswordPac, PFX,  UUID , PFXPassword, Total, RFCEmisor,RFCReceptor, Motivo, Sustitucion);

            if (oResponse.Documento == null)
            {
                throw new Exception(oResponse.Mensaje);
            }
        }
    }
}