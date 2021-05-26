using Bussines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Data.Controllers
{
    public class UserController : ApiController
    {
        [System.Web.Http.Route("Api/User/ValidateUser")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult ValidateUser(string user, string pass, string rfcCompany)
        {
            UserOperation userOperation = new UserOperation();
            var userVal = userOperation.ValidateUser(user, pass, rfcCompany);
            return Json(userVal);
        }
    }
}
