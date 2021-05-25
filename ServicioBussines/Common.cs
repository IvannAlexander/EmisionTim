using Contract;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public static class Common
    {
        static bool Inicializado;
        static object locker = new object();
        public static void ConfigureMap()
        {
            if (!Inicializado)
            {
                lock (locker)
                {
                    AutoMapper.Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<Sys_Certificate, CertificateDto>();
                        cfg.CreateMap<CertificateDto, Sys_Certificate>();

                        cfg.CreateMap<Sys_Client, ClientDto>();
                        cfg.CreateMap<ClientDto, Sys_Client>();

                        cfg.CreateMap<Sys_CompanyAddress, CompanyAddressDto>();
                        cfg.CreateMap<CompanyAddressDto, Sys_CompanyAddress>();

                        cfg.CreateMap<Sys_Company, CompanyDto>();
                        cfg.CreateMap<CompanyDto, Sys_Company>();

                        cfg.CreateMap<Sys_Complements, ComplementDto>();
                        cfg.CreateMap<ComplementDto, Sys_Complements>();

                        cfg.CreateMap<Sys_Menu, MenuDto>();
                        cfg.CreateMap<MenuDto, Sys_Menu>();

                        cfg.CreateMap<Sys_Menu_Profile, MenuProfileDto>();
                        cfg.CreateMap<MenuProfileDto, Sys_Menu_Profile>();

                        cfg.CreateMap<Sys_Profile, ProfileDto>();
                        cfg.CreateMap<ProfileDto, Sys_Profile>();

                        cfg.CreateMap<Sys_User, UsersDto>();
                        cfg.CreateMap<UsersDto, Sys_User>();
                    });
                    Inicializado = true;
                }
            }
        }

        public static T Map<K, T>(K dos)
        {
            ConfigureMap();
            return AutoMapper.Mapper.Map<K, T>(dos);
        }
        public static T Map<K, T>(K dos, T tres)
        {
            ConfigureMap();
            return AutoMapper.Mapper.Map<K, T>(dos, tres);
        }
    }
}
