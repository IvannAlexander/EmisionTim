using ServicioModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmisionWeb.Controllers
{
    public class LoginSession
    {
        public static Sys_Usuario Usuario {
            get
            {
                if (HttpContext.Current.Session["SysUsuario"] == null)
                {
                    HttpContext.Current.Session["SysUsuario"] = new Sys_Usuario();
                }
                return (Sys_Usuario)HttpContext.Current.Session["SysUsuario"];
            }
            set
            {
                if (value == null)
                {
                    HttpContext.Current.Session["SysUsuario"] = new Sys_Usuario();
                }
                HttpContext.Current.Session["SysUsuario"] = value;
            }
        }

    }
}