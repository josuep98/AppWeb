using System;
using System.Collections.Generic;
using System.IO;
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

        public string Guardar(ViajeCls objViajeCls, HttpPostedFileBase Foto, int Titulo)
        {
            string Mensaje = "";

            try
            {
                if (!ModelState.IsValid || (Foto == null && Titulo == -1))
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    if (Foto == null && Titulo == -1)
                    {
                        objViajeCls.Mensaje = "La foto es obligatorio";
                        Mensaje += "<ul><li>Debe ingresar una foto!</li></ul>";
                    }
                    Mensaje += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        Mensaje += "<li class='list-group-item'>" + item + "</li>";
                    }
                    Mensaje += "</ul>";
                }
                else
                {
                    byte[] FotoBd = null;
                    if (Foto != null)
                    {
                        BinaryReader Lector = new BinaryReader(Foto.InputStream);
                        FotoBd = Lector.ReadBytes((int)Foto.ContentLength);
                        using (var Bd = new BDPasajeEntities())
                        {
                            if (Titulo == -1)
                            {
                                Viaje objViaje = new Viaje();
                                objViaje.IIDBUS = objViajeCls.IdBus;
                                objViaje.IIDLUGARDESTINO = objViajeCls.IdLugarDestino;
                                objViaje.IIDLUGARORIGEN = objViajeCls.IdLugarOrigen;
                                objViaje.PRECIO = objViajeCls.Precio;
                                objViaje.FECHAVIAJE = objViajeCls.FechaViaje;
                                objViaje.NUMEROASIENTOSDISPONIBLES = objViajeCls.Numero;
                                objViaje.FOTO = FotoBd;
                                objViaje.nombrefoto = objViajeCls.NombreFoto;
                                objViaje.BHABILITADO = 1;
                                Bd.Viaje.Add(objViaje);
                                Mensaje = Bd.SaveChanges().ToString();
                                ListarCombos();
                                if (Mensaje == "0")
                                {
                                    Mensaje = "";
                                }
                            }
                            else
                            {
                                Viaje objViaje = Bd.Viaje.Where(p => p.IIDVIAJE == Titulo).First();
                                objViaje.IIDLUGARDESTINO = objViajeCls.IdLugarDestino;
                                objViaje.IIDLUGARORIGEN = objViajeCls.IdLugarOrigen;
                                objViaje.PRECIO = objViajeCls.Precio;
                                objViaje.FECHAVIAJE = objViajeCls.FechaViaje;
                                objViaje.NUMEROASIENTOSDISPONIBLES = objViajeCls.Numero;
                                if (Foto != null)
                                {
                                    objViaje.FOTO = FotoBd;
                                }
                                Mensaje = Bd.SaveChanges().ToString();
                            }
                        }
                    }

                    ListarCombos();
                }
            }
            catch (Exception ex)
            {
                Mensaje = "";
            }
            return Mensaje;
        }

        public ActionResult Filtrar(int LugarDestinoParametro)
        {
            List<ViajeCls> ListaViaje = new List<ViajeCls>();
            using (var Bd = new BDPasajeEntities())
            {
                if (LugarDestinoParametro == null)
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
                else
                {
                    ListaViaje = (from Viaje in Bd.Viaje
                                  join LugarOrigen in Bd.Lugar
                                  on Viaje.IIDLUGARORIGEN equals LugarOrigen.IIDLUGAR
                                  join LugarDestino in Bd.Lugar
                                  on Viaje.IIDLUGARDESTINO equals LugarDestino.IIDLUGAR
                                  join Bus in Bd.Bus
                                  on Viaje.IIDBUS equals Bus.IIDBUS
                                  where Viaje.BHABILITADO == 1 &&
                                  Viaje.IIDLUGARDESTINO == LugarDestinoParametro
                                  select new ViajeCls
                                  {
                                      IdViaje = Viaje.IIDVIAJE,
                                      NombreBus = Bus.PLACA,
                                      NombreOrigen = LugarOrigen.NOMBRE,
                                      NombreDestino = LugarDestino.NOMBRE
                                  }).ToList();
                }
            }
            return PartialView("_TablaViaje", ListaViaje);
        }

        public JsonResult RecuperarInformacion(int IdViaje)
        {
            ViajeCls objviajeCls = new ViajeCls();
            using (var Bd = new BDPasajeEntities())
            {
                Viaje objViaje = Bd.Viaje.Where(p => p.IIDVIAJE == IdViaje).First();
                objviajeCls.IdViaje = objViaje.IIDVIAJE;
                objviajeCls.IdBus = (int)objViaje.IIDBUS;
                objviajeCls.IdLugarDestino = (int)objViaje.IIDLUGARDESTINO;
                objviajeCls.IdLugarOrigen = (int)objViaje.IIDLUGARORIGEN;
                objviajeCls.Precio = (decimal)objViaje.PRECIO;
                //año-mes-día(pide)
                //bdd (dd-mm-yyyy)
                objviajeCls.FechaViajeCadena = ((DateTime)objViaje.FECHAVIAJE).ToString("yyyy-MM-dd");
                objviajeCls.Numero = (int)objViaje.NUMEROASIENTOSDISPONIBLES;
                objviajeCls.NombreFoto = objViaje.nombrefoto;

                objviajeCls.Extension = Path.GetExtension(objViaje.nombrefoto);
                objviajeCls.FotoRecuperarCadena = Convert.ToBase64String(objViaje.FOTO);
            }

            return Json(objviajeCls, JsonRequestBehavior.AllowGet);
        }

        public int EliminarViaje(int IdViaje)
        {
            int NumRegistrosAf = 0;
            try
            {
                using (var Bd = new BDPasajeEntities())
                {
                    Viaje objViaje = Bd.Viaje.Where(p => p.IIDVIAJE == IdViaje).First();
                    objViaje.BHABILITADO = 0;
                    //0 no se hizo nada, 1 se realizó correctamente
                    NumRegistrosAf = Bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NumRegistrosAf = 0;
            }
            return NumRegistrosAf;
        }

    }
}