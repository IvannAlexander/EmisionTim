using log4net;
using ServicioContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioBussines
{
    public class OperacionMenu : BaseObject
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(OperacionMenu));

        public List<Sys_Menu> ObtenerMenuCompleto()
        {
            try
            {
                using (var db = new Entities())
                {
                    var listaMenu = db.Sys_Menu.ToList();
                    return listaMenu;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No recuperar el menu completo.");
            }
        }


        public List<Sys_Menu> ObtenerMenuPorUsuario(string rfc, string usuario)
        {
            try
            {
                using (var db = new Entities())
                {
                    var listMenu = db.Sys_Usuario.Where(p => p.Sys_Rfc == rfc && p.Sys_Usr == usuario)
                                    .Join(
                                        db.Sys_Menu_Usuario,
                                        u => u.Sys_Id,
                                        mp => mp.Sys_Usuario_Id,
                                        (u, mp) => new { mp }
                                    )
                                    .Join(
                                        db.Sys_Menu,
                                        m => m.mp.Sys_Menu_Id,
                                        me => me.Sys_Id,
                                        (m, me) => new { me }
                                    )
                                    .ToList();

                    
                    var lista = new List<Sys_Menu>();
                    foreach (var menu in listMenu)
                    {
                        lista.Add(menu.me);
                    }
                    
                    return lista;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No recuperar el menu completo.");
            }
        }

    }
}
