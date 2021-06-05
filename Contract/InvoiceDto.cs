﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class InvoiceDto
    {
        public long Sys_Id { get; set; }
        public long Sys_IdCompany { get; set; }
        public System.DateTime Sys_FechaRegistro { get; set; }
        public string Sys_RfcEmisor { get; set; }
        public string Sys_NombreEmisor { get; set; }
        public string Sys_RfcReceptor { get; set; }
        public string Sys_NombreReceptor { get; set; }
        public string Sys_RegimenFiscal { get; set; }
        public string Sys_UsoCfdi { get; set; }
        public string Sys_FormaPago { get; set; }
        public string Sys_MetodoPago { get; set; }
        public string Sys_LugarExpedicion { get; set; }
        public string Sys_NoCertificado { get; set; }
        public string Sys_CondicionPago { get; set; }
        public string Sys_Moneda { get; set; }
        public decimal Sys_Subtotal { get; set; }
        public decimal Sys_Total { get; set; }
        public System.DateTime Sys_FechaCaptura { get; set; }
        public string Sys_TipoComprobante { get; set; }
        public string Sys_Folio { get; set; }
        public string Sys_Serie { get; set; }
        public bool Sys_Cancelado { get; set; }
        public string Sys_TimbreFiscal { get; set; }
        public string Sys_TimbreFiscalRelacionado { get; set; }
        public decimal Sys_TipoCambio { get; set; }
        public decimal Sys_Descuento { get; set; }
    }
}
