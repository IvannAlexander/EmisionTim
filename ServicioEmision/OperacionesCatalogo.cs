using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmisionService
{
    public class OperacionesCatalogo
    {
        public static string ObtenerTipoComprobante(string tipoComprobante)
        {
            switch (tipoComprobante)
            {
                case "I":
                    return "I - Ingreso";
                case "E":
                    return "E - Egreso";
                case "T":
                    return "T - Traslado";
                case "N":
                    return "N - Nómina";
                case "P":
                    return "P - Pago";
                default:
                    return tipoComprobante;
            }
        }
        public static string ObtenerRegimenFiscal(string regimen)
        {
            switch (regimen)
            {
                case "601":
                    return "601 - General de Ley Personas Morales";
                case "603":
                    return "603 - Personas Morales con Fines no Lucrativos";
                case "605":
                    return "605 - Sueldos y Salarios e Ingresos Asimilados a Salarios";
                case "606":
                    return "606 - Arrendamiento";
                case "608":
                    return "608 - Demás ingresos";
                case "609":
                    return "609 - Consolidación";
                case "610":
                    return "610 - Residentes en el Extranjero sin Establecimiento Permanente en México";
                case "611":
                    return "611 - Ingresos por Dividendos(socios y accionistas)";
                case "612":
                    return "612 - Personas Físicas con Actividades Empresariales y Profesionales";
                case "614":
                    return "614 - Ingresos por intereses";
                case "616":
                    return "616 - Sin obligaciones fiscales";
                case "620":
                    return "620 - Sociedades Cooperativas de Producción que optan por diferir sus ingresos";
                case "621":
                    return "621 - Incorporación Fiscal";
                case "622":
                    return "622 - Actividades Agrícolas, Ganaderas, Silvícolas y Pesqueras";
                case "623":
                    return "623 - Opcional para Grupos de Sociedades";
                case "624":
                    return "624 - Coordinados";
                case "628":
                    return "628 - Hidrocarburos";
                case "607":
                    return "607 - Régimen de Enajenación o Adquisición de Bienes";
                case "629":
                    return "629 - De los Regímenes Fiscales Preferentes y de las Empresas Multinacionales";
                case "630":
                    return "630 - Enajenación de acciones en bolsa de valores";
                case "615":
                    return "615 - Régimen de los ingresos por obtención de premios";
                default:
                    return regimen;
            }
        }
        public static string ObtenerUsoCfdi(string usoCfdi)
        {
            switch (usoCfdi)
            {
                case "G01":
                    return "G01 - Adquisición de mercancias";
                case "G02":
                    return "G02 - Devoluciones, descuentos o bonificaciones";
                case "G03":
                    return "G03 - Gastos en general";
                case "I01":
                    return "I01 - Construcciones";
                case "I02":
                    return "I02 - Mobilario y equipo de oficina por inversiones";
                case "I03":
                    return "I03 - Equipo de transporte";
                case "I04":
                    return "I04 - Equipo de computo y accesorios";
                case "I05":
                    return "I05 - Dados, troqueles, moldes, matrices y herramental";
                case "I06":
                    return "I06 - Comunicaciones telefónicas";
                case "I07":
                    return "I07 - Comunicaciones satelitales";
                case "I08":
                    return "I08 - Otra maquinaria y equipo";
                case "D01":
                    return "D01 - Honorarios médicos, dentales y gastos hospitalarios.";
                case "D02":
                    return "D02 - Gastos médicos por incapacidad o discapacidad";
                case "D03":
                    return "D03 - Gastos funerales.";
                case "D04":
                    return "D04 - Donativos.";
                case "D05":
                    return "D05 - Intereses reales efectivamente pagados por créditos hipotecarios(casa habitación).";
                case "D06":
                    return "D06 - Aportaciones voluntarias al SAR.";
                case "D07":
                    return "D07 - Primas por seguros de gastos médicos.";
                case "D08":
                    return "D08 - Gastos de transportación escolar obligatoria.";
                case "D09":
                    return "D09 - Depósitos en cuentas para el ahorro, primas que tengan como base planes de pensiones.";
                case "D10":
                    return "D10 - Pagos por servicios educativos(colegiaturas)";
                case "P01":
                    return "P01 - Por definir";
                default:
                    return usoCfdi;
            }
        }
        public static string ObtenerMetodoPago(string metodoPago)
        {
            switch (metodoPago)
            {
                case "PUE":
                    return "PUE - Pago en una sola exhibición";
                case "PPD":
                    return "PPD - Pago en parcialidades o diferido";

                default:
                    return metodoPago;
            }
        }
        public static string ObtenerFormaPago(string formaPago)
        {
            switch (formaPago)
            {
                case "01":
                    return "01 -  Efectivo";
                case "02":
                    return "02 -  Cheque nominativo";
                case "03":
                    return "03 -  Transferencia electrónica de fondos";
                case "04":
                    return "04 -  Tarjeta de crédito";
                case "05":
                    return "05 -  Monedero electrónico";
                case "06":
                    return "06 -  Dinero electrónico";
                case "08":
                    return "08 -  Vales de despensa";
                case "12":
                    return "12 -  Dación en pago";
                case "13":
                    return "13 -  Pago por subrogación";
                case "14":
                    return "14 -  Pago por consignación";
                case "15":
                    return "15 -  Condonación";
                case "17":
                    return "17 -  Compensación";
                case "23":
                    return "23 -  Novación";
                case "24":
                    return "24 -  Confusión";
                case "25":
                    return "25 -  Remisión de deuda";
                case "26":
                    return "26 -  Prescripción o caducidad";
                case "27":
                    return "27 -  A satisfacción del acreedor";
                case "28":
                    return "28 -  Tarjeta de débito";
                case "29":
                    return "29 -  Tarjeta de servicios";
                case "30":
                    return "30 -  Aplicación de anticipos";
                case "31":
                    return "31 -  Intermediario pagos";
                case "99":
                    return "99 -  Por definir";
                default:
                    return formaPago;
            }
        }
    }
}
