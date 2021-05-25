using log4net;
using EmisionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class ConvertTxtOperation
    {
        //protected static ILog Logger = LogManager.GetLogger(typeof(ConvertTxtOperation));
        //#region Valemex

        //public string ConvierteTxtAti(byte[] archivo, string rfc)
        //{
        //    try
        //    {
        //        var contenido = System.Text.UTF8Encoding.Default.GetString(archivo);
        //        var convertirTxt = new OperacionesConvertirTxt();
        //        var comprobante = convertirTxt.ConvertirTxt(contenido);
        //        var numCertificado = string.Empty;
        //        var certificado = string.Empty;
        //        var key = string.Empty;
        //        var passCert = string.Empty;
        //        using (var db = new Entities())
        //        {
        //            var sysCertificado = db.Sys_Certificado.OrderByDescending(p => p.Sys_Fec_Vencimiento).FirstOrDefault(p => p.Sys_Rfc == rfc);
        //            numCertificado = sysCertificado.Sys_Numero;
        //            certificado = sysCertificado.Sys_Certificado1;
        //            key = sysCertificado.Sys_Key;
        //            passCert = OperacionesComun.Decrypt(sysCertificado.Sys_Pwd);
        //        }
        //        //Genera CFDI
        //        var operacionesEmision = new EmisionOperation();
        //        var cfdi = operacionesEmision.GeneraCfdi33(comprobante, numCertificado, certificado, key, passCert);
        //        //Genera PDF

        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //    }
        //    return "";
        //}

        //#endregion Valemex
    }
}
