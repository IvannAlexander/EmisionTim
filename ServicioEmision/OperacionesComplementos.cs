using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net;

namespace ServicioEmision
{
    public class OperacionesComplementos
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionesComplementos));

        public XDocument AgregarComplemento(XDocument comprobante, XDocument complemento)
        {
            try
            {
                var xmlOut = XElement.Parse(complemento.ToString());
                var nodoComplemento = comprobante.Descendants().ToList().FirstOrDefault(p => p.Name.LocalName == "Complemento");
                if (nodoComplemento != null)
                {
                    nodoComplemento.Add(xmlOut);
                    //comprobante = XDocument.Parse(nodoComplemento.ToString()); 
                }
                else
                {
                    var comp = comprobante.ToString().Replace("</cfdi:Comprobante>", "<cfdi:Complemento/></cfdi:Comprobante>");
                    comprobante = XDocument.Parse(comp);
                    AgregarComplemento(comprobante, complemento);
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se agrego correctamente el complemento al CFDI.");
            }
            return comprobante;
        }
    }
}
