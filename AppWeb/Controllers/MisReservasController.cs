using AppWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Controllers
{
    public class MisReservasController : Controller
    {
        // GET: MisReservas
        public ActionResult Index()
        {
            List<ReservaCls> listaReserva = new List<ReservaCls>();

            var PasajesId = ControllerContext.HttpContext.Request.Cookies["PasajesId"];
            var PasajesCantidad = ControllerContext.HttpContext.Request.Cookies["PasajesCantidad"];

            if (PasajesCantidad != null)
            {
                string valorId = PasajesId.Value;
                string valorCantidad = PasajesCantidad.Value;

                string[] arrayElementos = valorCantidad.Split('{');
                string[] arrayIds = valorId.Split('{');
                string[] reserva;
                if (arrayIds[0] != "")
                {
                    for (int i = 0; i < arrayElementos.Length; i++)
                    {
                        ReservaCls objReservaCls = new ReservaCls();
                        reserva = arrayElementos[i].Split('*');
                        objReservaCls.IdViaje = int.Parse(arrayIds[i]);
                        objReservaCls.Cantidad = int.Parse(reserva[0]);
                        objReservaCls.FechaViaje = DateTime.Parse(reserva[1]);
                        objReservaCls.LugarOrigen = reserva[2];
                        objReservaCls.LugarDestino = reserva[3];
                        objReservaCls.Precio = decimal.Parse(reserva[4]);

                        listaReserva.Add(objReservaCls);
                    }
                }
            }
            return View(listaReserva);
        }


        public ActionResult Filtrar()
        {
            List<ReservaCls> listaReserva = new List<ReservaCls>();

            var PasajesId = ControllerContext.HttpContext.Request.Cookies["PasajesId"];
            var PasajesCantidad = ControllerContext.HttpContext.Request.Cookies["PasajesCantidad"];

            if (PasajesCantidad != null)
            {
                string valorId = PasajesId.Value;
                string valorCantidad = PasajesCantidad.Value;

                string[] arrayElementos = valorCantidad.Split('{');
                string[] arrayIds = valorId.Split('{');
                string[] reserva;
                if (arrayIds[0] != "")
                {
                    for (int i = 0; i < arrayElementos.Length; i++)
                    {
                        ReservaCls objReservaCls = new ReservaCls();
                        reserva = arrayElementos[i].Split('*');
                        objReservaCls.IdViaje = int.Parse(arrayIds[i]);
                        objReservaCls.Cantidad = int.Parse(reserva[0]);
                        objReservaCls.FechaViaje = DateTime.Parse(reserva[1]);
                        objReservaCls.LugarOrigen = reserva[2];
                        objReservaCls.LugarDestino = reserva[3];
                        objReservaCls.Precio = decimal.Parse(reserva[4]);

                        listaReserva.Add(objReservaCls);
                    }
                }
            }
            return PartialView("_TablaMisReservas", listaReserva);
        }

        public string GuardarDatos(string Total)
        {
            string cadena = "";
            try
            {
                var PasajesId = ControllerContext.HttpContext.Request.Cookies["PasajesId"];
                var PasajesCantidad = ControllerContext.HttpContext.Request.Cookies["PasajesCantidad"];
                if (PasajesCantidad != null)
                {
                    string[] arrayElementos = PasajesCantidad.Value.Split('{');
                    string[] reserva;
                    string[] arrayIds = PasajesId.Value.Split('{');
                    using (var bTransaction = new TransactionScope())
                    {
                        using (var Bd = new BDPasajeEntities())
                        {
                            VENTA objVenta = new VENTA();
                            Usuario objUsuario = (Usuario)Session["Usuario"];
                            objVenta.TOTAL = decimal.Parse(Total);
                            objVenta.IIDUSUARIO = objUsuario.IIDUSUARIO;
                            objVenta.FECHAVENTA = DateTime.Now;
                            objVenta.BHABILITADO = 1;
                            Bd.VENTA.Add(objVenta);
                            Bd.SaveChanges();
                            int idVenta = objVenta.IIDVENTA;
                            for (int i = 0; i < arrayElementos.Length; i++)
                            {
                                reserva = arrayElementos[i].Split('*');
                                DETALLEVENTA objDetalleVenta = new DETALLEVENTA();
                                objDetalleVenta.IIDVENTA = idVenta;
                                int idViaje = int.Parse(arrayIds[i]);
                                objDetalleVenta.IIDVIAJE = idViaje;
                                objDetalleVenta.IIDBUS = int.Parse(reserva[5]);
                                int asientos = int.Parse(reserva[0]);
                                objDetalleVenta.cantidad = asientos;
                                objDetalleVenta.PRECIO = int.Parse((reserva[4]));
                                objDetalleVenta.BHABILITADO = 1;
                                Bd.DETALLEVENTA.Add(objDetalleVenta);
                                //Disminuir stock
                                Viaje objVentaBusqueda = Bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                                objVentaBusqueda.NUMEROASIENTOSDISPONIBLES = objVentaBusqueda.NUMEROASIENTOSDISPONIBLES - asientos;
                                Bd.SaveChanges();
                            }
                            Bd.SaveChanges();
                            bTransaction.Complete();
                            cadena = "OK";
                            HttpCookie cookieId = new HttpCookie("PasajesId", "");
                            HttpCookie cookieCantidad = new HttpCookie("PasajesCantidad", "");

                            ControllerContext.HttpContext.Response.SetCookie(cookieId);
                            ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);
                        }
                    }
                }
            }
            catch (Exception)
            {
                cadena = "";
            }
            return cadena;
        }

    }
}