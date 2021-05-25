using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ServicioTimbrado
{
    public class WebService
    {
        public string Url { get; set; }
        public string MethodName { get; set; }
        public Dictionary<string, string> Params = new Dictionary<string, string>();
        public XDocument ResultXML;
        public string ResultString;
        public XmlDocument XmlResult;
        public XmlDocument XmlTimbre;
        public string Error { get; set; }

        public WebService()
        {

        }

        public WebService(string url, string methodName)
        {
            Url = url;
            MethodName = methodName;
        }

        /// <summary>
        /// Invokes service
        /// </summary>
        public void Invoke()
        {
            Invoke(true);
        }

        /// <summary>
        /// Invokes service
        /// </summary>
        /// <param name="encode">Added parameters will encode? (default: true)</param>
        public bool Invoke(bool encode)
        {
            bool ok = false;
            try
            {
                string soapStr =
                  @"<?xml version=""1.0"" encoding=""UTF-8""?>
                          <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                          xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                          xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                          <soap:Body>
                          <{0} xmlns=""http://tempuri.org/"">
                          {1}
                          </{0}>
                          </soap:Body>
                          </soap:Envelope>";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Headers.Add("SOAPAction", "\"http://tempuri.org/" + MethodName + "\"");
                req.ContentType = "text/xml;charset=\"utf-8\"";
                req.Accept = "text/xml";
                req.Method = "POST";

                using (Stream stm = req.GetRequestStream())
                {
                    string postValues = "";
                    foreach (var param in Params)
                    {
                        if (encode)
                        {

                            postValues += string.Format("<{0}>{1}</{0}>", HttpUtility.HtmlEncode(param.Key), HttpUtility.HtmlEncode(param.Value));

                        }
                        else
                            postValues += string.Format("<{0}>{1}</{0}>", param.Key, param.Value);
                    }

                    soapStr = string.Format(soapStr, MethodName, postValues);
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(soapStr);
                    }
                }

                using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string result = responseReader.ReadToEnd();
                    XmlResult = new XmlDocument();
                    XmlResult.LoadXml(result);
                    //XmlResult.InnerXml = (HttpUtility.HtmlDecode(result));

                    XmlNode nodo1 = GetUltimoNodo(XmlResult.DocumentElement);

                    XmlResult.InnerXml = (HttpUtility.HtmlDecode(nodo1.InnerText));

                    XmlTimbre = new XmlDocument();
                    //Buscamos el tag timbre
                    if (XmlResult.DocumentElement.Name == "tfd:TimbreFiscalDigital")
                    {
                        XmlTimbre.InnerXml = XmlResult.InnerXml;
                        ok = true;
                    }
                    else
                    {
                        XmlNodeList nodos = XmlResult.DocumentElement.GetElementsByTagName("TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital");
                        if (nodos.Count > 0)
                        {
                            XmlTimbre.InnerXml = nodos[0].OuterXml;
                            ok = true;
                        }
                        else
                        {
                            //recorremos el nodos hasta encontrar el último, para ponerlo como error
                            XmlNode nodo = GetUltimoNodo(XmlResult.DocumentElement.FirstChild);

                            throw new Exception(nodo.OuterXml);
                        }
                    }

                    //Removemos la declaracion en caso de trarela...
                    foreach (XmlNode node in XmlTimbre)
                    {
                        if (node.NodeType == XmlNodeType.XmlDeclaration)
                        {
                            XmlTimbre.RemoveChild(node);
                            break;
                        }
                    }
                    
                    req = null;

                    return ok;
                }

            }
            catch (Exception ex)
            {
                Error = "Error al timbrar en metodo: " + MethodName + " url: " + Url + " error : " + ex.Message.ToString();
                return ok;
            }
        }

        XmlNode GetUltimoNodo(XmlNode nodo)
        {
            XmlNode nodox = nodo;
            if (nodo.HasChildNodes)
            {
                foreach (XmlNode node in nodo.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        nodox = GetUltimoNodo(node);
                    }
                }
            }
            return nodox;

        }
    }
}
