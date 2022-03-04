using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class PaginaController : Controller
    {
        // GET: Pagina
        public ActionResult Index()
        {
            List<PaginaCls> ListaPagina = new List<PaginaCls>();

            using (var Bd = new BDPasajeEntities())
            {
                ListaPagina = (from Pagina in Bd.Pagina
                               where Pagina.BHABILITADO == 1
                               select new PaginaCls
                               {
                                   IdPagina = Pagina.IIDPAGINA,
                                   Mensaje = Pagina.MENSAJE,
                                   Controlador = Pagina.CONTROLADOR,
                                   Accion = Pagina.ACCION
                               }).ToList();
            }

            return View(ListaPagina);
        }

        public ActionResult Filtrar(PaginaCls objPaginaCls)
        {
            String Mensaje = objPaginaCls.MensajeFiltrar;
            List<PaginaCls> ListaPagina = new List<PaginaCls>();

            using (var Bd = new BDPasajeEntities())
            {
                if (Mensaje == null)
                {
                    ListaPagina = (from Pagina in Bd.Pagina
                                   where Pagina.BHABILITADO == 1
                                   select new PaginaCls
                                   {
                                       IdPagina = Pagina.IIDPAGINA,
                                       Mensaje = Pagina.MENSAJE,
                                       Controlador = Pagina.CONTROLADOR,
                                       Accion = Pagina.ACCION
                                   }).ToList();
                }
                else
                {
                    ListaPagina = (from Pagina in Bd.Pagina
                                   where Pagina.BHABILITADO == 1
                                   && Pagina.MENSAJE.Contains(Mensaje)
                                   select new PaginaCls
                                   {
                                       IdPagina = Pagina.IIDPAGINA,
                                       Mensaje = Pagina.MENSAJE,
                                       Controlador = Pagina.CONTROLADOR,
                                       Accion = Pagina.ACCION
                                   }).ToList();
                }
            }
            return PartialView("_TablaPagina", ListaPagina);

        }

        public int EliminarPagina(int IdPagina)
        {
            int Respuesta = 0;

            try
            {
                using (var Bd = new BDPasajeEntities())
                {
                    Pagina objPagina = Bd.Pagina.Where(p => p.IIDPAGINA == IdPagina).First();
                    objPagina.BHABILITADO = 0;
                    Respuesta = Bd.SaveChanges();
                }
            }
            catch (Exception)
            {
                Respuesta = 0;
            }

            return Respuesta;
        }

        //podemos pasar propiedades que no existen en nuestro modelo como parámetro de nuestro método
        public string Guardar(PaginaCls objPaginaCls, int Titulo)
        {
            string Respuesta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    Respuesta += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        Respuesta += "<li class='list-group-item'>" + item + "</li>";
                    }
                    Respuesta += "</ul>";
                }
                else
                {
                    using (var Bd = new BDPasajeEntities())
                    {
                        //Agregar
                        if (Titulo == -1)
                        {
                            Pagina objPagina = new Pagina();
                            objPagina.MENSAJE = objPaginaCls.Mensaje;
                            objPagina.ACCION = objPaginaCls.Accion;
                            objPagina.CONTROLADOR = objPaginaCls.Controlador;
                            objPagina.BHABILITADO = 1;
                            Bd.Pagina.Add(objPagina);
                            Respuesta = Bd.SaveChanges().ToString();
                            if (Respuesta == "0") Respuesta = "";
                        }
                        else
                        //Editar
                        {
                            Pagina objPagina = Bd.Pagina.Where(p => p.IIDPAGINA == Titulo).First();

                            objPagina.MENSAJE = objPaginaCls.Mensaje;
                            objPagina.CONTROLADOR = objPaginaCls.Controlador;
                            objPagina.ACCION = objPaginaCls.Accion;
                            Respuesta = Bd.SaveChanges().ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Respuesta = "";
            }

            return Respuesta;
        }

        public JsonResult RecuperarInformacion(int IdPagina)
        {
            PaginaCls objPaginaCls = new PaginaCls();

            using (var Bd = new BDPasajeEntities())
            {
                Pagina objPagina = Bd.Pagina.Where(p => p.IIDPAGINA == IdPagina).First();
                objPaginaCls.Mensaje = objPagina.MENSAJE;
                objPaginaCls.Accion = objPagina.ACCION;
                objPaginaCls.Controlador = objPagina.CONTROLADOR;
            }
            return Json(objPaginaCls, JsonRequestBehavior.AllowGet);
        }


    }
}