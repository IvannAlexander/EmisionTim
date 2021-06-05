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

using log4net;
using MessagingToolkit.QRCode.Codec;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace EmisionService
{
    public class PdfOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(PdfOperation));

        public byte[] GenerarPdfFactura(string xml)
        {
            try
            {
                var operacionesSerializacion = new EmisionOperation();
                
                var comprobante = operacionesSerializacion.DeserializarDocumentos(xml);
                comprobante.TipoDeComprobante = OperacionesCatalogo.ObtenerTipoComprobante(comprobante.TipoDeComprobante);
                comprobante.Emisor.RegimenFiscal = OperacionesCatalogo.ObtenerRegimenFiscal(comprobante.Emisor.RegimenFiscal);
                comprobante.Receptor.UsoCFDI = OperacionesCatalogo.ObtenerUsoCfdi(comprobante.Receptor.UsoCFDI);
                comprobante.MetodoPago = OperacionesCatalogo.ObtenerMetodoPago(comprobante.MetodoPago);
                comprobante.FormaPago = OperacionesCatalogo.ObtenerFormaPago(comprobante.FormaPago);
                comprobante.CantidadConLetra = EmisionService.AmountWithLetter.Enletras(comprobante.Total.ToString(), comprobante.Moneda);

                if (comprobante.Impuestos != null)
                {
                    if (comprobante.Impuestos.Traslados != null)
                    {
                        comprobante.IvaExt = comprobante.Impuestos.Traslados.Where(p => p.Impuesto == "002").Sum(p => p.Importe);
                        comprobante.IepsExt = comprobante.Impuestos.Traslados.Where(p => p.Impuesto == "003").Sum(p => p.Importe);
                    }
                }

                TimbreFiscalDigital timbreFiscal = null;
                //EstadoDeCuentaCombustible estadoDeCuentaCombustible = new EstadoDeCuentaCombustible();

                if (comprobante.Complemento != null)
                {
                    foreach (var complemento in comprobante.Complemento)
                    {
                        var tim = complemento.Any.FirstOrDefault(p => p.LocalName == "TimbreFiscalDigital");

                        if (tim != null)
                        {
                            var xmlTim = XDocument.Parse(tim.OuterXml);
                            timbreFiscal = operacionesSerializacion.Deserializar<TimbreFiscalDigital>(xmlTim.ToString());

                            comprobante.CadenaOriginal = operacionesSerializacion.ObtenerCadenaOriginal(xmlTim, true);
                        }
                    }
                }
                var ultimosCaracteres = comprobante.Sello.Substring(comprobante.Sello.Length - 8);
                var qr = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?&id=" + timbreFiscal.UUID + "&re=" + comprobante.Emisor.Rfc + "&rr=" + comprobante.Receptor.Rfc + "&tt=" + comprobante.Total + "&fe=" + ultimosCaracteres;
                comprobante.Qr = Convert.ToBase64String(GetQrCode(qr));
                return GenerarPdfConObjeto(comprobante, timbreFiscal);//, estadoDeCuentaCombustible);
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        //private byte[] GenerarPdfConObjeto(Comprobante comprobante, TimbreFiscalDigital timbreFiscal, EstadoDeCuentaCombustible estadoDeCuentaCombustible)
        private byte[] GenerarPdfConObjeto(Comprobante comprobante, TimbreFiscalDigital timbreFiscal)
        {
            try
            {
                var report = new ReportViewer();
                var reportPath = Path.Combine(ConfigurationManager.AppSettings["Rdlc"], "Factura.rdlc");
                var ds = new System.Data.DataSet() { Tables = { new DataTable() } };

                var listaComprobante = new List<Comprobante>();
                listaComprobante.Add(comprobante);
                var dtComprobante = ObtenerTablaConObjeto<Comprobante>(listaComprobante);

                var listaEmisor = new List<ComprobanteEmisor>();
                listaEmisor.Add(comprobante.Emisor);
                var dtEmisor = ObtenerTablaConObjeto<ComprobanteEmisor>(listaEmisor);

                var listaReceptor = new List<ComprobanteReceptor>();
                listaReceptor.Add(comprobante.Receptor);
                var dtReceptor = ObtenerTablaConObjeto<ComprobanteReceptor>(listaReceptor);

                var listaTimbre = new List<TimbreFiscalDigital>();
                listaTimbre.Add(timbreFiscal);
                var dtTimbre = ObtenerTablaConObjeto<TimbreFiscalDigital>(listaTimbre);

                //var listaEcc = new List<EstadoDeCuentaCombustible>();
                //listaEcc.Add(estadoDeCuentaCombustible);
                //var dtEcc = ObtenerTablaConObjeto<EstadoDeCuentaCombustible>(listaEcc);

                //var dtEccConc = ObtenerTablaConObjeto<EstadoDeCuentaCombustibleConceptoEstadoDeCuentaCombustible>(estadoDeCuentaCombustible.Conceptos);

                var dtConceptos = ObtenerTablaConObjeto<ComprobanteConcepto>(comprobante.Conceptos);

                var rdComprobante = new ReportDataSource("DsComprobante", dtComprobante);
                var rdTimbre = new ReportDataSource("DsTimbre", dtTimbre);
                //var rdEcc = new ReportDataSource("DsEcc", dtEcc);
                var rdEmisor = new ReportDataSource("DsEmisor", dtEmisor);
                var rdReceptor = new ReportDataSource("DsReceptor", dtReceptor);
                //var rdEccConc = new ReportDataSource("DsEccConc", dtEccConc);
                var rdConceptos = new ReportDataSource("DsConceptos", dtConceptos);

                report.LocalReport.DataSources.Clear();
                report.LocalReport.DataSources.Add(rdComprobante);
                report.LocalReport.DataSources.Add(rdTimbre);
                //report.LocalReport.DataSources.Add(rdEcc);
                report.LocalReport.DataSources.Add(rdEmisor);
                report.LocalReport.DataSources.Add(rdReceptor);
                //report.LocalReport.DataSources.Add(rdEccConc);
                report.LocalReport.DataSources.Add(rdConceptos);

                report.LocalReport.Refresh();
                report.LocalReport.ReportPath = reportPath;

                return EscribePdf(report);
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        private byte[] EscribePdf(ReportViewer report)
        {
            try
            {
                var devInfo = string.Empty;
                var mimeType = string.Empty;
                var encoding = string.Empty;
                PageCountMode pageCountMode = PageCountMode.Actual;
                Warning[] warnings;
                string[] streamIDs;
                var fileNameExtension = string.Empty;
                return report.LocalReport.Render("PDF", devInfo, pageCountMode, out mimeType, out encoding, out fileNameExtension, out streamIDs, out warnings);
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        public static DataTable ObtenerTablaConObjeto<TDataClass>(List<TDataClass> lista) where TDataClass : class
        {
            Type t = typeof(TDataClass);
            var dt = new DataTable(t.Name);
            foreach (PropertyInfo pi in t.GetProperties())
            {
                dt.Columns.Add(new DataColumn(pi.Name));
            }
            if (lista != null)
            {
                foreach (TDataClass item in lista)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dr[dc.ColumnName] = item.GetType().GetProperty(dc.ColumnName).GetValue(item, null);
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private byte[] GetQrCode(string cadena)
        {
            try
            {
                Bitmap resultado = new Bitmap(20, 20);
                QRCodeEncoder encoder = new QRCodeEncoder();
                encoder.QRCodeVersion = 0;
                resultado = encoder.Encode(cadena, System.Text.Encoding.UTF8);
                MemoryStream stream = new MemoryStream();
                resultado.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();
                return stream.ToArray();
            }
            catch (Exception ee)
            {

                throw new Exception(ee.Message);
            }

        }

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
