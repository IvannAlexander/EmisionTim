//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServicioModelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sys_Usuario
    {
        public int Sys_Id { get; set; }
        public string Sys_Rfc { get; set; }
        public string Sys_Usr { get; set; }
        public string Sys_Pass { get; set; }
        public string Sys_Correo { get; set; }
        public Nullable<int> Sys_Intentos { get; set; }
        public Nullable<System.DateTime> Sys_Fec_Bloqueo { get; set; }
        public System.DateTime Sys_Fec_Creacion { get; set; }
        public Nullable<System.DateTime> Sys_Ultimo_Login { get; set; }
        public Nullable<int> Sys_Estatus { get; set; }
        public Nullable<int> Sys_Sistema_Id { get; set; }
        public Nullable<int> Sys_Perfil_Id { get; set; }
    }
}
