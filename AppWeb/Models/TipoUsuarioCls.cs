using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class TipoUsuarioCls
    {
        [Display(Name = "Id Tipo Usuario")]
        public int IdTipoUsuario { get; set; }

        [Display(Name = "Nombre Usuario")]
        [Required]
        [StringLength(150, ErrorMessage = "Longitud máxima de 150")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        [StringLength(250, ErrorMessage = "Longitud máxima de 250")]
        public string Descripcion { get; set; }
        public string BHabilitado { get; set; }

    }
}