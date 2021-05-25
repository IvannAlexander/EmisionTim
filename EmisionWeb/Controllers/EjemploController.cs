using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmisionWeb.Controllers
{
    public class EjemploController : Controller
    {
        // GET: Ejemplo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index( string command)
        {
            if (command.Equals("submit1"))
            {
                // Call action here...
            }
            else
            {
                // Call another action here...
            }
            return View();
        }
    }
}