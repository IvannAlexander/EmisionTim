//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using log4net;
//using MessagingToolkit.QRCode.Codec;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

namespace EmisionService
{
    public class PdfOperation
    {
        //protected static ILog Logger = LogManager.GetLogger(typeof(PdfOperation));
        //public void GenerarPdf(XDocument cfdi)
        //{
        //    try
        //    {
        //        var serializacion = new SerializationOperation();
        //        var comprobante = serializacion.Deserializar<Comprobante>(cfdi.ToString());

        //        TimbreFiscalDigital timbreFiscal = null;
        //        XDocument xmlTim = null;
        //        if (comprobante.Complemento != null)
        //        {
        //            foreach (var complemento in comprobante.Complemento)
        //            {
                        
        //                var tim = complemento.Any.FirstOrDefault(p => p.LocalName == "TimbreFiscalDigital");

        //                if (tim != null)
        //                {
        //                    xmlTim = XDocument.Parse(tim.OuterXml);
        //                    timbreFiscal = serializacion.Deserializar<TimbreFiscalDigital>(xmlTim.ToString());
        //                }
        //            }
                    

        //        }


                
        //        var ultimosCaracteres = comprobante.Sello.Substring(comprobante.Sello.Length - 8);
        //        var qr = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?&id=" + timbreFiscal.UUID + "&re=" + comprobante.Emisor.Rfc + "&rr=" + comprobante.Receptor.Rfc + "&tt=" + comprobante.Total + "&fe=" + ultimosCaracteres;
        //        //comprobante.Qr = Convert.ToBase64String(GetQrCode(qr));

        //        comprobante.CantidadConLetra = AmountWithLetter.Enletras(comprobante.Total.ToString(), comprobante.Moneda);

        //        var operacionesEmision = new EmisionOperation();
        //        comprobante.CadenaOriginal = operacionesEmision.ObtenerCadenaOriginal(xmlTim, true);


        //        var rutaReporte = @"Z:\Alexander\Mega\Personal\Trabajo\Proyectos\Mvc\Emision\ServicioEmision\Pdf\GenericoCfdi.rpt";//System.IO.Path.Combine(directorio, "Reportes", "Factura.rpt");

        //        var rdocRpt = new ReportDocument();
        //        rdocRpt.Load(rutaReporte);

        //        var listaComprobante = new List<Comprobante> { comprobante };
        //        var listaEmisor = new List<ComprobanteEmisor> { comprobante.Emisor };
        //        var listaReceptor = new List<ComprobanteReceptor> { comprobante.Receptor };
        //        var listaTimbre = new List<TimbreFiscalDigital> { timbreFiscal };
        //        var listaImpuesto = new List<ComprobanteImpuestos> { comprobante.Impuestos };

        //        rdocRpt.Database.Tables["Comprobante"].SetDataSource(listaComprobante);
        //        rdocRpt.Database.Tables["ComprobanteEmisor"].SetDataSource(listaEmisor);
        //        rdocRpt.Database.Tables["ComprobanteReceptor"].SetDataSource(listaReceptor);
        //        rdocRpt.Database.Tables["TimbreFiscalDigital"].SetDataSource(listaTimbre);
                
        //        rdocRpt.Database.Tables["ComprobanteConcepto"].SetDataSource(comprobante.Conceptos.ToList());
        //        rdocRpt.Database.Tables["ComprobanteImpuestos"].SetDataSource(listaImpuesto);
        //        rdocRpt.Refresh();
        //        var ruataFactura = @"D:\Borrar\Test\" + timbreFiscal.UUID.ToUpper() + ".pdf";//System.IO.Path.Combine(directorioPdf, timbreFiscal.UUID.ToUpper() + ".pdf");
        //        GenerarPdf(rdocRpt, ruataFactura);
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("No se genero el PDF correctamente.");
        //    }
        //}

        //private byte[] GetQrCode(string cadena)
        //{
        //    Bitmap resultado = new Bitmap(20, 20);
        //    QRCodeEncoder encoder = new QRCodeEncoder();
        //    encoder.QRCodeVersion = 7;
        //    resultado = encoder.Encode(cadena);
        //    MemoryStream stream = new MemoryStream();
        //    resultado.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //    stream.Close();
        //    return stream.ToArray();
        //}

        //private static void GenerarPdf(ReportDocument rdocRpt, string rutaTicket)
        //{
        //    try
        //    {
        //        var crDiskFileDestinationOptions = new DiskFileDestinationOptions();
        //        var crFormatTypeOptions = new PdfRtfWordFormatOptions();

        //        crDiskFileDestinationOptions.DiskFileName = rutaTicket;
        //        var crExportOptions = rdocRpt.ExportOptions;
        //        crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
        //        crExportOptions.FormatOptions = crFormatTypeOptions;
        //        rdocRpt.Export();
                
        //    }
        //    catch (Exception ee)
        //    {
        //        throw new Exception(ee.Message);
        //    }
        //}

    }
}
