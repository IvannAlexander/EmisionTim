using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using log4net;

namespace ServicioEmision
{
    public class OperacionesEmision
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionesEmision));

        /// <summary>
        /// Método encargado de generar el CFDI.
        /// </summary>
        /// <param name="comprobante">Objeto tipo comprobante lleno para serializarlo.</param>
        /// <param name="noCertificado">Número del certificado en claro.</param>
        /// <param name="certificado">Certificado en base64.</param>
        /// <param name="key">Key en base64.</param>
        /// <param name="passCert">Contraseña del certificado en claro.</param>
        /// <returns>Regresa CFDI en objeto XDocument.</returns>
        public XDocument GeneraCfdi33(Comprobante comprobante, string noCertificado, string certificado, string key, string passCert)
        {
            try
            {
                var serializacion = new OperacionesSerializacion();
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
                    var operacionComplemento = new OperacionesComplementos();
                    doctoComprobante = operacionComplemento.AgregarComplemento(doctoComprobante, xmlComp);
                }
                var cadenaOriginal = ObtenerCadenaOriginal(doctoComprobante, false);
                var cfdi = FirmarCfdi(doctoComprobante, cadenaOriginal, key, passCert);
                //Obtener timbre XML
                var operacionesTimbrado = new ServicioTimbrado.OperacionesTimbrado();
                var timbre = operacionesTimbrado.TimbrarXml(cfdi);
                //Adiciona timbre a CFDI
                return PegarTimbre(cfdi.ToString(), timbre, serializacion);
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se genero el XML 3.3 correctamente.");
            }
        }

        private XDocument PegarTimbre(string cfdi, string timbre, OperacionesSerializacion serializacion)
        {
            try
            {
                var comprobante = XDocument.Parse(cfdi);
                var xmlComp = XDocument.Parse(timbre);
                var operacionComplemento = new OperacionesComplementos();
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
                var operacionesCrypto = new OperacionesCrypto();
                var firma = operacionesCrypto.SignString(key, passCert, cadenaOriginal);
                var cfdi = DeserializarDocumentos(comprobante);
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

        private XDocument SerializarDocumentos(Comprobante comprobante, string tipoComplemento)
        {
            try
            {
                var serializacion = new OperacionesSerializacion();
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

        private Comprobante DeserializarDocumentos(XDocument comprobante)
        {
            try
            {
                var serializacion = new OperacionesSerializacion();
                return serializacion.Deserializar<Comprobante>(comprobante.ToString());
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception(ee.Message);
            }
        }



    }
}
