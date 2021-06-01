using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmisionService
{
    public class ConvertTxtOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ConvertTxtOperation));

        public Comprobante ConvertirTxt(string contenido)
        {
            var comprobante = new Comprobante();
            ValesDespensa1.ValesDeDespensa valesDeDespensa = null;
            try
            {

                
                var consumoDeCombustibles = new ConsumoCombustibles11.ConsumoDeCombustibles();
                var listaDatosExtra = new List<DatosExtras>();
                var seccion = string.Empty;
                var relacionados = new List<ComprobanteCfdiRelacionadosCfdiRelacionado>();
                var listaConceptos = new List<ComprobanteConcepto>();
                ComprobanteConcepto concepto = null;
                ComprobanteConceptoImpuestosTraslado trasladado = null;
                ComprobanteConceptoImpuestosRetencion retenido = null;

                ComprobanteImpuestosRetencion retenidoGen = null;
                ComprobanteImpuestosTraslado trasladadoGen = null;
                
                foreach (var linea in contenido.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
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

                    //Se divide por seccion
                    if (atributo.StartsWith("/"))
                    {
                        seccion = atributo;
                    }

                    //var complemento = string.Empty;
                    
                    if (seccion.Contains("/Comprobante/Adicionales/ValesDeDespensa"))
                    {
                        //complemento = "ValesDeDespensa";
                        if (valesDeDespensa == null)
                        {
                            comprobante.TipoComplemento = "ValesDeDespensa";
                            comprobante.ExisteComplemento = true;
                            valesDeDespensa = new ValesDespensa1.ValesDeDespensa();
                            valesDeDespensa.version = "1.0";
                            //comprobante.ValesDespensa = valesDeDespensa;
                        }
                        ComplementoValesDespensa(valesDeDespensa, seccion, atributo, valor);
                    }
                    //if (seccion == "/Comprobante/Conceptos/Operaciones")
                    //{
                    //    complemento = "EstadoDeCuentaCombustible";
                    //}

                    #region Conceptos
                    if (seccion == "/Comprobante/Conceptos/Concepto")
                    {
                        switch (atributo)
                        {
                            case "/Comprobante/Conceptos/Concepto":
                                concepto = new ComprobanteConcepto();
                                break;
                            case "claveProdServ":
                                concepto.ClaveProdServ = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "cantidad":
                                concepto.Cantidad = string.IsNullOrEmpty(valor) ? 0 : Convert.ToDecimal(valor);
                                break;
                            case "claveUnidad":
                                concepto.ClaveUnidad = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "unidad":
                                concepto.Unidad = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "descripcion":
                                concepto.Descripcion = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "valorUnitario":
                                concepto.ValorUnitario = ConvertToDecimalRound(valor, 2);
                                break;
                            case "importe":
                                concepto.Importe = ConvertToDecimalRound(valor, 2);
                                if (comprobante.Conceptos == null)
                                {
                                    comprobante.Conceptos = new List<ComprobanteConcepto>();
                                }
                                if (!string.IsNullOrEmpty(concepto.ClaveProdServ))
                                {
                                    comprobante.Conceptos.Add(concepto);
                                }
                                
                                break;
                        }
                        

                        
                    }
                    #endregion Conceptos

                    #region Trasladados
                    if (seccion == "/Comprobante/Conceptos/Concepto/ImpuestosTraslados")
                    {
                        switch (atributo)
                        {
                            case "base":
                                if (!string.IsNullOrEmpty(valor))
                                {
                                    trasladado = new ComprobanteConceptoImpuestosTraslado();
                                    trasladado.Base = ConvertToDecimalRound(valor, 2);
                                }
                                break;
                            case "impuesto":
                                trasladado.Impuesto = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "tipoFactor":
                                trasladado.TipoFactor = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "tasaCuota":
                                if (!string.IsNullOrEmpty(valor))
                                {
                                    trasladado.TasaOCuota = ConvertToDecimalRound(valor, 4);
                                    trasladado.TasaOCuotaSpecified = true;
                                }
                                break;
                            case "importe":
                                if (!string.IsNullOrEmpty(valor))
                                {
                                    trasladado.Importe = ConvertToDecimalRound(valor, 2);
                                    trasladado.ImporteSpecified = true;
                                    if (concepto.Impuestos == null)
                                    {
                                        concepto.Impuestos = new ComprobanteConceptoImpuestos();
                                    }
                                    if (concepto.Impuestos.Traslados == null)
                                    {
                                        concepto.Impuestos.Traslados = new List<ComprobanteConceptoImpuestosTraslado>();
                                    }

                                    concepto.Impuestos.Traslados.Add(trasladado);
                                    trasladado = null;
                                }
                                break;
                        }
                    }
                    #endregion Trasladados

                    #region Comprobante
                    if (seccion == "/Comprobante")
                    {
                        switch (atributo)
                        {
                            case "version":
                                comprobante.Version = valor;
                                break;
                            case "serie":
                                comprobante.Serie = valor;
                                break;
                            case "folio":
                                comprobante.Folio = valor;
                                break;
                            case "fecha":
                                //var fechaExp = DateTime.Now;
                                //DateTime.TryParseExact(valor, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out fechaExp);
                                comprobante.Fecha = valor;
                                break;
                            case "formaDePago":
                                comprobante.FormaPago = valor;
                                break;
                            case "condicionesDePago":
                                comprobante.CondicionesDePago = valor;
                                break;
                            case "subTotal":
                                comprobante.SubTotal = ConvertToDecimalRound(valor, 2);
                                break;
                            case "descuento":
                                if (!string.IsNullOrEmpty(valor))
                                {
                                    comprobante.Descuento = ConvertToDecimalRound(valor, 2);
                                    comprobante.DescuentoSpecified = true;
                                }
                                break;
                            case "total":
                                comprobante.Total = ConvertToDecimalRound(valor, 2);
                                break;
                            case "metodoDePago":
                                comprobante.MetodoPago = valor;
                                break;
                            case "tipoDeComprobante":
                                comprobante.TipoDeComprobante = valor;
                                break;
                            case "lugarExpedicion":
                                comprobante.LugarExpedicion = valor;
                                break;

                            default:
                                listaDatosExtra.Add(new DatosExtras { Atributo = atributo, Valor = valor, Seccion = seccion });
                                break;
                        }
                    }
                    else if (seccion == "/Comprobante/Adicionales")
                    {
                        switch (atributo)
                        {
                            case "moneda":
                                comprobante.Moneda = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "tipoDeCambio":
                                if (string.IsNullOrEmpty(valor))
                                {
                                    comprobante.TipoCambioSpecified = true;
                                    comprobante.TipoCambio = Convert.ToDecimal(valor);
                                }
                                break;
                            default:
                                listaDatosExtra.Add(new DatosExtras { Atributo = atributo, Valor = valor, Seccion = seccion });
                                break;
                        }
                    }
                    else if (seccion == "/Comprobante/Relacionado")
                    {
                        //if (atributo == "tipoRelacion")
                        //{
                        //    if (string.IsNullOrEmpty(valor))
                        //    {
                        //        comprobante.CfdiRelacionados.CfdiRelacionado = new ComprobanteCfdiRelacionadosCfdiRelacionado { UUID=new string ()};
                        //    }
                        //}

                    }
                    else if (seccion == "/Comprobante/Relacionado/cfdiRelacionado")
                    {
                        if (atributo == "UUID")
                        {
                            if (!string.IsNullOrEmpty(valor))
                            {
                                var cfdiRelacionado = new ComprobanteCfdiRelacionadosCfdiRelacionado();
                                relacionados.Add(new ComprobanteCfdiRelacionadosCfdiRelacionado { UUID = valor });
                            }
                        }
                    }
                    else if (seccion == "/Comprobante/Emisor")
                    {
                        switch (atributo)
                        {
                            case "rfc":
                                comprobante.Emisor = new ComprobanteEmisor();
                                comprobante.Emisor.Rfc = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "nombre":
                                comprobante.Emisor.Nombre = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                        }
                    }
                    else if (seccion == "/Comprobante/Emisor/Regimenes")
                    {
                        switch (atributo)
                        {
                            case "regimen":
                                comprobante.Emisor.RegimenFiscal = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                        }
                    }
                    else if (seccion == "/Comprobante/Emisor/DomicilioFiscal")
                    {
                        listaDatosExtra.Add(new DatosExtras { Atributo = atributo, Valor = valor, Seccion = seccion });
                    }
                    else if (seccion == "/Comprobante/Emisor/Adicionales")
                    {
                        listaDatosExtra.Add(new DatosExtras { Atributo = atributo, Valor = valor, Seccion = seccion });
                    }
                    else if (seccion == "/Comprobante/Emisor/ExpedidoEn")
                    {
                        listaDatosExtra.Add(new DatosExtras { Atributo = atributo, Valor = valor, Seccion = seccion });
                    }
                    else if (seccion == "/Comprobante/Emisor/ExpedidoEn/Adicionales")
                    {
                        listaDatosExtra.Add(new DatosExtras { Atributo = atributo, Valor = valor, Seccion = seccion });
                    }
                    else if (seccion == "/Comprobante/Receptor")
                    {
                        switch (atributo)
                        {
                            case "rfc":
                                comprobante.Receptor = new ComprobanteReceptor();
                                comprobante.Receptor.Rfc = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "nombre":
                                comprobante.Receptor.Nombre = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "residenciaFiscal":
                                comprobante.Receptor.ResidenciaFiscal = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "numRegIdTrib":
                                comprobante.Receptor.NumRegIdTrib = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                            case "usoCFDI":
                                comprobante.Receptor.UsoCFDI = string.IsNullOrEmpty(valor) ? null : valor;
                                break;
                        }
                    }
                    else if (seccion == "/Comprobante/Receptor/Domicilio")
                    {
                        listaDatosExtra.Add(new DatosExtras { Atributo = atributo, Valor = valor, Seccion = seccion });
                    }

                    #endregion Comprobante

                    #region Impuestos
                    if (seccion == "/Comprobante/Impuestos")
                    {
                        if (comprobante.Impuestos == null)
                        {
                            comprobante.Impuestos = new ComprobanteImpuestos();
                        }
                        switch (atributo)
                        {
                            case "totalImpuestosRetenidos":
                                if (!string.IsNullOrEmpty(valor))
                                {
                                    comprobante.Impuestos.TotalImpuestosRetenidos = ConvertToDecimalRound(valor, 2);
                                    comprobante.Impuestos.TotalImpuestosRetenidosSpecified = true;
                                }
                                else
                                {
                                    comprobante.Impuestos.TotalImpuestosRetenidosSpecified = false;
                                }
                                break;
                            case "totalImpuestosTrasladados":
                                if (!string.IsNullOrEmpty(valor))
                                {
                                    comprobante.Impuestos.TotalImpuestosTrasladados = ConvertToDecimalRound(valor, 2);
                                    comprobante.Impuestos.TotalImpuestosTrasladadosSpecified = true;
                                }
                                else
                                {
                                    comprobante.Impuestos.TotalImpuestosTrasladadosSpecified = false;
                                }
                                break;
                        }
                    }

                    if (seccion == "/Comprobante/Impuestos/Retenciones/Retencion")
                    {
                        switch (atributo)
                        {
                            case "tipoImpuesto":
                                if (comprobante.Impuestos.Retenciones == null && !string.IsNullOrEmpty(valor))
                                {
                                    comprobante.Impuestos.Retenciones = new List<ComprobanteImpuestosRetencion>();
                                }
                                if (comprobante.Impuestos.Retenciones != null)
                                {
                                    retenidoGen = new ComprobanteImpuestosRetencion();
                                    retenidoGen.Impuesto = valor;
                                }
                                break;
                            case "importe":
                                if (comprobante.Impuestos.Retenciones != null)
                                {
                                    retenidoGen.Importe = ConvertToDecimalRound(valor, 2);
                                    comprobante.Impuestos.Retenciones.Add(retenidoGen);
                                }
                                break;
                        }
                    }

                    if (seccion == "/Comprobante/Impuestos/Traslados/Traslado")
                    {

                        switch (atributo)
                        {
                            case "tipoImpuesto":
                                if (comprobante.Impuestos.Traslados == null && !string.IsNullOrEmpty(valor))
                                {
                                    comprobante.Impuestos.Traslados = new List<ComprobanteImpuestosTraslado>();
                                }
                                if (comprobante.Impuestos.Traslados != null)
                                {
                                    trasladadoGen = new ComprobanteImpuestosTraslado();
                                    trasladadoGen.Impuesto = valor;
                                }
                                break;
                            case "tipoFactor":
                                if (comprobante.Impuestos.Traslados != null)
                                {
                                    trasladadoGen.TipoFactor = valor;
                                }
                                break;
                            case "tasaCuota":
                                if (comprobante.Impuestos.Traslados != null)
                                {
                                    trasladadoGen.TasaOCuota = ConvertToDecimalRound(valor, 6);
                                }
                                break;
                            case "importe":
                                if (comprobante.Impuestos.Traslados != null)
                                {
                                    trasladadoGen.Importe = ConvertToDecimalRound(valor, 2);
                                    comprobante.Impuestos.Traslados.Add(trasladadoGen);
                                }
                                break;
                        }
                    }

                    #endregion Impuestos

                    if (relacionados.Count() > 0)
                    {
                        comprobante.CfdiRelacionados = new ComprobanteCfdiRelacionados();
                        comprobante.CfdiRelacionados.CfdiRelacionado = new List<ComprobanteCfdiRelacionadosCfdiRelacionado>();
                        comprobante.CfdiRelacionados.CfdiRelacionado = relacionados;
                    }

                    
                }
                
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
            }

            if (valesDeDespensa != null)
            {
                comprobante.ValesDespensa = valesDeDespensa;
            }

            //comprobante.Certificado = "@@Cert@@";
            //comprobante.Sello = "@@Sello@@";
            return comprobante;
        }


        ValesDespensa1.ValesDeDespensaConcepto valesDespensaConcepto { get; set; }
        private void ComplementoValesDespensa(ValesDespensa1.ValesDeDespensa valesDeDespensa, string seccion, string atributo, string valor)
        {
            try
            {
                if (seccion == "/Comprobante/Adicionales/ValesDeDespensa")
                {
                    switch(atributo)
                    {
                        case "tipodeoperacion":
                            valesDeDespensa.tipoOperacion = valor;
                            break;
                        case "registroPatronal":
                            valesDeDespensa.registroPatronal = string.IsNullOrEmpty(valor) ? null : valor;
                            break;
                        case "numeroCuenta":
                            valesDeDespensa.numeroDeCuenta = valor;
                            break;
                        case "total":
                            valesDeDespensa.total = ConvertToDecimalRound(valor, 2);
                            break;
                    }
                }

                if (seccion == "/Comprobante/Adicionales/ValesDeDespensa/Detalle")
                {
                    switch (atributo)
                    {
                        case "identificador":
                            valesDespensaConcepto = new ValesDespensa1.ValesDeDespensaConcepto();
                            valesDespensaConcepto.identificador = valor;
                            break;
                        case "tarjeta":
                            break;
                        case "transaccion":
                            break;
                        case "rfcestablecimiento":
                            break;
                        case "establecimiento":
                            break;
                        case "fecha":
                            valesDespensaConcepto.fecha = string.Format("{0:yyyy-MM-ddTHH:mm:ss}", ConvertToDateFormat(valor));
                            break;
                        case "rfc":
                            valesDespensaConcepto.rfc = valor;
                            break;
                        case "curp":
                            valesDespensaConcepto.curp = valor;
                            break;
                        case "nombre":
                            valesDespensaConcepto.nombre = valor;
                            break;
                        case "numSeguridadSocial":
                            valesDespensaConcepto.numSeguridadSocial = string.IsNullOrEmpty(valor) ? null : valor;
                            break;
                        case "importe":
                            if (valesDeDespensa.Conceptos == null)
                            {
                                valesDeDespensa.Conceptos = new List<ValesDespensa1.ValesDeDespensaConcepto>();
                            }
                            valesDespensaConcepto.importe = ConvertToDecimalRound(valor, 2);
                            valesDeDespensa.Conceptos.Add(valesDespensaConcepto);
                            valesDespensaConcepto = null;
                            break;
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception(ee.Message);
            }
        }

        private decimal ConvertToDecimalRound(string valor, int decimales)
        {
            decimal resp = 0;
            decimal.TryParse(valor, out resp);
            decimal.Round(resp, decimales, MidpointRounding.AwayFromZero);
            return resp;
        }

        private DateTime ConvertToDateFormat(string valor)
        {
            DateTime resp = DateTime.Now;
            DateTime.TryParse(valor, out resp);
            return resp;
        }
    }
}
