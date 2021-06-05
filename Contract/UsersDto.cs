using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class UsersDto
    {
        public int Sys_Id { get; set; }
        public long Sys_IdCompany { get; set; }
        public string Sys_Rfc { get; set; }
        public string Sys_Usr { get; set; }
        public string Sys_Pass { get; set; }
        public string Sys_Email { get; set; }
        public Nullable<int> Sys_AccessAttempts { get; set; }
        public Nullable<System.DateTime> Sys_LockDate { get; set; }
        public System.DateTime Sys_CreationDate { get; set; }
        public Nullable<System.DateTime> Sys_LastLoggin { get; set; }
        public Nullable<int> Sys_Status { get; set; }
        public Nullable<int> Sys_Profile_Id { get; set; }
    }
}
