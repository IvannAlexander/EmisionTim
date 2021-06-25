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
    public class ClientController : ApiController
    {
        [System.Web.Http.Route("Api/Client/SaveClient")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult SaveClient([FromBody] ClientDto client)
        {
            var userOperation = new UserOperation();
            var user = userOperation.ValidateUser(client.User, client.Pwd, client.Sys_RfcCompany);
            if (user == null)
            {
                return Json("Usuario invalido");
            }
            client.Sys_IdCompany = user.Sys_IdCompany;
            client.Sys_UserId = user.Sys_Id;
            ClientOperation clientOperation = new ClientOperation();
            var answer = clientOperation.SaveClient(client);
            return Json(answer);
        }
        
        [System.Web.Http.Route("Api/Client/GetClientsByCompany")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetClientsByCompany(string usr, string pwd, string, string rfcCompany)
        {
            var userOperation = new UserOperation();
            var user = userOperation.ValidateUser(usr, pwd, rfcCompany);
            if (user == null)
            {
                return Json("Usuario invalido");
            }
            ClientOperation clientOperation = new ClientOperation();
            var answer = clientOperation.GetClientsByCompany(user.Sys_IdCompany, rfcCompany);
            return Json(answer);
        }

        [System.Web.Http.Route("Api/Client/GetClientByRfcOrName")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetClientByRfcOrName(string usr, string pwd, string rfcCompany, string name, string rfcClient)
        {
            var userOperation = new UserOperation();
            var user = userOperation.ValidateUser(usr, pwd, rfcCompany);
            if (user == null)
            {
                return Json("Usuario invalido");
            }
            ClientOperation clientOperation = new ClientOperation();
            var answer = clientOperation.GetClientByRfcOrName(user.Sys_IdCompany, rfcCompany, name, rfcClient);
            return Json(answer);
        }
    }
}
