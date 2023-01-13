using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class ReservaCls
    {
        public int IdViaje { get; set; }
        public string NombreArchivo { get; set; }
        public byte[] Foto { get; set; }
        public string LugarOrigen { get; set; }
        public string LugarDestino { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaViaje { get; set; }
        public string NombreBus { get; set; }
        public string DescripcionBus { get; set; }
        //Podemos realizar la resta de asientos y validar aquí que número de asientos disponibles 
        //sea menor al total de asientos mediante el get
        public int NumAsientosDis { get; set; }
        public int TotalAsientos { get; set; }
        public int IdBus { get; set; }
        public int Cantidad { get; set; }

    }
}