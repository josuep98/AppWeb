using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class RolCls
    {
        [Display(Name = "IdRol")]
        public int IdRol { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        public int BHabilitado { get; set; }

    }
}