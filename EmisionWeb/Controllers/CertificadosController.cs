using ServicioContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace EmisionWeb.Controllers
{
    public class CertificadosController : Controller
    {
        private int NumRegistros = 1;
        // GET: Certificados
        //public ActionResult Index()
        //{
        //    if (string.IsNullOrEmpty(LoginSession.Usuario.Sys_Rfc))
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    var lista = CargaCert();
        //    //Index(1);
        //    return View(lista.ToPagedList(1, NumRegistros));
        //}

        public ActionResult Index(int? paginaActual)
        {
            var lista = new List<Sys_Certificado>();
            try
            {
                if (string.IsNullOrEmpty(LoginSession.Usuario.Sys_Rfc))
                {
                    return RedirectToAction("Index", "Login");
                }
                var pagina = paginaActual ?? 20;
                lista = CargaCert(NumRegistros, pagina);

            }
            catch (Exception ee)
            {
                ViewBag.Message = ee.InnerException != null ? ee.InnerException + " : " + ee.Message : ee.Message;
            }
            

            //return View(lista.ToPagedList(pagina, NumRegistros));
            return View(lista);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fuCer, HttpPostedFileBase fuKey, string txtCertificado)
        {
            ViewBag.Message = string.Empty;
            var lista = new List<Sys_Certificado>();
            try
            {
                ValidaInfo(fuCer, fuKey, txtCertificado);
                var cer = ConvierteBytes(fuCer);
                var key = ConvierteBytes(fuKey);
                var cliente = ClientFactory.GetCliente();
                using (cliente as IDisposable)
                {
                    cliente.GuardaCertificados(cer, key, txtCertificado, LoginSession.Usuario.Sys_Rfc);
                }
                
                ViewBag.Message = "Cargado correctamente.";

                lista = CargaCert(NumRegistros, 1);
            }
            catch (Exception ee)
            {
                ViewBag.Message = ee.Message;
            }
            //return View(lista.ToPagedList(1, NumRegistros));
            return View(lista);
        }

        public List<Sys_Certificado> CargaCert(int numRegistros, int paginaActual)
        {
            var certificados = new List<Sys_Certificado>();
            var cliente = ClientFactory.GetCliente();
            using (cliente as IDisposable)
            {
                certificados = cliente.ObtenerCertificadosPorRfc(LoginSession.Usuario.Sys_Rfc, numRegistros, paginaActual);
                var totalReg = cliente.CertificadosPorRfc(LoginSession.Usuario.Sys_Rfc);
                decimal tot = (totalReg / NumRegistros);
                ViewBag.TotalPag = Math.Round(tot);
                ViewBag.PageNumber = paginaActual;
            }
            return certificados;
        }

        private void ValidaInfo(HttpPostedFileBase fuCer, HttpPostedFileBase fuKey, string certificado)
        {
            if (fuCer == null)
            {
                throw new Exception("Favor de cargar el archivo .cer.");
            }
            if (fuKey == null)
            {
                throw new Exception("Favor de cargar el archivo .key.");
            }
            if (string.IsNullOrEmpty(certificado))
            {
                throw new Exception("Favor introducir la contraseña.");
            }
        }

        private byte[] ConvierteBytes(HttpPostedFileBase file)
        {
            var length = file.InputStream.Length;
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }
            return fileData;
        }
    }
}