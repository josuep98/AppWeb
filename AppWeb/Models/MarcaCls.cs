using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class MarcaCls
    {
        //get y set obtiene los valores del mapeo de la tabla
        //se asigna tipos de datos iguales a los del mapeo
        [Display(Name = "Id Marca")]
        public int IidMarca { get; set; }
        [Display(Name = "Nombre Marca")]
        [Required]
        [StringLength(100, ErrorMessage = "La longitud máxima es 100")]
        public string Nombre { get; set; }
        [Display(Name = "Descripción Marca")]
        [Required]
        [StringLength(200, ErrorMessage = "La longitud máxima es 200")]
        public string Descripcion { get; set; }
        //El display se utiliza solo en las columnas que se van a listar
        public int BHabilitado { get; set; }

        //Propiedad errores de validación
        public string MensajeError { get; set; }


    }
}