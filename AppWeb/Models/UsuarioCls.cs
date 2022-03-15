using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class UsuarioCls
    {

        public int IdUsuario { get; set; }
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Contra { get; set; }
        public string IdTipoUsuario { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdRol { get; set; }

        //Propiedad Adicional
        public string NombrePersona { get; set; }

        public string NombreRol { get; set; }

        public string NombreTipoEmpleado { get; set; }


    }
}