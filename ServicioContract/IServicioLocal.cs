using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServicioContract
{
    [ServiceContract]
    public interface IServicioLocal
    {

        [OperationContract]
        void GuardarUsuario(Sys_Usuario usuario);

        [OperationContract]
        Sys_Usuario ValidaUsuario(string usuario, string pass);

        [OperationContract]
        Sys_Usuario ObtenerUsuario(string usuario, string rfc);

        [OperationContract]
        void GuardarEmpresa(Sys_Empresa empresa);

        [OperationContract]
        void NuevaEmpresa(Sys_Empresa empresa);

        [OperationContract]
        Sys_Empresa ObtenerEmpresaPorRfc(string rfc);

        [OperationContract]
        void GuardaCertificados(byte[] archivoCer, byte[] archivoKey, string pass, string rfcEmpresa);

        [OperationContract]
        List<Sys_Menu> ObtenerMenuCompleto();

        [OperationContract]
        List<Sys_Menu> ObtenerMenuPorUsuario(string rfc, string usuario);

        [OperationContract]
        List<Sys_Certificado> ObtenerCertificadosPorRfc(string rfc, int numRegistros, int paginaActual);

        [OperationContract]
        int CertificadosPorRfc(string rfc);

        [OperationContract]
        List<Sys_Cliente> ObtenerClientePorRfcNombre(string rfc, string razonSocial, string rfcEmpresa);

        [OperationContract]
        List<Sys_Cliente> ObtenerClientePorRfcEmpresa(string rfcEmpresa);

        [OperationContract]
        int ObtenerTotalClientePorRfcEmpresa(string rfcEmpresa);

        [OperationContract]
        void GuardarClienteNuevo(Sys_Cliente cliente);

        [OperationContract]
        void GuardarModificacionesDelCliente(Sys_Cliente cliente);

        #region Personalizado
        /// <summary>
        /// Servicio para generar CFDI a partir de un ATI
        /// </summary>
        /// <param name="archivoAti"></param>
        /// <param name="usuario"></param>
        /// <param name="pass"></param>
        [OperationContract]
        void GenerarFacturaAti(byte[] archivoAti, string usuario, string pass);
        #endregion Personalizado
    }
}
