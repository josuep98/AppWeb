using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models
{
    public class ClienteCls
    {

        [Display(Name = "Id Cliente")]
        public int IdCliente { get; set; }
        [Display(Name = "Nombres")]
        [Required]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombres { get; set; }
        [Display(Name = "Apellido Paterno")]
        [Required]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string ApellidoPa { get; set; }
        [Display(Name = "Apellido Materno")]
        [Required]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string ApellidoMa { get; set; }
        [Display(Name = "Email")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        [EmailAddress(ErrorMessage = "Ingrese un Email válido")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Dirección")]
        [DataType(DataType.MultilineText)]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        [Required]
        public string Direccion { get; set; }
        [Display(Name = "Sexo")]
        [Required]
        public int IdSexo { get; set; }
        [Display(Name = "Teléfono Domicilio")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        [Required]
        public string TelefonoFijo { get; set; }
        [Display(Name = "Celular")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres")]
        [Required]
        public string Celular { get; set; }
        public int BHabilitado { get; set; }
        public int BTieneUsuario { get; set; }
        public int TipoUsuario { get; set; }

        //Propiedad adicional
        public string MensajeError { get; set; }

    }
}