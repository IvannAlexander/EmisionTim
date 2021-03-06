using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using log4net;
using EmisionService;
using Contract;
using Model;
using System.Xml.Linq;
using System.Configuration;
using System.IO;

namespace Bussines
{
    public class CertificateOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(CertificateOperation));

        public string SaveCertificate(byte[] fileCer, byte[] fileKey, string pass, string rfcCompany, long idCompany)
        {
            try
            {
                var cerBase64 = Convert.ToBase64String(fileCer);
                var keyBase64 = Convert.ToBase64String(fileKey);
                var certificado = new X509Certificate2(fileCer, pass);
                ValidateCsd(certificado.SubjectName.Name);
                string rfc = GetRfc(certificado.Subject);
                var nombreCer = GetName(certificado.IssuerName.Name);
                if (rfc != rfcCompany)
                {
                    return $"El RFC {rfc} del certificado no corredponde al RFC de la empresa. Favor de validar.";
                }
                using (var db = new Db_EmisionEntities())
                {
                    var numCert = GetNumberCertificate(certificado.SerialNumber);
                    var cert = db.Sys_Certificate.FirstOrDefault(p => p.Sys_Number == numCert && p.Sys_ExpirationDateCert == certificado.NotAfter);
                    if (cert != null)
                    {
                        return "El cetificado ya existe en la base de datos.";
                    }
                    var sysCertificado = new Sys_Certificate
                    {
                        Sys_RegistrationDateCert = certificado.NotBefore,
                        Sys_RegistrationDate = DateTime.Now,
                        Sys_ExpirationDateCert = certificado.NotAfter,
                        Sys_Number = numCert,
                        Sys_Key = keyBase64,
                        Sys_Pwd = EmisionService.CommonOperation.Encrypt(pass),
                        Sys_Certificate1 = cerBase64,
                        Sys_Name = nombreCer.Trim(),
                        Sys_Rfc = rfcCompany,
                        Sys_IdCompany = idCompany
                    };
                    db.Sys_Certificate.Add(sysCertificado);
                    db.SaveChanges();
                }
                return string.Empty;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                return ee.Message;
            }
        }

        private string GetName(string subject)
        {
            var partes = subject.Split(',');
            var resp = partes.FirstOrDefault(p => p.Trim().ToString().StartsWith("CN"));

            return resp?.Replace("CN=", "") ?? string.Empty;
        }

        private void ValidateCsd(string subject)
        {
            var partes = subject.Split(',');
            var resp = partes.FirstOrDefault(p => p.Trim().ToString().StartsWith("OU"));
            if (string.IsNullOrEmpty(resp))
            {
                throw new Exception("El archivo .cer no corresponde a un archivo CSD.");
            }
        }

        private string GetRfc(string subject)
        {
            var split = subject.Split(',');
            var resp = split.FirstOrDefault(p => p.Trim().ToString().StartsWith("OID.2.5.4.45"));
            var rfcs = resp?.Replace("OID.2.5.4.45=", "") ?? string.Empty;
            var part = rfcs.Split('/');
            if (part != null && part.Count() > 0)
            {
                return part[0].Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetNumberCertificate(string numeroCert)
        {
            try
            {
                var resp = string.Empty;
                for (int i = 0; i < numeroCert.Length; i++)
                {
                    if ((i % 2) == 1)
                    {
                        resp += numeroCert.Substring(i, 1);
                    }
                }
                return resp;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("Error al convertir número a certificado.");
            }
        }

        public List<CertificateDto> GetCertificateByRfc(string rfc)
        {
            try
            {
                //var pagina = paginaActual <= 0 ? 0 : paginaActual - 1;
                using (var db = new Db_EmisionEntities())
                {
                    var list = new List<CertificateDto>();
                    var certs = db.Sys_Certificate.Where(p => p.Sys_Rfc == rfc).OrderByDescending(p => p.Sys_RegistrationDate).ToList();//.Skip(pagina).Take(numRegistros).ToList();
                    certs.ForEach(p => list.Add(Common.Map<Sys_Certificate, CertificateDto>(p)));
                    return list;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                Logger.Error(ee.StackTrace);
                if (ee.InnerException != null)
                {
                    Logger.Error(ee.InnerException.Message);
                }
                return null;
            }
        }

        public string CreateCfdi(string xml, long idCompany)
        {
            try
            {
                var serializacion = new SerializationOperation();
                var cfdi = serializacion.Deserializar<Comprobante>(xml);
                var operacionesEmision = new EmisionOperation();
                var cert = new CertificateDto();
                using (var context = new Db_EmisionEntities())
                {
                    var certDb = context.Sys_Certificate.OrderByDescending(p => p.Sys_ExpirationDateCert).FirstOrDefault(p => p.Sys_Rfc == cfdi.Emisor.Rfc);
                    cert = Common.Map<Sys_Certificate, CertificateDto>(certDb);
                }
                var passCert = EmisionService.CommonOperation.Decrypt(cert.Sys_Pwd);
                var timbre = "";
                var answer = operacionesEmision.GeneraCfdi33(cfdi, cert.Sys_Number, cert.Sys_Certificate1, cert.Sys_Key, passCert, ref timbre);
                if (answer != null)
                {
                    var operacionesSerializacion = new EmisionOperation();
                    var xmlTim = XDocument.Parse(timbre);
                    var timbreFiscal = operacionesSerializacion.Deserializar<TimbreFiscalDigital>(xmlTim.ToString());
                    var fecha = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(cfdi.Fecha));
                    var path = Path.Combine(ConfigurationManager.AppSettings["Invoice"], fecha, cfdi.Emisor.Rfc);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var t = answer.ToString();
                    var fileName = Path.Combine(path, $"{cfdi.Emisor.Rfc}_{cfdi.Receptor.Rfc}_{timbreFiscal.UUID}.xml");
                    File.WriteAllText(fileName, t);
                    //Save in databse
                    SaveInvoice(cfdi, idCompany, timbreFiscal.UUID);
                }
                return answer.ToString();
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                Logger.Error(ee.StackTrace);
                if (ee.InnerException != null)
                {
                    Logger.Error(ee.InnerException.Message);
                }
                return "Error: No se genero el XML.";
            }
        }

        private void SaveInvoice(Comprobante comprobante, long idCompany, string uuid)
        {
            try
            {
                

                InvoiceDto invoice = new InvoiceDto
                {
                    Sys_Folio = comprobante.Folio,
                    Sys_FechaCaptura = DateTime.Now,
                    Sys_FechaRegistro = Convert.ToDateTime(comprobante.Fecha),
                    Sys_Cancelado = false,
                    Sys_CondicionPago = comprobante.CondicionesDePago,
                    Sys_FormaPago = comprobante.FormaPago,
                    Sys_IdCompany = idCompany,
                    Sys_LugarExpedicion = comprobante.LugarExpedicion,
                    Sys_MetodoPago = comprobante.MetodoPago,
                    Sys_Moneda = comprobante.Moneda,
                    Sys_NoCertificado = comprobante.NoCertificado,
                    Sys_NombreEmisor = comprobante.Emisor.Nombre,
                    Sys_NombreReceptor = comprobante.Receptor.Nombre,
                    Sys_RegimenFiscal = comprobante.Emisor.RegimenFiscal,
                    Sys_RfcEmisor = comprobante.Emisor.Rfc,
                    Sys_RfcReceptor = comprobante.Receptor.Rfc,
                    Sys_Serie = comprobante.Serie,
                    Sys_Subtotal = comprobante.SubTotal,
                    Sys_TipoComprobante = comprobante.TipoDeComprobante,
                    Sys_Total = comprobante.Total,
                    Sys_UsoCfdi = comprobante.Receptor.UsoCFDI,
                    Sys_TipoCambio = comprobante.TipoCambio,
                    Sys_Descuento = comprobante.Descuento,
                    Sys_TimbreFiscal = uuid
                };

                //Save info
                using (var context = new Db_EmisionEntities())
                {
                    var invoicedb = Common.Map<InvoiceDto, Sys_Invoice>(invoice);
                    context.Sys_Invoice.Add(invoicedb);
                    context.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                Logger.Error(ee.StackTrace);
                if (ee.InnerException != null)
                {
                    Logger.Error(ee.InnerException.Message);
                }
            }
        }


        public byte[] CreateCfdiPdf(string xml, long idCompany)
        {
            try
            {
                var cfdi = CreateCfdi(xml, idCompany);
                if (!cfdi.StartsWith("Error"))
                {
                    PdfOperation pdfOperation = new PdfOperation();
                    var pdf = pdfOperation.GenerarPdfFactura(cfdi);
                    return pdf;
                }
                return null;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                Logger.Error(ee.StackTrace);
                if (ee.InnerException != null)
                {
                    Logger.Error(ee.InnerException.Message);
                }
                return null;
            }
        }
    }
}
