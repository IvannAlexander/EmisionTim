using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using EmisionService;
using Contract;
using Model;

namespace Bussines
{
    public class UserOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(UserOperation));

        //public void GuardarUsuario(Sys_Usuario usuario)
        //{
        //    try
        //    {
        //        var usr = ObtenerUsuario(usuario.Sys_Usr, usuario.Sys_Rfc);
        //        if (usr != null)
        //        {
        //            throw new Exception("El usuario " + usuario.Sys_Usr + " ya existe.");
        //        }
        //        using (var db = new Entities())
        //        {
        //            var pwd = OperacionesComun.Encrypt(usuario.Sys_Pass);
        //            usuario.Sys_Pass = pwd;
        //            //var emp = db.Sys_Empresa.FirstOrDefault(p=>p.Sys_Rfc == usuario.Sys_Rfc);
        //            db.Sys_Usuario.Add(usuario);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("No se guardo el usuario correctamente.");
        //    }
        //}
        public UsersDto ValidateUser(string user, string pass, string rfcCompany)
        {
            Logger.Info("ValidtaeUser");
            try
            {
                var pwd = EmisionService.CommonOperation.Encrypt(pass);
                using (var db = new Db_EmisionEntities())
                {
                    var usr = db.Sys_User.FirstOrDefault(p => p.Sys_Usr == user && p.Sys_Pass == pwd && p.Sys_Rfc == rfcCompany);
                    var u = Common.Map<Sys_User, UsersDto>(usr);
                    return u;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                Logger.Error(ee.StackTrace);
                if (ee.InnerException != null)
                {
                    Logger.Error(ee.InnerException.Message);
                }
                return null;
            }
        }

        //public Sys_Usuario ObtenerUsuario(string usuario, string rfc)
        //{
        //    try
        //    {
        //        using (var db = new Entities())
        //        {
        //            return db.Sys_Usuario.FirstOrDefault(p => p.Sys_Rfc == rfc && p.Sys_Usr == usuario);
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("Error al obtener el usuario.");
        //    }
        //}
    }
}
