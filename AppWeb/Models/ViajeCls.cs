using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class ViajeCls
    {
        [Display(Name = "Id Viaje")]
        public int IdViaje { get; set; }
        [Display(Name = "Lugar Origen")]
        [Required]
        public int IdLugarOrigen { get; set; }
        [Display(Name = "Lugar Destino")]
        [Required]
        public int IdLugarDestino { get; set; }
        [Display(Name = "Precio")]
        [Required]
        [Range(0, 10000, ErrorMessage = "Precio fuera de rango!")]
        public decimal Precio { get; set; }
        [Display(Name = "Fecha de viaje")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaViaje { get; set; }
        [Display(Name = "Bus")]
        [Required]
        public int IdBus { get; set; }
        [Display(Name = "Número asientos disponibles")]
        [Required]
        public int Numero { get; set; }

        //Propiedades adicionales
        [Display(Name = "Lugar Origen")]
        public string NombreOrigen { get; set; }
        [Display(Name = "Lugar Destino")]
        public string NombreDestino { get; set; }
        [Display(Name = "Nombre Bus")]
        public string NombreBus { get; set; }
        public string NombreFoto { get; set; }

        public string Mensaje { get; set; }
        public string FechaViajeCadena { get; set; }
        public string Extension { get; set; }
        public string FotoRecuperarCadena { get; set; }

    }
}