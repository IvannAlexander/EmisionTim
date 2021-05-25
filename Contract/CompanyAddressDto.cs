using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class CompanyAddressDto
    {
        public int Sys_Id { get; set; }
        public string Sys_Rfc { get; set; }
        public string Sys_ZipCode { get; set; }
        public string Sys_Street { get; set; }
        public string Sys_ExternalNumber { get; set; }
        public string Sys_InternalNumber { get; set; }
        public string Sys_Cologne { get; set; }
        public string Sys_Municipality { get; set; }
        public string Sys_Reference { get; set; }
        public string Sys_State { get; set; }
        public string Sys_Country { get; set; }
        public string Sys_User { get; set; }
        public System.DateTime Sys_RegistrationDate { get; set; }
        public Nullable<System.DateTime> Sys_ModificationDate { get; set; }
    }
}
