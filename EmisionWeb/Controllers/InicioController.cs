using ServicioContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmisionWeb.Controllers
{
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(LoginSession.Usuario.Sys_Rfc))
            {
                return RedirectToAction("Index", "Login");
            }
            if (Session["Sys_Menu"] == null)
            {
                var cliente = ClientFactory.GetCliente();
                using (cliente as IDisposable)
                {
                    Session["Sys_Menu"] = cliente.ObtenerMenuPorUsuario(LoginSession.Usuario.Sys_Rfc, LoginSession.Usuario.Sys_Usr);
                }
            }

            return View();
        }
        
        public ActionResult CerrarSesion()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}