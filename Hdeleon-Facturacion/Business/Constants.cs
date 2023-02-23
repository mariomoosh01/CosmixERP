using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hdeleon_Facturacion.Business
{
    public class Constants
    {
        public static string LengthMenu
        {
            get { return "Mostrando _MENU_ registros por página"; }
        }
        public static string ZeroRecords
        {
            get { return "No se encuentran datos"; }
        }
        public static string Info
        {
            get { return "Mostrando página _PAGE_ de _PAGES_"; }
        }
        public static string InfoEmpty
        {
            get { return "No existen datos"; }
        }
        public static string InfoFiltered
        {
            get { return "(filtrado de _MAX_ total de registros)"; }
        }
        public static string Previous
        {
            get { return "Anterior"; }
        }
        public static string Next
        {
            get { return "Siguiente"; }
        }

        public static string ExcepcionMessage = "Ocurrio un error: ";

        public const string PathCer = "Files/Sello.cer";
        public const string PathKey = "Files/Sello.key";
        public const string PathPfx = "Files/Sello.pfx";
        public const string PathLogo = "Files/Logo.png";
        public static class AMBITO
        {
            public const string FEDERAL = "FEDERAL";
            public const string LOCAL = "LOCAL";
        }
        public static class TIPO_IMPUESTO
        {
            public const string TRASLADO = "Traslado";
            public const string RETENCION = "Retención";
        }
        public static bool TEST{
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["TEST"] == "1") //1 es prueba, 2 es producción
                        return true;
                    return false;
                }
                catch { return false; }
            }
        }

        public static int PAC
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["Pac"].ToString());
                }
                catch { return 1; //por defecto
                }
            }
        }

    }
}