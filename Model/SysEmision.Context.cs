//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Db_EmisionEntities : DbContext
    {
        public Db_EmisionEntities()
            : base("name=Db_EmisionEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Sys_Certificate> Sys_Certificate { get; set; }
        public virtual DbSet<Sys_CompanyAddress> Sys_CompanyAddress { get; set; }
        public virtual DbSet<Sys_Complements> Sys_Complements { get; set; }
        public virtual DbSet<Sys_Menu> Sys_Menu { get; set; }
        public virtual DbSet<Sys_Menu_Profile> Sys_Menu_Profile { get; set; }
        public virtual DbSet<Sys_Profile> Sys_Profile { get; set; }
        public virtual DbSet<Sys_User> Sys_User { get; set; }
        public virtual DbSet<Sys_Company> Sys_Company { get; set; }
        public virtual DbSet<Sys_Invoice> Sys_Invoice { get; set; }
        public virtual DbSet<Sys_Client> Sys_Client { get; set; }
    }
}
