using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ServicioBussines;
using ServicioContract;

namespace ServicioConsola
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class LocalServiceProcess : BaseObject, IServicioLocal
    {

        #region Usuarios
        /// <summary>
        /// Método encargado de validar si existe el usuario.
        /// </summary>
        /// <param name="usuario">Nombre del usuario.</param>
        /// <param name="pass">Contraseña del usuario</param>
        /// <returns></returns>
        public Sys_Usuario ValidaUsuario(string usuario, string pass)
        {
            var operacionUsuario = new OperacionUsuario();
            return operacionUsuario.ValidaUsuario(usuario, pass);
        }

        /// <summary>
        /// Método encargado de obtener a usuario por usuario o rfc.
        /// </summary>
        /// <param name="usuario">Usuario unico en toda la base de datos.</param>
        /// <param name="rfc">RFC del usuario.</param>
        /// <returns></returns>
        public Sys_Usuario ObtenerUsuario(string usuario, string rfc)
        {
            var operacionUsuario = new OperacionUsuario();
            var usr = operacionUsuario.ObtenerUsuario(usuario, rfc);
            return usr;
        }

        /// <summary>
        /// Método encargado de guardar el objeto Usuario.
        /// </summary>
        /// <param name="usuario">Objeto usuario ya con información.</param>
        public void GuardarUsuario(Sys_Usuario usuario)
        {
            var operacionUsuario = new OperacionUsuario();
            operacionUsuario.GuardarUsuario(usuario);
        }

        #endregion Usuarios

        #region Empresas

        /// <summary>
        /// Método encargado de guardar una empresa ya existente.
        /// </summary>
        /// <param name="empresa">Objeto empresa ya con información.</param>
        public void GuardarEmpresa(Sys_Empresa empresa)
        {
            var operacionEmpresa = new OperacionEmpresa();
            operacionEmpresa.GuardarEmpresa(empresa);
        }

        /// <summary>
        /// Método encargado de guardar nuevo registro de empresa.
        /// </summary>
        /// <param name="empresa">Objeto de empresa ya con información.</param>
        public void NuevaEmpresa(Sys_Empresa empresa)
        {
            var operacionEmpresa = new OperacionEmpresa();
            operacionEmpresa.NuevaEmpresa(empresa);
        }

        /// <summary>
        /// Método encargado de Obtener información de empresa por RFC
        /// </summary>
        /// <param name="rfc">RFC de la empresa.</param>
        /// <returns></returns>
        public Sys_Empresa ObtenerEmpresaPorRfc(string rfc)
        {
            var operacionEmpresa = new OperacionEmpresa();
            var empresa = operacionEmpresa.ObtenerEmpresaPorRfc(rfc);
            return empresa;
        }

        #endregion Empresas

        #region Menus
        
        /// <summary>
        /// Método encargado de obtener todos los menus del sustema.
        /// </summary>
        /// <returns>Regresa lista de menus completo.</returns>
        public List<Sys_Menu> ObtenerMenuCompleto()
        {
            var operacionMenu = new OperacionMenu();
            return operacionMenu.ObtenerMenuCompleto();
        }

        /// <summary>
        /// Obtener lista de menus por RFC y usuario.
        /// </summary>
        /// <param name="rfc">RFC de la empresa logueada</param>
        /// <param name="usuario">Usuario logueado.</param>
        /// <returns>Regresa lista de menus.</returns>
        public List<Sys_Menu> ObtenerMenuPorUsuario(string rfc, string usuario)
        {
            var operacionMenu = new OperacionMenu();
            return operacionMenu.ObtenerMenuPorUsuario(rfc, usuario);
        }

        #endregion Menus

        #region Certificados

        /// <summary>
        /// Método encargado de obtener certificados por RFC.
        /// </summary>
        /// <param name="rfc">RFC de la empresa logueada.</param>
        /// <param name="numRegistros">Numero de registros a mostrar.</param>
        /// <param name="paginaActual">pagina actual en la que esta posicionado.</param>
        /// <returns>Regresa lista de certificados.</returns>
        public List<Sys_Certificado> ObtenerCertificadosPorRfc(string rfc, int numRegistros, int paginaActual)
        {
            var operacionCertificado = new OperacionCertificados();
            return operacionCertificado.ObtenerCertificadosPorRfc(rfc, numRegistros, paginaActual);
        }

        /// <summary>
        /// Obtener total de certificados.
        /// </summary>
        /// <param name="rfc">RFC de la empresa logueada.</param>
        /// <returns>Regresa numero total de certificados.</returns>
        public int CertificadosPorRfc(string rfc)
        {
            var operacionCertificado = new OperacionCertificados();
            return operacionCertificado.CertificadosPorRfc(rfc);
        }

        /// <summary>
        /// Método encargado de guardar certificado y key.
        /// </summary>
        /// <param name="archivoCer">Archivo .cer en arreglo</param>
        /// <param name="archivoKey">Archivo .key en arreglo</param>
        /// <param name="pass">Contraseña del certificado.</param>
        /// <param name="rfcEmpresa">Rfc de la empresa logueada.</param>
        public void GuardaCertificados(byte[] archivoCer, byte[] archivoKey, string pass, string rfcEmpresa)
        {
            var operacionCertificados = new OperacionCertificados();
            operacionCertificados.GuardaCertificados(archivoCer, archivoKey, pass, rfcEmpresa);
        }

        #endregion Certificados

        #region Clientes

        /// <summary>
        /// método encargado de obtener el cliente por RFC, razon social o nombre comercial.
        /// </summary>
        /// <param name="rfc">RFC capturado por el usuario.</param>
        /// <param name="razonSocial">Razon social o nombre comercial.</param>
        /// <param name="rfcEmpresa">RFC de la empresa logueada.</param>
        /// <returns>Regresa lista de clientes por filtro.</returns>
        public List<Sys_Cliente> ObtenerClientePorRfcNombre(string rfc, string razonSocial, string rfcEmpresa)
        {
            var operacionCliente = new OperacionCliente();
            return operacionCliente.ObtenerClientePorRfcNombre(rfc, razonSocial, rfcEmpresa);
        }

        /// <summary>
        /// Método encargado para obtener todos los registros de clientes por empresa.
        /// </summary>
        /// <param name="rfcEmpresa">RFC de la empresa logueada.</param>
        /// <returns>Regresa lista de clientes.</returns>
        public List<Sys_Cliente> ObtenerClientePorRfcEmpresa(string rfcEmpresa)
        {
            var operacionCliente = new OperacionCliente();
            return operacionCliente.ObtenerClientePorRfcEmpresa(rfcEmpresa);
        }

        /// <summary>
        /// Métpdp encargado de obtener el total de clientes por empresa logueada.
        /// </summary>
        /// <param name="rfcEmpresa">RFC de empresa logueada.</param>
        /// <returns>Regresa total de clientes.</returns>
        public int ObtenerTotalClientePorRfcEmpresa(string rfcEmpresa)
        {
            var operacionCliente = new OperacionCliente();
            return operacionCliente.ObtenerTotalClientePorRfcEmpresa(rfcEmpresa);
        }

        /// <summary>
        /// Método encargado de guardar un cliente nuevo.
        /// </summary>
        /// <param name="cliente">Objeto con información del cliente.</param>
        public void GuardarClienteNuevo(Sys_Cliente cliente)
        {
            var operacionCliente = new OperacionCliente();
            operacionCliente.GuardarClienteNuevo(cliente);
        }

        /// <summary>
        /// Método encargado de guardar modificaciones para el cliente.
        /// </summary>
        /// <param name="cliente">Objeto con información del cliente.</param>
        public void GuardarModificacionesDelCliente(Sys_Cliente cliente)
        {
            var operacionCliente = new OperacionCliente();
            operacionCliente.GuardarModificacionesDelCliente(cliente);
        }

        #endregion Clientes

        #region Personalizado

        public void GenerarFacturaAti(byte[] archivoAti, string usuario, string pass)
        {
            var sys_Usuario = ValidaUsuario(usuario, pass);
            if (sys_Usuario != null)
            {
                var operacionConvierteTxt = new OperacionConvierteTxt();
                operacionConvierteTxt.ConvierteTxtAti(archivoAti, sys_Usuario.Sys_Rfc);
            }
        }

        #endregion Personalizado
    }
}
