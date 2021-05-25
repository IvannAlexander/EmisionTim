using ServicioContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmisionWeb.Models;

namespace EmisionWeb.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index(ClientesModif clientesGen)
        {
            
            if (string.IsNullOrEmpty(LoginSession.Usuario.Sys_Rfc))
            {
                return RedirectToAction("Index", "Login");
            }

            var clienteMif = new ClientesModif();

            if (!string.IsNullOrEmpty(clientesGen.Rfc) || !string.IsNullOrEmpty(clientesGen.Nombre))
            {
                clienteMif.ListaClientes = ObtenerClientesPorFiltro(clientesGen.Rfc, clientesGen.Nombre, LoginSession.Usuario.Sys_Rfc);
            }
            else
            {
                clienteMif.ListaClientes = ObtenerClientes(LoginSession.Usuario.Sys_Rfc);
            }

            return View(clienteMif);
        }

        private List<Sys_Cliente> ObtenerClientes(string rfc)
        {
            var cliente = ClientFactory.GetCliente();
            using (cliente as IDisposable)
            {
                return cliente.ObtenerClientePorRfcEmpresa(rfc);
            }
        }

        private List<Sys_Cliente> ObtenerClientesPorFiltro(string rfc, string razonSocial, string rfcEmpresa)
        {
            var cliente = ClientFactory.GetCliente();
            using (cliente as IDisposable)
            {
                return cliente.ObtenerClientePorRfcNombre(rfc, razonSocial, rfcEmpresa);
            }
        }

        public ActionResult GuardarCliente(ClientesModif sysCliente)
        {
            //var listaClientes = new List<Sys_Cliente>();
            try
            {
                var cliente = ClientFactory.GetCliente();
                using (cliente as IDisposable)
                {
                    sysCliente.Cliente.Sys_Fecha_Registro = DateTime.Now;
                    sysCliente.Cliente.Sys_Fecha_Modificacion = DateTime.Now;
                    sysCliente.Cliente.Sys_Usuario_Id = LoginSession.Usuario.Sys_Id;
                    sysCliente.Cliente.Sys_Empresa_Rfc = LoginSession.Usuario.Sys_Rfc;
                    cliente.GuardarClienteNuevo(sysCliente.Cliente);
                }
            }
            catch (Exception ee)
            {
                ViewBag.Error = ee.Message;
            }
            return RedirectToAction("Index", sysCliente);
        }
        
        //[HttpPost]
        public ActionResult BuscarClientes(ClientesModif sysCliente)
        {
            return RedirectToAction("Index", sysCliente);
        }
    }
}