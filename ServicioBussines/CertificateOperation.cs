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

namespace Bussines
{
    public class CertificateOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(CertificateOperation));

        public string SaveCertificate(byte[] fileCer, byte[] fileKey, string pass, string rfcCompany)
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
                        Sys_Rfc = rfcCompany
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

        public string CreateCfdi(string xml)
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
                var answer = operacionesEmision.GeneraCfdi33(cfdi, cert.Sys_Number, cert.Sys_Certificate1, cert.Sys_Key, passCert);
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

        //public int CertificadosPorRfc(string rfc)
        //{
        //    try
        //    {
        //        using (var db = new Entities())
        //        {
        //            return db.Sys_Certificado.Where(p => p.Sys_Rfc == rfc).Count();
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("Error al obtener los certificados.");
        //    }
        //}

    }
}
