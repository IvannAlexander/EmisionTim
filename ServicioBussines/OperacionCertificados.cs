using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ServicioContract;
using ServicioEmision;

namespace ServicioBussines
{
    public class OperacionCertificados : BaseObject
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionUsuario));

        public void GuardaCertificados(byte[] archivoCer, byte[] archivoKey, string pass, string rfcEmpresa)
        {
            try
            {
                var cerBase64 = Convert.ToBase64String(archivoCer);
                var keyBase64 = Convert.ToBase64String(archivoKey);
                var certificado = new X509Certificate2(archivoCer, pass);
                
                ValidarCsd(certificado.SubjectName.Name);
                string rfc = ObtenerRfc(certificado.Subject);
                var nombreCer = ObtenerNombre(certificado.IssuerName.Name);
                if (rfc != rfcEmpresa)
                {
                    throw new Exception("El RFC " + rfc + " del certificado no corredponde al RFC de la empresa. Favor de validar.");
                }
                using (var db = new Entities())
                {
                    var numCert = ObtenerNumeroCertificado(certificado.SerialNumber);
                    var cert = db.Sys_Certificado.FirstOrDefault(p=>p.Sys_Numero == numCert);
                    if (cert != null)
                    {
                        throw new Exception("El cetificado ya existe en la base de datos.");
                    }
                    //var emp = db.Sys_Empresa.FirstOrDefault(p => p.Sys_Rfc == rfcEmpresa);
                    var sysCertificado = new Sys_Certificado
                    {
                        Sys_Fec_Alta = certificado.NotBefore,
                        Sys_Fec_Registro = DateTime.Now,
                        Sys_Fec_Vencimiento = certificado.NotAfter,
                        Sys_Numero = numCert,
                        Sys_Key = keyBase64,
                        Sys_Pwd = OperacionesComun.Encrypt(pass),
                        Sys_Certificado1 = cerBase64,
                        Sys_Nombre = nombreCer.Trim(),
                        Sys_Rfc = rfcEmpresa
                    };
                    db.Sys_Certificado.Add(sysCertificado);
                    db.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                //throw new Exception("No se guardo el certificado correctamente. Valide que el archivo sea CSD.");
                throw new Exception(ee.Message);
            }
        }

        private string ObtenerNombre(string subject)
        {
            var partes = subject.Split(',');
            var resp = partes.FirstOrDefault(p => p.Trim().ToString().StartsWith("CN"));
            
            return resp?.Replace("CN=", "") ?? string.Empty;
        }

        private void ValidarCsd(string subject)
        {
            var partes = subject.Split(',');
            var resp = partes.FirstOrDefault(p => p.Trim().ToString().StartsWith("OU"));
            if (string.IsNullOrEmpty(resp))
            {
                throw new Exception("El archivo .cer no corresponde a un archivo CSD.");
            }
        }

        private string ObtenerRfc(string subject)
        {
            var partes = subject.Split(',');
            var resp = partes.FirstOrDefault(p => p.Trim().ToString().StartsWith("OID.2.5.4.45"));
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

        private static string ObtenerNumeroCertificado(string numeroCert)
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

        public List<Sys_Certificado> ObtenerCertificadosPorRfc(string rfc, int numRegistros, int paginaActual)
        {
            try
            {
                var pagina = paginaActual <= 0 ? 0 : paginaActual - 1;
                using (var db = new Entities())
                {
                    var lista = db.Sys_Certificado.Where(p => p.Sys_Rfc == rfc).OrderByDescending(p => p.Sys_Fec_Registro).Skip(pagina).Take(numRegistros).ToList();
                    return lista;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("Error al obtener los certificados.");
            }
        }

        public int CertificadosPorRfc(string rfc)
        {
            try
            {
                using (var db = new Entities())
                {
                    return db.Sys_Certificado.Where(p => p.Sys_Rfc == rfc).Count();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("Error al obtener los certificados.");
            }
        }

    }
}
