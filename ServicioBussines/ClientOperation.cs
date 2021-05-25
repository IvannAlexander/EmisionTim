using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines
{
    public class ClientOperation
    {
        //protected static ILog Logger = LogManager.GetLogger(typeof(MenuOperation));

        //public List<Sys_Cliente> ObtenerClientePorRfcNombre(string rfc, string razonSocial, string rfcEmpresa)
        //{
        //    try
        //    {
        //        using (var db = new Entities())
        //        {
        //            var listaCliente = db.Sys_Cliente.Where(p => p.Sys_Empresa_Rfc == rfcEmpresa && (p.Sys_RFC.Contains(rfc) || (p.Sys_Razon_Social.Contains(razonSocial)))).ToList();
        //            return listaCliente;
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("No se recupero la información del cliente correctamente.");
        //    }
        //}

        //public List<Sys_Cliente> ObtenerClientePorRfcEmpresa(string rfcEmpresa)
        //{
        //    try
        //    {
        //        using (var db = new Entities())
        //        {
        //            var listaCliente = db.Sys_Cliente.Where(p => p.Sys_Empresa_Rfc == rfcEmpresa).ToList();
        //            return listaCliente;
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("No se recupero la información del cliente correctamente.");
        //    }
        //}

        //public int ObtenerTotalClientePorRfcEmpresa(string rfcEmpresa)
        //{
        //    try
        //    {
        //        using (var db = new Entities())
        //        {
        //            return db.Sys_Cliente.Where(p => p.Sys_Empresa_Rfc == rfcEmpresa).Count();
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("No se recupero la información del cliente correctamente.");
        //    }
        //}

        //public void GuardarClienteNuevo(Sys_Cliente cliente)
        //{
        //    try
        //    {
        //        using (var db = new Entities())
        //        {
        //            db.Sys_Cliente.Add(cliente);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("Error al guardar nuevo cliente.");
        //    }
        //}

        //public void GuardarModificacionesDelCliente(Sys_Cliente cliente)
        //{
        //    try
        //    {
        //        using (var db = new Entities())
        //        {
        //            var clienteActual = db.Sys_Cliente.FirstOrDefault(p => p.Sys_Empresa_Rfc == cliente.Sys_Empresa_Rfc && p.Sys_RFC == cliente.Sys_RFC);

        //            if (clienteActual != null)
        //            {
        //                clienteActual.Sys_Razon_Social = cliente.Sys_Razon_Social;
        //                clienteActual.Sys_Nombre_Comercial = cliente.Sys_Nombre_Comercial;
        //                clienteActual.Sys_Correo = cliente.Sys_Correo;
        //                clienteActual.Sys_Calle = cliente.Sys_Calle;
        //                clienteActual.Sys_CP = cliente.Sys_CP;
        //                clienteActual.Sys_Municipio = cliente.Sys_Municipio;
        //                clienteActual.Sys_Estado = cliente.Sys_Estado;
        //                clienteActual.Sys_Colonia = cliente.Sys_Colonia;
        //                clienteActual.Sys_Pais = cliente.Sys_Pais;
        //                clienteActual.Sys_Fecha_Modificacion = DateTime.Now;
        //                clienteActual.Sys_Usuario_Id = cliente.Sys_Usuario_Id;
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Logger.Error(ee);
        //        throw new Exception("Error al guardar nuevo cliente.");
        //    }
        //}

    }
}
