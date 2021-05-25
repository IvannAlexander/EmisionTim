using ServicioModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EmisionWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Sys_Usuario usuario)
        {
            LoginSession.Usuario = null;

            if (ModelState.IsValid)
            {
                var cliente = ClientFactory.GetCliente();
                using (cliente as IDisposable)
                {
                    var sysUsr = cliente.ValidaUsuario(usuario.Sys_Usr, usuario.Sys_Pass);
                    if (sysUsr != null)
                    {
                        LoginSession.Usuario = sysUsr;
                        return RedirectToAction("Index", "Inicio");
                    }
                    else
                    {
                        ViewBag.Message = "Usuario o contraseña incorrecto.";
                        //ModelState.AddModelError("", "Usuario o contraseña incorrecto.");
                    }

                }
            }
            return View(usuario);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}