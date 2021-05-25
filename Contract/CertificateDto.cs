using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class CertificateDto
    {
        public int Sys_Id { get; set; }
        public string Sys_Rfc { get; set; }
        public string Sys_Number { get; set; }
        public string Sys_Name { get; set; }
        public string Sys_Certificate1 { get; set; }
        public string Sys_Key { get; set; }
        public string Sys_Pwd { get; set; }
        public System.DateTime Sys_RegistrationDate { get; set; }
        public System.DateTime Sys_RegistrationDateCert { get; set; }
        public System.DateTime Sys_ExpirationDateCert { get; set; }
    }
}
