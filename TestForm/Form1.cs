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
                //var usuario = new Sys_Usuario
                //{
                //    Sys_Correo = "usuarioOk@test.com.mx",
                //    Sys_Estatus = 1,
                //    Sys_Fec_Creacion = DateTime.Now,
                //    Sys_Intentos = 0,
                //    Sys_Pass = "AABBcc22++",
                //    Sys_Usr = "Alex",
                //    Sys_Rfc = "AAA010101AAA"
                //};

                //var db = ClientFactory.GetCliente();
                //using (db as IDisposable)
                //{
                //    db.GuardarUsuario(usuario);
                //}
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
            //try
            //{

            //    var client = ClientFactory.GetCliente();
            //    using (client as IDisposable)
            //    {
            //        try
            //        {
            //            var usr = client.ObtenerUsuario("Alex", "MUFI880608267");
            //            usr.ToString();
            //        }
            //        catch (FaultException fe)
            //        {
            //            MessageBox.Show(fe.Message);
            //        }
            //        catch (CommunicationException ec)
            //        {
            //            MessageBox.Show(ec.Message);
            //        }
            //        catch (TimeoutException et)
            //        {
            //            MessageBox.Show(et.Message);
            //        }
            //        catch (Exception ee)
            //        {
            //            MessageBox.Show(ee.Message);
            //        }

            //    }
            //}
            //catch (FaultException fe)
            //{
            //    MessageBox.Show(fe.Message);
            //}
            //catch (CommunicationException ec)
            //{
            //    MessageBox.Show(ec.Message);
            //}
            //catch (TimeoutException et)
            //{
            //    MessageBox.Show(et.Message);
            //}
            //catch (Exception ee)
            //{
            //    MessageBox.Show(ee.Message);
            //}
        }

        private void btnCertificados_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    var certificado =
            //        @"Z:\Alexander\Mega\Personal\Trabajo\Proyectos\Mvc\Doc\Cert_Sellos\CSDAAA010101AAA\CSD01_AAA010101AAA.cer";
            //    var key =
            //        @"Z:\Alexander\Mega\Personal\Trabajo\Proyectos\Mvc\Doc\Cert_Sellos\CSDAAA010101AAA\CSD01_AAA010101AAA.key";
            //    var bCer = File.ReadAllBytes(certificado);
            //    var bKey = File.ReadAllBytes(key);
            //    var cliente = ClientFactory.GetCliente();
            //    using (cliente as IDisposable)
            //    {
            //        cliente.GuardaCertificados(bCer, bKey, "12345678a", "AAA010101AAA");
                    
            //    }
            //}
            //catch (FaultException fe)
            //{
            //    MessageBox.Show(fe.Message);
            //}
            //catch (CommunicationException ec)
            //{
            //    MessageBox.Show(ec.Message);
            //}
            //catch (TimeoutException et)
            //{
            //    MessageBox.Show(et.Message);
            //}
            //catch (Exception ee)
            //{
            //    MessageBox.Show(ee.Message);
            //}
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
            var pass = "AAA010101AAA*";
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
    }
}
