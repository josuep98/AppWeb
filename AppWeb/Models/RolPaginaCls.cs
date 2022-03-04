using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class RolPaginaCls
    {
        [Display(Name = "Id Rol Página")]
        public int IdRolPagina { get; set; }
        [Required]
        public int IdRol { get; set; }
        [Required]
        public int IdPagina { get; set; }
        public int Bhabilitado { get; set; }

        //Propiedades adicionales
        [Display(Name = "Nombre Rol")]
        public string NombreRol { get; set; }
        [Display(Name = "Nombre Mensaje")]
        public string NombreMensaje { get; set; }

    }

}