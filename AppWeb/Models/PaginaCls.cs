using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class PaginaCls
    {
        [Display(Name = "Id Página")]
        public int IdPagina { get; set; }
        [Required]
        [Display(Name = "Titulo del link")]
        public string Mensaje { get; set; }
        [Required]
        [Display(Name = "Nombre de Acción")]
        public string Accion { get; set; }
        [Required]
        [Display(Name = "Nombre del controlador")]
        public string Controlador { get; set; }
        public int BHabilitado { get; set; }

        //Propiedad adicional
        public string MensajeFiltrar { get; set; }

    }
}