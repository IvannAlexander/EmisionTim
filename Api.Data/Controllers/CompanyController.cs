using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Data.Controllers
{
    public class CompanyController : ApiController
    {
        [System.Web.Http.Route("Api/Company/CreateNewCompany")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult CreateNewCompany([FromBody]NewCompanyDto company)
        {
            Bussines.CompanyOperation companyOperation = new Bussines.CompanyOperation();
            var answer = companyOperation.CreateNewCompany(company);
            return Json(answer);
        }
    }
}
