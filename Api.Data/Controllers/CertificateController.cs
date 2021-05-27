using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Data.Controllers
{
    public class CertificateController : ApiController
    {
        [System.Web.Http.Route("Api/Certificate/SaveCertificate")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult SaveCertificate([FromBody]LoadCertificateDto loadCertificate)
        {
            Bussines.CertificateOperation certificateOperation = new Bussines.CertificateOperation();
            var answer = certificateOperation.SaveCertificate(loadCertificate.FileCer, loadCertificate.FileKey, loadCertificate.Pass, loadCertificate.RfcCompany);
            return Json(answer);
        }

        [System.Web.Http.Route("Api/Certificate/GetCertificateByRfc")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCertificateByRfc(string rfc)
        {
            Bussines.CertificateOperation certificateOperation = new Bussines.CertificateOperation();
            var answer = certificateOperation.GetCertificateByRfc(rfc);
            return Json(answer);
        }
    }
}
