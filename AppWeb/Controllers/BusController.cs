using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class BusController : Controller
    {
        // GET: Bus

        public void ListarComboSucursal()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Item in Bd.Sucursal
                         where Item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Item.NOMBRE,
                             Value = Item.IIDSUCURSAL.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.ListaSucursal = Lista;
            }
        }
        public void ListarComboTipoBus()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Item in Bd.TipoBus
                         where Item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Item.NOMBRE,
                             Value = Item.IIDTIPOBUS.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.ListaTipoBus = Lista;
            }
        }

        public void ListarComboMarca()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Item in Bd.Marca
                         where Item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Item.NOMBRE,
                             Value = Item.IIDMARCA.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.ListaMarca = Lista;
            }
        }

        public void ListarComboModelo()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Item in Bd.Modelo
                         where Item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Item.NOMBRE,
                             Value = Item.IIDMODELO.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.ListaModelo = Lista;
            }
        }

        public void ListarCombos()
        {
            ListarComboSucursal();
            ListarComboTipoBus();
            ListarComboMarca();
            ListarComboModelo();
        }
        public ActionResult Agregar()
        {
            ListarCombos();
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(BusCls objBusCls)
        {
            string Placa = objBusCls.Placa;
            int NumRegistros = 0;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegistros = Bd.Bus.Where(p => p.PLACA.Equals(Placa)).Count();
            }

            if (!ModelState.IsValid || NumRegistros >= 1)
            {
                if (NumRegistros >= 1) objBusCls.MensajeError = "La plca ingresada ya existe";
                ListarCombos();
                return View(objBusCls);
            }

            using (var Bd = new BDPasajeEntities())
            {
                Bus objBus = new Bus();

                objBus.IIDSUCURSAL = objBusCls.IdSucursal;
                objBus.IIDTIPOBUS = objBusCls.IdTipoBus;
                objBus.PLACA = objBusCls.Placa;
                objBus.FECHACOMPRA = objBusCls.FechaCompra;
                objBus.IIDMODELO = objBusCls.IdModelo;
                objBus.NUMEROFILAS = objBusCls.NumeroFilas;
                objBus.NUMEROCOLUMNAS = objBusCls.NumeroColumnas;
                objBus.DESCRIPCION = objBusCls.Descripcion;
                objBus.OBSERVACION = objBusCls.Observacion;
                objBus.IIDMARCA = objBusCls.IdMarca;

                objBus.BHABILITADO = 1;

                Bd.Bus.Add(objBus);
                Bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int Id)
        {
            ListarCombos();
            BusCls objBusCls = new BusCls();
            using (var Bd = new BDPasajeEntities())
            {
                Bus objBus = Bd.Bus.Where(p => p.IIDBUS.Equals(Id)).First();
                objBusCls.IdBus = objBus.IIDBUS;
                objBusCls.IdSucursal = (int)objBus.IIDSUCURSAL;
                objBusCls.IdTipoBus = (int)objBus.IIDTIPOBUS;
                objBusCls.Placa = objBus.PLACA;
                objBusCls.FechaCompra = (DateTime)objBus.FECHACOMPRA;
                objBusCls.IdModelo = (int)objBus.IIDMODELO;
                objBusCls.NumeroFilas = (int)objBus.NUMEROFILAS;
                objBusCls.NumeroColumnas = (int)objBus.NUMEROCOLUMNAS;
                objBusCls.Descripcion = objBus.DESCRIPCION;
                objBusCls.Observacion = objBus.OBSERVACION;
                objBusCls.IdMarca = (int)objBus.IIDMARCA;


            }
            return View(objBusCls);
        }

        [HttpPost]
        public ActionResult Editar(BusCls objBusCls)
        {
            int IdBus = objBusCls.IdBus;
            string Placa = objBusCls.Placa;
            int NumRegistros = 0;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegistros = Bd.Bus.Where(p => p.PLACA.Equals(Placa) && !p.IIDBUS.Equals(IdBus)).Count();
            }

            if (!ModelState.IsValid || NumRegistros >= 1)
            {
                if (NumRegistros >= 1) objBusCls.MensajeError = "Le placa ingresada ya existe";
                ListarCombos();
                return View(objBusCls);
            }

            using (var Bd = new BDPasajeEntities())
            {
                Bus objBus = Bd.Bus.Where(p => p.IIDBUS.Equals(IdBus)).First();
                objBus.IIDSUCURSAL = objBusCls.IdSucursal;
                objBus.IIDTIPOBUS = objBusCls.IdTipoBus;
                objBus.PLACA = objBusCls.Placa;
                objBus.FECHACOMPRA = objBusCls.FechaCompra;
                objBus.IIDMODELO = objBusCls.IdModelo;
                objBus.NUMEROCOLUMNAS = objBusCls.NumeroColumnas;
                objBus.NUMEROFILAS = objBusCls.NumeroFilas;
                objBus.DESCRIPCION = objBusCls.Descripcion;
                objBus.OBSERVACION = objBusCls.Observacion;
                objBus.IIDMARCA = objBusCls.IdMarca;

                Bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Index(BusCls objBusCls)
        {
            ListarCombos();

            List<BusCls> ListaRespuesta = new List<BusCls>();
            List<BusCls> ListaBus = null;

            using (var Bd = new BDPasajeEntities())
            {
                //Este proceso se ejecuta siempre
                ListaBus = (from Bus in Bd.Bus
                            join Sucursal in Bd.Sucursal
                            on Bus.IIDSUCURSAL equals Sucursal.IIDSUCURSAL
                            join TipoBus in Bd.TipoBus
                            on Bus.IIDTIPOBUS equals TipoBus.IIDTIPOBUS
                            join TipoModelo in Bd.Modelo
                            on Bus.IIDMODELO equals TipoModelo.IIDMODELO
                            where Bus.BHABILITADO == 1
                            select new BusCls
                            {
                                IdBus = Bus.IIDBUS,
                                Placa = Bus.PLACA,
                                NombreModelo = TipoModelo.NOMBRE,
                                NombreSucursal = Sucursal.NOMBRE,
                                NombreTipoBus = TipoBus.NOMBRE,
                                //Para los filtros debemos traer los ids 
                                IdModelo = TipoModelo.IIDMODELO,
                                IdSucursal = Sucursal.IIDSUCURSAL,
                                IdTipoBus = TipoBus.IIDTIPOBUS
                            }).ToList();

                if (objBusCls.IdBus == 0 && objBusCls.Placa == null && objBusCls.IdModelo == 0 && objBusCls.IdTipoBus == 0
                    && objBusCls.IdSucursal == 0)
                {
                    ListaRespuesta = ListaBus;
                }
                else
                {
                    //Filtro por bus
                    if (objBusCls.IdBus != 0)
                    {
                        ListaBus = ListaBus.Where(p => p.IdBus.ToString().Contains(objBusCls.IdBus.ToString())).ToList();
                    }
                    //Filtro placa
                    if (objBusCls.Placa != null)
                    {
                        ListaBus = ListaBus.Where(p => p.Placa.ToString().Contains(objBusCls.Placa)).ToList();
                    }
                    //Filtrar por modelo
                    if (objBusCls.IdModelo != 0)
                    {
                        ListaBus = ListaBus.Where(p => p.IdModelo.ToString().Contains(objBusCls.IdModelo.ToString())).ToList();
                    }
                    //Filtrar por Sucursal
                    if (objBusCls.IdSucursal != 0)
                    {
                        ListaBus = ListaBus.Where(p => p.IdSucursal.ToString().Contains(objBusCls.IdSucursal.ToString())).ToList();
                    }
                    //Filtrar por Tipo bus
                    if (objBusCls.IdTipoBus != 0)
                    {
                        ListaBus = ListaBus.Where(p => p.IdTipoBus.ToString().Contains(objBusCls.IdTipoBus.ToString())).ToList();
                    }

                    ListaRespuesta = ListaBus;
                }

            }
            return View(ListaRespuesta);
        }

        [HttpPost]
        public ActionResult Eliminar(int IdBus)
        {
            using (var Bd = new BDPasajeEntities())
            {
                Bus objBus = Bd.Bus.Where(p => p.IIDBUS.Equals(IdBus)).First();
                objBus.BHABILITADO = 0;
                Bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}