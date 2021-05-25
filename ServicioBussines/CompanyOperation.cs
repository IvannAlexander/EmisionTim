using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using EmisionService;
using Model;
using Contract;
using System.Data.Entity;
using AutoMapper;

namespace Bussines
{
    public class CompanyOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(UserOperation));

        public string CreateNewCompany(NewCompanyDto company)
        {
            Logger.Info("CreateNewCompany");
            try
            {
                var c = GetCompanyByRfc(company.Company.Sys_Rfc);
                if (c != null)
                {
                    throw new Exception("La empresa con el RFC: " + company.Company.Sys_Rfc + " ya existe.");
                }
                using (var context = new Db_EmisionEntities())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            
                            //add company
                            var comp = Common.Map<CompanyDto, Sys_Company>(company.Company);

                            context.Sys_Company.Add(comp);
                            context.SaveChanges();

                            //Add address
                            var address = Common.Map<CompanyAddressDto, Sys_CompanyAddress>(company.CompanyAddress);
                            address.Sys_Rfc = comp.Sys_Rfc;
                            context.Sys_CompanyAddress.Add(address);
                            context.SaveChanges();

                            //Get Menu
                            MenuOperation menuOperation = new MenuOperation();
                            var menus = menuOperation.GetMenu();

                            //Create Profile
                            Sys_Profile profile = new Sys_Profile
                            {
                                Sys_Name = "Administrador",
                                Sys_Rfc = comp.Sys_Rfc
                            };
                            context.Sys_Profile.Add(profile);
                            context.SaveChanges();

                            //Create Menu-Profile
                            foreach (var menu in menus)
                            {
                                Sys_Menu_Profile sys_Menu_Profile = new Sys_Menu_Profile
                                {
                                    Sys_Menu_Id = menu.Sys_Id,
                                    Sys_Profile_Id = profile.Sys_Id
                                };
                                context.Sys_Menu_Profile.Add(sys_Menu_Profile);
                                context.SaveChanges();
                            }

                            //Create User
                            var usr = new Sys_User
                            {
                                Sys_Email = comp.Sys_Email,
                                Sys_Status = 1,//Activo
                                Sys_CreationDate = DateTime.Now,
                                Sys_AccessAttempts = 0,
                                Sys_Pass = comp.Sys_Rfc,
                                Sys_Usr = $"Admin{comp.Sys_Rfc}",
                                Sys_Rfc = comp.Sys_Rfc
                            };

                            usr.Sys_Profile_Id = profile.Sys_Id;
                            usr.Sys_Pass = EmisionService.CommonOperation.Encrypt(usr.Sys_Pass);
                            context.Sys_User.Add(usr);
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        catch (Exception ee)
                        {
                            transaction.Rollback();
                            Logger.Error(ee);
                            throw new Exception("Error al registrar nueva empresa.");
                        }

                    }
                }
            }
            catch (Exception ee)
            {
                return ee.Message;
            }
            return string.Empty;
        }

        public void SaveCompany(CompanyDto company)
        {
            try
            {
                using (var db = new Db_EmisionEntities())
                {
                    var c = AutoMapper.Mapper.Map<Sys_Company>(company);
                    db.Sys_Company.Add(c);
                    db.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se guardo la empresa correctamente.");
            }
        }

        public CompanyDto GetCompanyByRfc(string rfc)
        {
            try
            {
                using (var db = new Db_EmisionEntities())
                {
                    var company = db.Sys_Company.FirstOrDefault(p => p.Sys_Rfc == rfc);

                    return Common.Map<Sys_Company, CompanyDto>(company);
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
