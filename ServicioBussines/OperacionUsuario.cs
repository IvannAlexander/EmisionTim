using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicioContract;
using log4net;
using ServicioEmision;

namespace ServicioBussines
{
    public class OperacionUsuario : BaseObject
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionUsuario));

        public void GuardarUsuario(Sys_Usuario usuario)
        {
            try
            {
                var usr = ObtenerUsuario(usuario.Sys_Usr, usuario.Sys_Rfc);
                if (usr != null)
                {
                    throw new Exception("El usuario " + usuario.Sys_Usr + " ya existe.");
                }
                using (var db = new Entities())
                {
                    var pwd = OperacionesComun.Encrypt(usuario.Sys_Pass);
                    usuario.Sys_Pass = pwd;
                    //var emp = db.Sys_Empresa.FirstOrDefault(p=>p.Sys_Rfc == usuario.Sys_Rfc);
                    db.Sys_Usuario.Add(usuario);
                    db.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se guardo el usuario correctamente.");
            }
        }
        public Sys_Usuario ValidaUsuario(string usuario, string pass)
        {
            try
            {

                var pwd = OperacionesComun.Encrypt(pass);
                using (var db = new Entities())
                //using (var db = new Entities())
                {
                    var usr = db.Sys_Usuario.FirstOrDefault(p => p.Sys_Usr == usuario && p.Sys_Pass == pwd);
                    return usr;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se valido correctamente el usuario.");
            }
        }

        public Sys_Usuario ObtenerUsuario(string usuario, string rfc)
        {
            try
            {
                using (var db = new Entities())
                {
                    return db.Sys_Usuario.FirstOrDefault(p => p.Sys_Rfc == rfc && p.Sys_Usr == usuario);
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("Error al obtener el usuario.");
            }
        }
    }
}
