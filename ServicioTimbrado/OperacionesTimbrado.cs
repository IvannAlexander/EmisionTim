using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServicioTimbrado
{
    
    public class OperacionesTimbrado
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionesTimbrado));

        public string TimbrarXml(XDocument cfdi)
        {
            return ObtenerTimbrarMasterEdi(cfdi.ToString());
        }


        private string ObtenerTimbrarMasterEdi(string xml)
        {
            try
            {
                //var urlTimbrado = ConfigurationManager.AppSettings["TimbradoMasteredi"];
                var urlTimbrado = "http://timbradodev.masfactura.com.mx/Timbrado/TimbradoCFDServiceExternal.asmx";
                var ws = new WebService(urlTimbrado, "TimbradoCFDStrXML");
                ws.Params.Add("strXML", xml);
                ws.Params.Add("strUser", "Post7342");
                ws.Params.Add("strPass", "masteredi1#");
                var resp = ws.Invoke(true);
                if (resp)
                {
                    return ws.XmlTimbre.InnerXml;
                }
                else
                {
                    return "Error: " + ws.Error;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("Error al timbrar CFDI Masteredi.");
            }
        }

    }
}
