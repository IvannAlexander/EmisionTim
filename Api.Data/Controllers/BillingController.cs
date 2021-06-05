using Bussines;
using Contract;
using EmisionService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Data.Controllers
{
    public class BillingController : ApiController
    {

        [System.Web.Http.Route("Api/Billing/GetPdfFromXml")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult GetPdfFromXml([System.Web.Http.FromBody]BillingDto cfdi)
        {
            var operacionesPdf = new PdfOperation();
            //var result = System.Text.Encoding.UTF8.GetString(cfdi);
            var answer = operacionesPdf.GenerarPdfFactura(cfdi.XmlRequest);
            return Json(answer);
        }

        [System.Web.Http.Route("Api/Billing/CreateCfdi")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult CreateCfdi([System.Web.Http.FromBody] BillingDto cfdi)
        {
            var userOperation = new UserOperation();
            var user = userOperation.ValidateUser(cfdi.Usr, cfdi.Pwd, cfdi.RfcCompany);
            if (user == null)
            {
                return Json("Usuario invalido");
            }
            var certificateOperation = new CertificateOperation();
            var answer = certificateOperation.CreateCfdi(cfdi.XmlRequest, user.Sys_IdCompany);
            return Json(answer);
        }
    }
}
