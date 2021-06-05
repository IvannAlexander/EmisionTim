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

        public string SaveUser(UsersDto user)
        {
            try
            {
                var usr = GetUser(user.Sys_Usr, user.Sys_IdCompany);
                if (usr != null)
                {
                    return $"El usuario {user.Sys_Usr} ya existe.";
                }
                using (var db = new Db_EmisionEntities())
                {
                    var pwd = EmisionService.CommonOperation.Encrypt(user.Sys_Pass);
                    user.Sys_Pass = pwd;
                    var usrDb = Common.Map<UsersDto, Sys_User>(user);
                    db.Sys_User.Add(usrDb);
                    db.SaveChanges();
                }
                return string.Empty;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                Logger.Error(ee.StackTrace);
                if (ee.InnerException != null)
                {
                    Logger.Error(ee.InnerException.Message);
                }
                return ee.Message;
            }
        }
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

        public UsersDto GetUser(string user, long idCompany)
        {
            try
            {
                using (var db = new Db_EmisionEntities())
                {
                    var usr = db.Sys_User.FirstOrDefault(p => p.Sys_IdCompany == idCompany && p.Sys_Usr == user);
                    return Common.Map<Sys_User, UsersDto>(usr);
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

        public string CreateProfile(string name, long idCompany)
        {
            try
            {
                using (var db = new Db_EmisionEntities())
                {
                    var profile = db.Sys_Profile.FirstOrDefault(p => p.Sys_IdCompany == idCompany && p.Sys_Name == name);
                    if (profile != null)
                    {
                        return $"El perfil {name} ya existe";
                    }
                    else
                    {
                        Sys_Profile profileDb = new Sys_Profile
                        {
                            Sys_Name = name,
                            Sys_IdCompany= idCompany
                        };
                        db.Sys_Profile.Add(profileDb);
                        db.SaveChanges();
                    }
                    return string.Empty;
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
                return ee.Message;
            }
        }

        public List<ProfileDto> GetListProfileByRfcCompany(long idCompany)
        {
            try
            {
                using (var db = new Db_EmisionEntities())
                {
                    var profile = db.Sys_Profile.Where(p => p.Sys_IdCompany == idCompany).ToList();
                    var profileDtos = new List<ProfileDto>();
                    profile.ForEach(p => profileDtos.Add(Common.Map<Sys_Profile, ProfileDto>(p)));
                    return profileDtos;
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
    }
}
