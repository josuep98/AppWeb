using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class BusCls
    {
        [Display(Name = "Id Bus")]
        public int IdBus { get; set; }
        [Display(Name = "Nombre Sucursal")]
        [Required]
        public int IdSucursal { get; set; }
        [Display(Name = "Tipo Bus")]
        [Required]
        public int IdTipoBus { get; set; }
        [Display(Name = "Placa")]
        [Required]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Placa { get; set; }

        [Display(Name = "Fecha Compra")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaCompra { get; set; }
        [Display(Name = "Nombre Modelo")]
        [Required]
        public int IdModelo { get; set; }
        [Display(Name = "Número de Filas")]
        [Required]
        public int NumeroFilas { get; set; }
        [Display(Name = "Número de Columnas")]
        [Required]
        public int NumeroColumnas { get; set; }
        public int Bhabilitado { get; set; }
        [Display(Name = "Descripción")]
        [Required]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Descripcion { get; set; }
        [Display(Name = "Observación")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Observacion { get; set; }
        [Display(Name = "Nombre Marca")]
        [Required]
        public int IdMarca { get; set; }

        //Propiedades adicionales
        [Display(Name = "Nombre Sucursal")]
        public string NombreSucursal { get; set; }
        [Display(Name = "Nombre Tipo Bus")]
        public string NombreTipoBus { get; set; }
        [Display(Name = "Nombre Modelo")]
        public string NombreModelo { get; set; }

        public string MensajeError { get; set; }

    }
}