using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class ViajeController : Controller
    {
        // GET: Viaje
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

        public void ListarComboBus()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Item in Bd.Bus
                         where Item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Item.PLACA,
                             Value = Item.IIDBUS.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                //Pasa información del controller a mi lista
                ViewBag.ListaBus = Lista;
            }
        }

        public void ListarCombos()
        {
            ListarComboBus();
            ListarComboLugar();
        }

        public ActionResult Index()
        {
            List<ViajeCls> ListaViaje = null;
            ListarCombos();
            using (var Bd = new BDPasajeEntities())
            {
                ListaViaje = (from Viaje in Bd.Viaje
                              join LugarOrigen in Bd.Lugar
                              on Viaje.IIDLUGARORIGEN equals LugarOrigen.IIDLUGAR
                              join LugarDestino in Bd.Lugar
                              on Viaje.IIDLUGARDESTINO equals LugarDestino.IIDLUGAR
                              join Bus in Bd.Bus
                              on Viaje.IIDBUS equals Bus.IIDBUS
                              where Viaje.BHABILITADO == 1
                              select new ViajeCls
                              {
                                  IdViaje = Viaje.IIDVIAJE,
                                  NombreBus = Bus.PLACA,
                                  NombreOrigen = LugarOrigen.NOMBRE,
                                  NombreDestino = LugarDestino.NOMBRE
                              }).ToList();
            }


            return View(ListaViaje);
        }

        public ActionResult Agregar()
        {
            ListarCombos();
            return View();
        }

    }
}