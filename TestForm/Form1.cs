using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bussines;
using Newtonsoft.Json;
using Contract;
using EmisionService;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAgregarEmpresa_Click(object sender, EventArgs e)
        {
            try
            {
                

                var company = new CompanyDto
                {
                    Sys_Rfc = "AAA010101AAA",
                    Sys_Name = "Test",
                    Sys_Status = true,
                    Sys_Email = "test@test.com",
                    Sys_Curp = "",
                    Sys_UserModified = "Sistema",
                    Sys_ModificationDate = DateTime.Now,
                    Sys_Page = "www.test.com.mx",
                    Sys_RegistrationDate = DateTime.Now,
                    Sys_Phone = "5557301245",
                    Sys_FiscalRegime = "601",
                    Sys_Balance = 0
                };

                CompanyAddressDto companyAddressDto = new CompanyAddressDto
                {
                    Sys_Cologne = "Colonia",
                    Sys_Country = "México",
                    Sys_ExternalNumber = "10",
                    Sys_Municipality = "Municipio",
                    Sys_ModificationDate = DateTime.Now,
                    Sys_Reference = "Referencia",
                    Sys_InternalNumber = "0",
                    Sys_RegistrationDate = DateTime.Now,
                    Sys_Rfc = "AAA010101AAA",
                    Sys_State = "Estado",
                    Sys_Street = "Calle",
                    Sys_User = "Sistema",
                    Sys_ZipCode = "51000"
                };

                NewCompanyDto newCompany = new NewCompanyDto
                {
                    Company = company,
                    CompanyAddress = companyAddressDto
                };
                var urlBase = "https://localhost:44368/Api/Company/CreateNewCompany";

                var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic VmFsZGV6QjpBQUJCY2MyMisr" },// + ConfigurationManager.AppSettings["Auth"] } ,
                    { "Content-Type","application/json" }
                };

                var json = JsonConvert.SerializeObject(newCompany);

                var data = RestClient.SendToService(urlBase, json, header, Encoding.UTF8);
                var answer = JsonConvert.DeserializeObject<string>(data);
                MessageBox.Show(answer);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        private void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                var user = new UsersDto
                {
                    Sys_Email = "usuarioOk@test.com.mx",
                    Sys_Status = 1,
                    Sys_CreationDate = DateTime.Now,
                    Sys_AccessAttempts = 0,
                    Sys_Pass = "AABBcc22++",
                    Sys_Usr = "Juan",
                    Sys_Rfc = "EKU9003173C9",
                    Sys_Profile_Id = 1
                };

                var urlBase = "https://localhost:44368/Api/User/SaveUser";

                var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic VmFsZGV6QjpBQUJCY2MyMisr" },// + ConfigurationManager.AppSettings["Auth"] } ,
                    { "Content-Type","application/json" }
                };

                var json = JsonConvert.SerializeObject(user);
                var data = RestClient.SendToService(urlBase, json, header, Encoding.UTF8);
                var answer = JsonConvert.DeserializeObject<string>(data);
                MessageBox.Show(answer);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnObtenEmpresa_Click(object sender, EventArgs e)
        {
            try
            {

                //var db = ClientFactory.GetCliente();
                //using (db as IDisposable)
                //{
                //    var empresa = db.ObtenerEmpresaPorRfc("MUFI880608267");
                //    empresa.ToString();
                //}
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnObtenUsuario_Click(object sender, EventArgs e)
        {
            var user = "Juan";
            var rfc = "EKU9003173C9";
            var url = $"https://localhost:44368/Api/User/GetUser?user={user}&rfcCompany={rfc}";
            var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic " },//+ ConfigurationManager.AppSettings["Auth"]},
                     { "Content-Type","application/json" }
                };
            var data = RestClient.GetFromService(url, header, Encoding.UTF8);
            var userVal = JsonConvert.DeserializeObject<UsersDto>(data);
            if (userVal == null)
            {
                MessageBox.Show("Usuario no existe.");
            }
            else
            {
                MessageBox.Show(userVal.Sys_Usr);
            }
        }

        private void btnCertificados_Click(object sender, EventArgs e)
        {
            try
            {
                var certificado =
                    @"C:\Users\ivann\Downloads\Csd-Pruebas\RFC-PAC-SC\Personas Morales\FIEL_EKU9003173C9_20190614160838\CSD_EKU9003173C9_20190617131829\CSD_Escuela_Kemper_Urgate_EKU9003173C9_20190617_131753s.cer";
                var key =
                    @"C:\Users\ivann\Downloads\Csd-Pruebas\RFC-PAC-SC\Personas Morales\FIEL_EKU9003173C9_20190614160838\CSD_EKU9003173C9_20190617131829\CSD_Escuela_Kemper_Urgate_EKU9003173C9_20190617_131753.key";
                var bCer = File.ReadAllBytes(certificado);
                var bKey = File.ReadAllBytes(key);

                LoadCertificateDto loadCertificateDto = new LoadCertificateDto
                {
                    FileCer = bCer,
                    FileKey = bKey,
                    RfcCompany = "EKU9003173C9",
                    Pass = "12345678a"
                };
                var urlBase = "https://localhost:44368/Api/Certificate/SaveCertificate";

                var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic VmFsZGV6QjpBQUJCY2MyMisr" },// + ConfigurationManager.AppSettings["Auth"] } ,
                    { "Content-Type","application/json" }
                };

                var json = JsonConvert.SerializeObject(loadCertificateDto);

                var data = RestClient.SendToService(urlBase, json, header, Encoding.UTF8);
                var answer = JsonConvert.DeserializeObject<string>(data);
                MessageBox.Show(answer);
            }
            catch (FaultException fe)
            {
                MessageBox.Show(fe.Message);
            }
            catch (CommunicationException ec)
            {
                MessageBox.Show(ec.Message);
            }
            catch (TimeoutException et)
            {
                MessageBox.Show(et.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnConvertir_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (FaultException fe)
            {
                MessageBox.Show(fe.Message);
            }
            catch (CommunicationException ec)
            {
                MessageBox.Show(ec.Message);
            }
            catch (TimeoutException et)
            {
                MessageBox.Show(et.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            var user = "AdminAAA010101AAA";
            var pass = "AAA010101AAA";
            var rfc = "AAA010101AAA";
            var url = $"https://localhost:44368/Api/User/ValidateUser?user={user}&pass={pass}&rfcCompany={rfc}";
            var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic " },//+ ConfigurationManager.AppSettings["Auth"]},
                     { "Content-Type","application/json" }
                };
            var data = RestClient.GetFromService(url, header, Encoding.UTF8);
            var userVal = JsonConvert.DeserializeObject<UsersDto>(data);
            if (userVal == null)
            {
                MessageBox.Show("Usuario o Contraseña incorrecto.");
            }
            else
            {
                MessageBox.Show(userVal.Sys_Usr);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rfc = "EKU9003173C9";
            var url = $"https://localhost:44368/Api/Certificate/GetCertificateByRfc?rfc={rfc}";
            var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic " },//+ ConfigurationManager.AppSettings["Auth"]},
                     { "Content-Type","application/json" }
                };
            var data = RestClient.GetFromService(url, header, Encoding.UTF8);
            var cert = JsonConvert.DeserializeObject<List<CertificateDto>>(data);
            if (cert == null)
            {
                MessageBox.Show($"No hay certificado con el RFC {rfc}.");
            }
            else
            {
                MessageBox.Show(cert[0].Sys_Rfc);
            }
        }

        private void btnCrearPerfil_Click(object sender, EventArgs e)
        {
            var name = "Perfil2";
            var rfcCompany = "AAA010101AAA";
            var urlBase = $"https://localhost:44368/Api/Profile/CreateProfile?name={name}&rfcCompany={rfcCompany}";

            var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic VmFsZGV6QjpBQUJCY2MyMisr" },// + ConfigurationManager.AppSettings["Auth"] } ,
                    { "Content-Type","application/json" }
                };

            var data = RestClient.SendToService(urlBase, string.Empty, header, Encoding.UTF8);
            var answer = JsonConvert.DeserializeObject<string>(data);
            MessageBox.Show(answer);
        }

        private void btnObtenerPerfiles_Click(object sender, EventArgs e)
        {
            var rfc = "AAA010101AAA";
            var url = $"https://localhost:44368/Api/Profile/GetListProfileByRfcCompany?rfcCompany={rfc}";
            var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic " },//+ ConfigurationManager.AppSettings["Auth"]},
                     { "Content-Type","application/json" }
                };
            var data = RestClient.GetFromService(url, header, Encoding.UTF8);
            var cert = JsonConvert.DeserializeObject<List<ProfileDto>>(data);
            if (cert == null)
            {
                MessageBox.Show($"No hay certificado con el RFC {rfc}.");
            }
            else
            {
                MessageBox.Show(cert[0].Sys_Rfc);
            }
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            var cfdi = File.ReadAllText(@"C:\Users\ivann\Downloads\new 3.xml");
            BillingDto billingDto = new BillingDto { XmlRequest = cfdi };
            var url = $"https://localhost:44368/Api/Billing/GetPdfFromXml";
            var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic " },//+ ConfigurationManager.AppSettings["Auth"]},
                     { "Content-Type","application/json" }
                };
            var json = JsonConvert.SerializeObject(billingDto);
            var data = RestClient.SendToService(url, json, header, Encoding.UTF8);
            var pdf = JsonConvert.DeserializeObject<byte[]>(data);
            File.WriteAllBytes(@"D:\Eliminar\ApiTestPdf.pdf", pdf);
        }

        private void btnCfdi_Click(object sender, EventArgs e)
        {
            Comprobante comprobante = new Comprobante
            {
                Version = "3.3",
                Serie = "A",
                Folio = "01",
                Fecha = DateTime.Now.ToString("s"),
                FormaPago = "99",
                MetodoPago = "PUE",
                CondicionesDePago = "CONTADO",
                SubTotal = 1,
                Moneda = "MXN",
                Total = 1.16M,
                TipoDeComprobante = "I",
                LugarExpedicion = "01001",
                Emisor = new ComprobanteEmisor
                {
                    Rfc = "EKU9003173C9",
                    Nombre = "Certificado de pruebas",
                    RegimenFiscal = "601",
                },
                Receptor = new ComprobanteReceptor
                {
                    Rfc = "PJM391026PK5",
                    Nombre = "Receptor de pruebas",
                    UsoCFDI = "G02"
                },
                Conceptos = new List<ComprobanteConcepto>
                {
                    new ComprobanteConcepto
                    {
                        Cantidad=1,
                        ClaveProdServ="01010101",
                        ClaveUnidad ="ACT",
                        Unidad="NO IDENTIFICADO",
                        Descripcion ="COMISION POR EMISION DE EDO DE CUENTA",
                        ValorUnitario = 1.00M,
                        Importe=1.00M,
                        Impuestos= new ComprobanteConceptoImpuestos
                        {
                            Traslados= new List<ComprobanteConceptoImpuestosTraslado>
                            {
                                new ComprobanteConceptoImpuestosTraslado
                                {
                                    Base =1.00M,
                                    Impuesto="002",
                                    TipoFactor ="Tasa",
                                    TasaOCuotaSpecified=true,
                                    TasaOCuota=0.160000M,
                                    ImporteSpecified=true,
                                    Importe=0.16M
                                }
                            }
                        }
                    }
                },
                Impuestos = new ComprobanteImpuestos
                {
                    TotalImpuestosTrasladados = 0.16M,
                    TotalImpuestosTrasladadosSpecified = true,
                    Traslados = new List<ComprobanteImpuestosTraslado>
                    {
                        new ComprobanteImpuestosTraslado
                        {
                            Impuesto="002",
                            TipoFactor ="Tasa",
                            TasaOCuota = 0.160000M,
                            Importe = 0.16M
                        }
                    }
                }
            };

            var urlBase = "https://localhost:44368/Api/Billing/CreateCfdi";

            var header = new Dictionary<string, string>()
                {
                    { "Authorization","Basic VmFsZGV6QjpBQUJCY2MyMisr" },// + ConfigurationManager.AppSettings["Auth"] } ,
                    { "Content-Type","application/json" }
                };

            var serializacion = new SerializationOperation();
            var xml =serializacion.Serializar<Comprobante>(comprobante);
            BillingDto billingDto = new BillingDto { XmlRequest = xml.ToString() };
            var json = JsonConvert.SerializeObject(billingDto);

            var data = RestClient.SendToService(urlBase, json, header, Encoding.UTF8);
            var answer = JsonConvert.DeserializeObject<string>(data);
            File.WriteAllText(@"D:\Eliminar\xmlApiPrueba.xml", answer);
        }
    }
}
