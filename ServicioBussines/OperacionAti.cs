using log4net;
using ServicioContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioBussines
{
    public class OperacionAti : BaseObject
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionAti));

        public void GenerarFacturaAti(byte[] archivoAti)
        {
            try
            {
                var archivo = System.Text.Encoding.UTF8.GetString(archivoAti);
                var correos = new List<string>();
                var archivoProcesado = string.Empty;
                var tipoCfdi = string.Empty;
                var comprobante = new Comprobante();
                var valesDespensa = new ValesDeDespensa();
                foreach (var linea in archivo.Split('\n'))
                {
                    var atributo = string.Empty;
                    var valor = string.Empty;
                    int indice = linea.IndexOf('\t');
                    if (indice < 0)
                    {
                        indice = linea.IndexOf(" ");
                    }
                    if (indice < 0)
                    {
                        atributo = linea;
                    }
                    else
                    {
                        atributo = linea.Substring(0, indice).Trim();
                        valor = linea.Substring(indice, linea.Length - indice).Trim();
                    }
                    if (atributo == "cuentaEmailEnvio")
                    {
                        correos.Add(valor);
                        continue;
                    }
                    if (atributo == "nombreArchivoProcesado")
                    {
                        archivoProcesado = valor;
                        continue;
                    }
                    if (atributo == "tipoComplemento")
                    {
                        tipoCfdi = valor;
                        continue;
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception(ee.Message);
            }
        }
        
    }
}
