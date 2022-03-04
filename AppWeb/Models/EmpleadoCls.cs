using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class EmpleadoCls
    {
        [Display(Name = "Id Empleado")]
        public int IdEmpleado { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }
        [Display(Name = "Apellido Paterno")]
        [Required]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string ApPaterno { get; set; }
        [Display(Name = "Apellido Materno")]
        [Required]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string ApMaterno { get; set; }
        [Display(Name = "Fecha Contrato")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaContrato { get; set; }
        [Display(Name = "Tipo Usuario")]
        [Required]
        public int IdTipoUsuario { get; set; }

        [Display(Name = "Sueldo")]
        [Required]
        [Range(0, 2500, ErrorMessage = "El sueldo es superior al permitido")]
        public decimal Sueldo { get; set; }

        [Display(Name = "Tipo Contrato")]
        [Required]
        public int IdTipoContrato { get; set; }
        [Display(Name = "Sexo")]
        [Required]
        public int IdSexo { get; set; }
        public int BHabilitado { get; set; }


        //Propiedades adicionales
        [Display(Name = "Tipo Contrato")]
        public string NombreTipoContrato { get; set; }
        [Display(Name = "Tipo Usuario")]
        public string NombreTipoUsuario { get; set; }
        public string MensajeError { get; set; }

    }
}