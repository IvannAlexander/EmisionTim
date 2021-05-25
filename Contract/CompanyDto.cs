using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class CompanyDto
    {
        public int Sys_Id { get; set; }
        public string Sys_Rfc { get; set; }
        public string Sys_Name { get; set; }
        public string Sys_FiscalRegime { get; set; }
        public string Sys_Curp { get; set; }
        public string Sys_Page { get; set; }
        public string Sys_Email { get; set; }
        public string Sys_Phone { get; set; }
        public string Sys_UserModified { get; set; }
        public System.DateTime Sys_RegistrationDate { get; set; }
        public Nullable<System.DateTime> Sys_ModificationDate { get; set; }
        public Nullable<bool> Sys_Status { get; set; }
        public Nullable<int> Sys_Balance { get; set; }
    }
}
