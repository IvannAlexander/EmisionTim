using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class MenuDto
    {
        public int Sys_Id { get; set; }
        public string Sys_Name { get; set; }
        public string Sys_Controller { get; set; }
        public string Sys_Action { get; set; }
        public Nullable<int> Sys_Parent { get; set; }
    }
}
