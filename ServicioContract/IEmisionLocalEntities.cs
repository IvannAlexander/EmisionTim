using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioModelo
{
    public interface IEmisionLocalEntities : System.IDisposable
    {
        DbSet<Sys_Usuario> Sys_Usuario { get; set; }
        DbSet<Sys_Certificado> Sys_Certificado { get; set; }
        DbSet<Sys_Dir_Empresa> Sys_Dir_Empresa { get; set; }
        DbSet<Sys_Empresa> Sys_Empresa { get; set; }
    }
}
