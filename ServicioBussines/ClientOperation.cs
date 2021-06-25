using Contract;
using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class ClientOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ClientOperation));

        public List<ClientDto> GetClientByRfcOrName(long idCompany, string rfcCompany, string name, string rfcFind)
        {
            try
            {
                using (var context = new Db_EmisionEntities())
                {
                    var lst = context.Sys_Client.Where(p => p.Sys_IdCompany == idCompany && p.Sys_RfcCompany == rfcCompany && (p.Sys_Rfc.Contains(rfcFind) || (p.Sys_Tradename.Contains(name)))).ToList();
                    var clients = new List<ClientDto>();
                    lst.ForEach(p => clients.Add(Common.Map<Sys_Client, ClientDto>(p)));
                    return clients;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se recupero la información del cliente por RFC o Razón Social.");
            }
        }

        public List<ClientDto> GetClientsByCompany(long idCompany, string rfcCompany)
        {
            try
            {
                using (var context = new Db_EmisionEntities())
                {
                    var lst = context.Sys_Client.Where(p => p.Sys_IdCompany == idCompany && p.Sys_RfcCompany == rfcCompany).ToList();
                    var clients = new List<ClientDto>();
                    lst.ForEach(p => clients.Add(Common.Map<Sys_Client, ClientDto>(p)));
                    return clients;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception($"No se recupero la información del clientes para {rfcCompany}.");
            }
        }

        public int GetTotalClientByCompany(long idCompany, string rfcCompany)
        {
            try
            {
                using (var context = new Db_EmisionEntities())
                {
                    return context.Sys_Client.Where(p => p.Sys_IdCompany == idCompany && p.Sys_RfcCompany == rfcCompany).Count();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se recupero la información del cliente correctamente.");
            }
        }


        public string SaveClient(ClientDto client)
        {
            var answer = string.Empty;
            try
            {
                using (var context = new Db_EmisionEntities())
                {
                    var currentClient = context.Sys_Client.FirstOrDefault(p => p.Sys_IdCompany == client.Sys_IdCompany && p.Sys_RfcCompany == client.Sys_RfcCompany && p.Sys_Rfc == client.Sys_Rfc);

                    if (currentClient != null)
                    {
                        currentClient.Sys_BusinessName = client.Sys_BusinessName;
                        currentClient.Sys_Tradename = client.Sys_Tradename;
                        currentClient.Sys_Email = client.Sys_Email;
                        currentClient.Sys_Street = client.Sys_Street;
                        currentClient.Sys_ZipCode = client.Sys_ZipCode;
                        currentClient.Sys_Municipality = client.Sys_Municipality;
                        currentClient.Sys_State = client.Sys_State;
                        currentClient.Sys_Cologne = client.Sys_Cologne;
                        currentClient.Sys_Country = client.Sys_Country;
                        currentClient.Sys_ModificationDate = DateTime.Now;
                        currentClient.Sys_UserId = client.Sys_UserId;
                    }
                    else
                    {
                        var newClient = Common.Map<ClientDto, Sys_Client>(client);
                        context.Sys_Client.Add(newClient);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                answer = "Error al guardar cliente.";
            }
            return answer;
        }

    }
}
