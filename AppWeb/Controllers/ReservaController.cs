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

            var PasajesId = ControllerContext.HttpContext.Request.Cookies["PasajesId"];
            var PasajesCantidad = ControllerContext.HttpContext.Request.Cookies["PasajesCantidad"];

            if (PasajesId != null)
            {
                ViewBag.ListaId = PasajesId.Value;
                ViewBag.ListaCantidad = PasajesCantidad.Value;
            }

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
                                   NumAsientosDis = (int)Viaje.NUMEROASIENTOSDISPONIBLES,
                                   IdBus = Bus.IIDBUS
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
        public string AgregarCookie(string IdViaje, string Cantidad, string FechaViaje, string LugarOrigen, string LugarDestino, string Precio, int IdBus)
        {
            string respuesta = "";

            try
            {
                var PasajesId = ControllerContext.HttpContext.Request.Cookies["PasajesId"];
                var PasajesCantidad = ControllerContext.HttpContext.Request.Cookies["PasajesCantidad"];
                if (PasajesId != null && PasajesCantidad != null && PasajesCantidad.Value != "" && PasajesId.Value != "")
                {
                    //Se crea por segunda vez
                    string IdCookie = PasajesId.Value + "{" + IdViaje;
                    string CantidadCookie = PasajesCantidad.Value + "{" + Cantidad + "*" + FechaViaje + "*" + LugarOrigen + "*" + LugarDestino + "*" +
                        Precio + "*" + IdBus;

                    HttpCookie cookieId = new HttpCookie("PasajesId", IdCookie);
                    HttpCookie cookieCantidad = new HttpCookie("PasajesCantidad", CantidadCookie);

                    ControllerContext.HttpContext.Response.SetCookie(cookieId);
                    ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);
                }
                else
                {
                    //Se crea por primera vez
                    string formatoCadena = Cantidad + "*" + FechaViaje + "*" + LugarOrigen + "*" + LugarDestino + "*" + Precio + "*" + IdBus;
                    HttpCookie cookieId = new HttpCookie("PasajesId", IdViaje);
                    HttpCookie cookieCantidad = new HttpCookie("PasajesCantidad", formatoCadena);

                    ControllerContext.HttpContext.Response.SetCookie(cookieId);
                    ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);
                }
                respuesta = "OK";
            }
            catch (Exception)
            {
                respuesta = "";
            }
            return respuesta;
        }

        public string QuitarCookie(string IdViaje)
        {
            string Respuesta = "";
            try
            {
                var PasajesId = ControllerContext.HttpContext.Request.Cookies["PasajesId"];
                var PasajesCantidad = ControllerContext.HttpContext.Request.Cookies["PasajesCantidad"];
                string valorId = PasajesId.Value;
                string valorCantidad = PasajesCantidad.Value;
                string[] arrayId = valorId.Split('{');
                int IndiceId = Array.IndexOf(arrayId, IdViaje);
                //6{7{9{2 - Ejemplo - Replace (cadena, '')
                string nuevoId = "";
                if (valorId.Contains("{" + IdViaje))
                {
                    nuevoId = valorId.Replace("{" + IdViaje, "");
                }
                else if (valorId.Contains(IdViaje + "{"))
                {
                    nuevoId = valorId.Replace(IdViaje + "{", "");
                }
                else
                {
                    nuevoId = valorId.Replace(IdViaje, "");
                }

                List<string> valor = valorCantidad.Split('{').ToList();
                valor.RemoveAt(IndiceId);
                string[] arrayCantidad = valor.ToArray();
                string nuevaCantidad = String.Join("{", arrayCantidad);

                HttpCookie cookieId = new HttpCookie("PasajesId", nuevoId);
                HttpCookie cookieCantidad = new HttpCookie("PasajesCantidad", nuevaCantidad);

                ControllerContext.HttpContext.Response.SetCookie(cookieId);
                ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);

                Respuesta = "OK";

            }
            catch (Exception ex)
            {
                Respuesta = "";
            }
            return Respuesta;
        }

    }
}