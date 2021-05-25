using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicioModelo;

namespace EmisionWeb
{
    public class ClientesModif
    {
        public string Rfc { get; set; }
        public string Nombre { get; set; }
        public Sys_Cliente Cliente { get; set; }
        public List<Sys_Cliente> ListaClientes { get; set; }
    }
}