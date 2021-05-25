using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Model;

namespace Bussines
{
    public class MenuOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(MenuOperation));

        public List<MenuDto> GetMenu()
        {
            try
            {
                using (var db = new Db_EmisionEntities())
                {
                    var menus = db.Sys_Menu.ToList();

                    var menuDto = new List<MenuDto>();
                    menus.ForEach(p => menuDto.Add(Common.Map<Sys_Menu, MenuDto>(p)));

                    return menuDto;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No recuperar el menu completo.");
            }
        }


        //public List<Sys_Menu> ObtenerMenuPorUsuario(string rfc, string usuario)
        //{
        //    try
        //    {
        //        using (var db = new Entities())
        //        {
        //            var listMenu = db.Sys_Usuario.Where(p => p.Sys_Rfc == rfc && p.Sys_Usr == usuario)
        //                            .Join(
        //                                db.Sys_Menu_Usuario,
        //                                u => u.Sys_Id,
        //                                mp => mp.Sys_Usuario_Id,
        //                                (u, mp) => new { mp }
        //                            )
        //                            .Join(
        //                                db.Sys_Menu,
        //                                m => m.mp.Sys_Menu_Id,
        //                                me => me.Sys_Id,
        //                                (m, me) => new { me }
        //                            )
        //                            .ToList();

                    
        //            var lista = new List<Sys_Menu>();
        //            foreach (var menu in listMenu)
        //            {
        //                lista.Add(menu.me);
        //            }
                    
        //            return lista;
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("No recuperar el menu completo.");
        //    }
        //}

    }
}
