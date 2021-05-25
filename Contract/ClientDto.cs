using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class ClientDto
    {
        public int Sys_Id { get; set; }
        public string Sys_Rfc { get; set; }
        public string Sys_BusinessName { get; set; }
        public string Sys_Tradename { get; set; }
        public string Sys_Email { get; set; }
        public string Sys_Street { get; set; }
        public string Sys_ZipCode { get; set; }
        public string Sys_Municipality { get; set; }
        public string Sys_State { get; set; }
        public string Sys_Cologne { get; set; }
        public string Sys_Country { get; set; }
        public System.DateTime Sys_RegistrationDate { get; set; }
        public System.DateTime Sys_ModificationDate { get; set; }
        public int Sys_UserId { get; set; }
        public string Sys_RfcCompany { get; set; }
    }
}
