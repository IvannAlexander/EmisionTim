using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class NewCompanyDto
    {
        public CompanyDto Company { get; set; }
        public CompanyAddressDto CompanyAddress { get; set; }
    }
}
