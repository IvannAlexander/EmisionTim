using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class LoadCertificateDto
    {
        public byte[] FileCer { get; set; }
        public byte[] FileKey { get; set; }
        public string Pass { get; set; }
        public string RfcCompany { get; set; }
        public long Sys_IdCompany { get; set; }
    }
}
