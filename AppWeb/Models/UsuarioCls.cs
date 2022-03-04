using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class UsuarioCls
    {

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Contra { get; set; }
        public string IdTipoUsuario { get; set; }
        public int Id { get; set; }
        public int IdRol { get; set; }

        //Propiedad Adicional
        public string NombrePersona { get; set; }

        public string NombreRol { get; set; }

        public string NombreTipoEmpleado { get; set; }  


    }
}