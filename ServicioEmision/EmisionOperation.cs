using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Contract;
using log4net;
using Model;

namespace EmisionService
{
    public class EmisionOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(EmisionOperation));

        

        /// <summary>
        /// Método encargado de generar el CFDI.
        /// </summary>
        /// <param name="comprobante">Objeto tipo comprobante lleno para serializarlo.</param>
        /// <param name="noCertificado">Número del certificado en claro.</param>
        /// <param name="certificado">Certificado en base64.</param>
        /// <param name="key">Key en base64.</param>
        /// <param name="passCert">Contraseña del certificado en claro.</param>
        /// <returns>Regresa CFDI en objeto XDocument.</returns>
        public XDocument GeneraCfdi33(Comprobante comprobante, string noCertificado, string certificado, string key, string passCert, ref string timbreXml)
        {
            try
            {
                var serializacion = new SerializationOperation();
                comprobante.NoCertificado = noCertificado;
                comprobante.Certificado = certificado;
                if (comprobante.Complemento != null)
                {
                    comprobante.Complemento.Add(new ComprobanteComplemento());
                }
                var doctoComprobante = SerializarDocumentos(comprobante, string.Empty);
                if (comprobante.ExisteComplemento)
                {
                    var xmlComp = SerializarDocumentos(comprobante, comprobante.TipoComplemento);
                    var operacionComplemento = new ComplementsOperation();
                    doctoComprobante = operacionComplemento.AgregarComplemento(doctoComprobante, xmlComp);
                }
                var cadenaOriginal = ObtenerCadenaOriginal(doctoComprobante, false);
                var cfdi = FirmarCfdi(doctoComprobante, cadenaOriginal, key, passCert);
                //Obtener timbre XML
                var operacionesTimbrado = new ServicioTimbrado.OperacionesTimbrado();

                var timbre = operacionesTimbrado.TimbrarXml(cfdi);
                if (timbre.StartsWith("Error"))
                {
                    Logger.Error(timbre);
                    return null;
                }
                timbreXml = timbre;
                //Adiciona timbre a CFDI
                return PegarTimbre(cfdi.ToString(), timbre, serializacion);
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se genero el XML 3.3 correctamente.");
            }
        }

        private XDocument PegarTimbre(string cfdi, string timbre, SerializationOperation serializacion)
        {
            try
            {
                
                var comprobante = XDocument.Parse(cfdi);
                var xmlComp = XDocument.Parse(timbre);
                var operacionComplemento = new ComplementsOperation();
                var comp = operacionComplemento.AgregarComplemento(comprobante, xmlComp);
                return comp;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception(ee.Message);
            }
        }

        public XDocument FirmarCfdi(XDocument comprobante, string cadenaOriginal, string key, string passCert)
        {
            try
            {
                var operacionesCrypto = new CryptoOperation();
                var firma = operacionesCrypto.SignString(key, passCert, cadenaOriginal);
                var cfdi = DeserializarDocumentos(comprobante.ToString());
                cfdi.Sello = firma;
                return SerializarDocumentos(cfdi, string.Empty);
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception(ee.Message);
            }
        }

        public string ObtenerCadenaOriginal(XDocument comprobante, bool esTimbre)
        {
            try
            {
                var dirXslt = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xslt");
                StreamReader xslr = null;
                if (!esTimbre)
                {
                    xslr = new StreamReader(Path.Combine(dirXslt, "cadenaoriginal_3_3.xslt"));
                }
                else
                {
                    xslr = new StreamReader(Path.Combine(dirXslt, "cadenaoriginal_TFD_1_1.xslt"));
                }
                var xslRdr = System.Xml.XmlReader.Create(xslr);
                var nav = comprobante.CreateNavigator();
                var xslCompiledTransform = new XslCompiledTransform();
                xslCompiledTransform.Load(xslRdr);
                var sbCadena = new StringBuilder();
                var stringWriter = new StringWriter(sbCadena);
                xslCompiledTransform.Transform(nav, null, stringWriter);
                return sbCadena.ToString();
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se genero la cadena original correctamente.");
            }
        }

        public XDocument SerializarDocumentos(Comprobante comprobante, string tipoComplemento)
        {
            try
            {
                var serializacion = new SerializationOperation();
                XDocument xmlComp = null;
                switch (tipoComplemento)
                {
                    case "Nomina12":
                        xmlComp = serializacion.Serializar(comprobante.Nomina12);
                        break;
                    case "ValesDeDespensa":
                        xmlComp = serializacion.Serializar(comprobante.ValesDespensa);
                        break;
                    default:
                        xmlComp = serializacion.Serializar<Comprobante>(comprobante);
                        break;
                }
                return xmlComp;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception(ee.Message);
            }
        }

        public Comprobante DeserializarDocumentos(string comprobante)
        {
            try
            {
                var serializacion = new SerializationOperation();
                return serializacion.Deserializar<Comprobante>(comprobante);
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception(ee.Message);
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
                throw new Exception("No se deserealizo correctamente.");

            }
        }

    }
}
