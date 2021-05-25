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
using ServicioContract;

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
                var usuario = new Sys_Usuario
                {
                    Sys_Correo = "usuario@test.com.mx",
                    Sys_Estatus = 1,
                    Sys_Fec_Creacion = DateTime.Now,
                    Sys_Intentos = 0,
                    Sys_Pass = "AABBcc22++",
                    Sys_Usr = "AAA010101AAA",
                    Sys_Rfc = "AAA010101AAA"
                };

                var listaUsuario = new List<Sys_Usuario> {usuario};

                var empresa = new Sys_Empresa
                {
                    Sys_Rfc = "AAA010101AAA",
                    Sys_Nombre = "Test",
                    Sys_Activo = true,
                    Sys_Correo = "test@test.com",
                    Sys_Curp = "",
                    Sys_Usr_Modif = "Sistema",
                    Sys_Fec_Modf = DateTime.Now,
                    Sys_Pagina = "www.test.com.mx",
                    Sys_Fec_Registro = DateTime.Now,
                    Sys_Telefono = "57301245",
                    Sys_Regimen_Fiscal = "601",
                    //Sys_Usuario = listaUsuario
                };
                var db = ClientFactory.GetCliente();
                using (db as IDisposable)
                {
                    db.NuevaEmpresa(empresa);
                }
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
                var usuario = new Sys_Usuario
                {
                    Sys_Correo = "usuarioOk@test.com.mx",
                    Sys_Estatus = 1,
                    Sys_Fec_Creacion = DateTime.Now,
                    Sys_Intentos = 0,
                    Sys_Pass = "AABBcc22++",
                    Sys_Usr = "Alex",
                    Sys_Rfc = "AAA010101AAA"
                };

                var db = ClientFactory.GetCliente();
                using (db as IDisposable)
                {
                    db.GuardarUsuario(usuario);
                }
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

                var db = ClientFactory.GetCliente();
                using (db as IDisposable)
                {
                    var empresa = db.ObtenerEmpresaPorRfc("MUFI880608267");
                    empresa.ToString();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnObtenUsuario_Click(object sender, EventArgs e)
        {
            try
            {

                var client = ClientFactory.GetCliente();
                using (client as IDisposable)
                {
                    try
                    {
                        var usr = client.ObtenerUsuario("Alex", "MUFI880608267");
                        usr.ToString();
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

        private void btnCertificados_Click(object sender, EventArgs e)
        {
            try
            {
                var certificado =
                    @"Z:\Alexander\Mega\Personal\Trabajo\Proyectos\Mvc\Doc\Cert_Sellos\CSDAAA010101AAA\CSD01_AAA010101AAA.cer";
                var key =
                    @"Z:\Alexander\Mega\Personal\Trabajo\Proyectos\Mvc\Doc\Cert_Sellos\CSDAAA010101AAA\CSD01_AAA010101AAA.key";
                var bCer = File.ReadAllBytes(certificado);
                var bKey = File.ReadAllBytes(key);
                var cliente = ClientFactory.GetCliente();
                using (cliente as IDisposable)
                {
                    cliente.GuardaCertificados(bCer, bKey, "12345678a", "AAA010101AAA");
                    
                }
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
    }
}
