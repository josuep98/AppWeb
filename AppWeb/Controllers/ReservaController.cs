using AppWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Controllers
{
    public class ReservaController : Controller
    {
        // GET: Reserva - Listado
        public ActionResult Index()
        {
            ListarComboLugar();
            using (var Bd = new BDPasajeEntities())
            {
                var Reserva = (from Viaje in Bd.Viaje
                               join Lugar in Bd.Lugar
                               on Viaje.IIDLUGARORIGEN equals Lugar.IIDLUGAR
                               join Bus in Bd.Bus
                               on Viaje.IIDBUS equals Bus.IIDBUS
                               join LugarDestino in Bd.Lugar
                               on Viaje.IIDLUGARDESTINO equals LugarDestino.IIDLUGAR
                               where Viaje.BHABILITADO == 1
                               select new ReservaCls
                               {
                                   IdViaje = Viaje.IIDVIAJE,
                                   NombreArchivo = Viaje.nombrefoto,
                                   Foto = Viaje.FOTO,
                                   LugarOrigen = Lugar.NOMBRE,
                                   LugarDestino = LugarDestino.NOMBRE,
                                   Precio = (decimal)Viaje.PRECIO,
                                   FechaViaje = (DateTime)Viaje.FECHAVIAJE,
                                   NombreBus = Bus.PLACA,
                                   DescripcionBus = Bus.DESCRIPCION,
                                   TotalAsientos = (int)(Bus.NUMEROCOLUMNAS * Bus.NUMEROFILAS),
                                   NumAsientosDis = (int)Viaje.NUMEROASIENTOSDISPONIBLES
                               }).ToList();
                return View(Reserva);
            }
        }
        //Lugar
        public void ListarComboLugar()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Item in Bd.Lugar
                         where Item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Item.NOMBRE,
                             Value = Item.IIDLUGAR.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                //Pasa información del controller a mi lista
                ViewBag.ListaLugar = Lista;
            }
        }
    }
}