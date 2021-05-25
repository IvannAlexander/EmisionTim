using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ServicioContract;
using ServicioEmision;

namespace ServicioBussines
{
    public class OperacionEmpresa : BaseObject
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionUsuario));

        public void NuevaEmpresa(Sys_Empresa empresa)
        {
            try
            {
                var emp = ObtenerEmpresaPorRfc(empresa.Sys_Rfc);
                if (emp != null)
                {
                    throw new Exception("La empresa con el RFC: " + empresa.Sys_Rfc + " ya existe.");
                }
                using (var db = new Entities())
                {
                    //foreach (var usr in empresa.Sys_Usuario)
                    //{
                    //    usr.Sys_Pass = OperacionesComun.Encrypt(usr.Sys_Pass);
                    //}
                    //db.Sys_Empresa.Add(empresa);
                    db.Sys_Empresa.Add(empresa);
                    db.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("Error al registrar nueva empresa.");
            }
        }

        public void GuardarEmpresa(Sys_Empresa empresa)
        {
            try
            {
                using (var db = new Entities())
                {
                    db.Sys_Empresa.Add(empresa);
                    db.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se guardo la empresa correctamente.");
            }
        }

        public Sys_Empresa ObtenerEmpresaPorRfc(string rfc)
        {
            try
            {
                using (var db = new Entities())
                {
                    return db.Sys_Empresa.FirstOrDefault(p => p.Sys_Rfc == rfc);
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("Error al buscar empresa por RFC.");
            }
        }
    }
}
