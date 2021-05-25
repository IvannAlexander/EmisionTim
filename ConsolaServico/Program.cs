using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using log4net;
using log4net.Config;
using ServicioContract;

namespace ServicioConsola
{
    class Program
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            try
            {
                ServicePointManager.DefaultConnectionLimit = 300;
                XmlConfigurator.Configure();
                Logger.Debug("Iniciando servicio");
                var tcpBinding = new NetTcpBinding { TransactionFlow = false };
                tcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                tcpBinding.Security.Mode = SecurityMode.None;
                var readerQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxDepth = 32,
                    MaxStringContentLength = int.MaxValue,
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = int.MaxValue,
                    MaxNameTableCharCount = int.MaxValue
                };
                tcpBinding.ReaderQuotas = readerQuotas;
                tcpBinding.MaxReceivedMessageSize = int.MaxValue;
                tcpBinding.MaxBufferPoolSize = int.MaxValue;
                tcpBinding.MaxBufferSize = int.MaxValue;
                tcpBinding.ReceiveTimeout = TimeSpan.MaxValue;//TimeSpan.FromSeconds(280);
                tcpBinding.SendTimeout = TimeSpan.MaxValue;//TimeSpan.FromSeconds(280);
                tcpBinding.CloseTimeout = TimeSpan.MaxValue;//TimeSpan.FromSeconds(280);
                var puerto = ConfigurationManager.AppSettings["Port"];
                var uriLocal = "net.tcp://localhost:" + puerto + "/Emision";
                Logger.Info("Uri:" + uriLocal);
                //System.Console.WriteLine(typeof(DmmTraceListener).AssemblyQualifiedName);
                var host = new ServiceHost(typeof(LocalServiceProcess));
                host.AddServiceEndpoint(typeof(IServicioLocal), tcpBinding, uriLocal);
                var debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();

                // if not found - add behavior with setting turned on 
                if (debug == null)
                {
                    host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                }
                else
                {
                    // make sure setting is turned ON
                    if (!debug.IncludeExceptionDetailInFaults)
                    {
                        debug.IncludeExceptionDetailInFaults = true;
                    }
                }
                host.Open();
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                Console.WriteLine(ee);
            }
            Console.ReadKey();
        }
    }
}
