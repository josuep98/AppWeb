using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class SucursalCls
    {

        [Display(Name = "Id Sucursal")]
        public int IdSucursal { get; set; }
        [Display(Name = "Nombre Sucursal")]
        [Required]
        [StringLength(100, ErrorMessage = "Longitud máxima de 100 caracteres")]
        public string Nombre { get; set; }
        [Display(Name = "Dirección")]
        [Required]
        [StringLength(200, ErrorMessage = "Longitud máxima de 200 caracteres")]
        public string Direccion { get; set; }
        [Display(Name = "Teléfono")]
        [Required]
        [StringLength(10, ErrorMessage = "Longitud máxima de 10 caracteres")]
        public string Telefono { get; set; }
        [Display(Name = "Email Sucursal")]
        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un email válido")]
        [StringLength(100, ErrorMessage = "Longitud máxima de 100 caracteres")]
        public string Email { get; set; }
        [Display(Name = "Fecha de Apertura")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaApertura { get; set; }
        public int BHabilitado { get; set; }

        //Propiedad adicional

        public string MensajeError { get; set; }

    }
}