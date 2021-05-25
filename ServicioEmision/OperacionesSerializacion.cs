using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net;

namespace ServicioEmision
{
    public class OperacionesSerializacion
    {

        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionesSerializacion));

        public XDocument Serializar<T>(T objetoCfdi)
        {
            try
            {
                var ser = new XmlSerializer(typeof(T));
                using (var memStream = new MemoryStream())
                {
                    var sw = new StreamWriter(memStream, Encoding.UTF8);
                    using (var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                    {
                        var tipo = typeof (T);
                        var namespaces = ObtenerNamespaces(tipo.Name);
                        ser.Serialize(xmlWriter, objetoCfdi, namespaces);
                        var xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                        var comp = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", string.Empty);
                        var doctoComprobante = XDocument.Parse(comp);
                        return doctoComprobante;
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se serializo correctamente.");
            }
        }

        public T Deserializar<T>(string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var xmlRed = new XmlReaderSettings();
                using (var strReader = new StringReader(xml))
                {
                    using (var xmlReader = XmlReader.Create(strReader, xmlRed))
                    {
                        var xmlResp = (T)serializer.Deserialize(xmlReader);
                        return xmlResp;
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se deserealizo correctamente.");

            }
        }

        private static XmlSerializerNamespaces ObtenerNamespaces(string tipo)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            if (tipo == "Nomina12")
            {
                namespaces.Add("nomina12", "http://www.sat.gob.mx/nomina12");
            }
            namespaces.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
            return namespaces;
        }

    }
}
