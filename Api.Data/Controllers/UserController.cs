using Bussines;
using Contract;
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

        [System.Web.Http.Route("Api/User/SaveUser")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult SaveUser(UsersDto user)
        {
            UserOperation userOperation = new UserOperation();
            var userVal = userOperation.SaveUser(user);
            return Json(userVal);
        }

        [System.Web.Http.Route("Api/User/GetUser")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetUser(string user, long idCompany)
        {
            UserOperation userOperation = new UserOperation();
            var userVal = userOperation.GetUser(user, idCompany);
            return Json(userVal);
        }

        [System.Web.Http.Route("Api/Profile/CreateProfile")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult CreateProfile(string name, long idCompany)
        {
            UserOperation userOperation = new UserOperation();
            var answer = userOperation.CreateProfile(name, idCompany);
            return Json(answer);
        }

        [System.Web.Http.Route("Api/Profile/GetListProfileByRfcCompany")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetListProfileByRfcCompany(long idCompany)
        {
            UserOperation userOperation = new UserOperation();
            var answer = userOperation.GetListProfileByRfcCompany(idCompany);
            return Json(answer);
        }
    }
}
