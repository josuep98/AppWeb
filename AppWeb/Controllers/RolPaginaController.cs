using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class RolPaginaController : Controller
    {
        // GET: RolPagina
        public ActionResult Index()
        {
            ListarComboRol();
            ListarComboPagina();
            List<RolPaginaCls> ListaRol = new List<RolPaginaCls>();
            using (var Bd = new BDPasajeEntities())
            {
                ListaRol = (from RolPagina in Bd.RolPagina
                            join Rol in Bd.Rol
                            on RolPagina.IIDROL equals
                            Rol.IIDROL
                            join Pagina in Bd.Pagina
                            on RolPagina.IIDPAGINA equals
                            Pagina.IIDPAGINA
                            where RolPagina.BHABILITADO == 1
                            select new RolPaginaCls
                            {
                                IdRolPagina = RolPagina.IIDROLPAGINA,
                                NombreRol = Rol.NOMBRE,
                                NombreMensaje = Pagina.MENSAJE
                            }).ToList();
            }

            return View(ListaRol);
        }

        public ActionResult Filtrar(int? IdRolDdl)
        {

            List<RolPaginaCls> ListaRol = new List<RolPaginaCls>();
            using (var Bd = new BDPasajeEntities())
            {
                if (IdRolDdl == null)
                {
                    ListaRol = (from RolPagina in Bd.RolPagina
                                join Rol in Bd.Rol
                                on RolPagina.IIDROL equals
                                Rol.IIDROL
                                join Pagina in Bd.Pagina
                                on RolPagina.IIDPAGINA equals
                                Pagina.IIDPAGINA
                                where RolPagina.BHABILITADO == 1
                                select new RolPaginaCls
                                {
                                    IdRolPagina = RolPagina.IIDROLPAGINA,
                                    NombreRol = Rol.NOMBRE,
                                    NombreMensaje = Pagina.MENSAJE
                                }).ToList();
                }
                else
                {
                    ListaRol = (from RolPagina in Bd.RolPagina
                                join Rol in Bd.Rol
                                on RolPagina.IIDROL equals
                                Rol.IIDROL
                                join Pagina in Bd.Pagina
                                on RolPagina.IIDPAGINA equals
                                Pagina.IIDPAGINA
                                where RolPagina.BHABILITADO == 1
                                && RolPagina.IIDROL == IdRolDdl
                                select new RolPaginaCls
                                {
                                    IdRolPagina = RolPagina.IIDROLPAGINA,
                                    NombreRol = Rol.NOMBRE,
                                    NombreMensaje = Pagina.MENSAJE
                                }).ToList();
                }
            }
            return PartialView("_TablaRolPagina", ListaRol);
        }

        public void ListarComboRol()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from item in Bd.Rol
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDROL.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                //Pasa información del controller a mi lista
                ViewBag.ListaRol = Lista;
            }
        }

        public void ListarComboPagina()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from item in Bd.Pagina
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.MENSAJE,
                             Value = item.IIDPAGINA.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                //Pasa información del controller a mi lista
                ViewBag.ListaPagina = Lista;
            }
        }

        public string Guardar(RolPaginaCls objRolPaginaCls, int Titulo)
        {
            //Vacío es error
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
                            RolPagina objRolPagina = new RolPagina();
                            objRolPagina.IIDROL = objRolPaginaCls.IdRol;
                            objRolPagina.IIDPAGINA = objRolPaginaCls.IdPagina;
                            objRolPagina.BHABILITADO = 1;
                            Bd.RolPagina.Add(objRolPagina);
                            Respuesta = Bd.SaveChanges().ToString();
                            if (Respuesta == "0") Respuesta = "";


                        }
                        else
                        {
                            RolPagina objRolPagina = Bd.RolPagina.Where(p => p.IIDROLPAGINA == Titulo).First();
                            objRolPagina.IIDROL = objRolPaginaCls.IdRol;
                            objRolPagina.IIDPAGINA = objRolPaginaCls.IdPagina;
                            Bd.SaveChanges().ToString();
                        }
                    }

                }
            }
            catch (Exception)
            {
                Respuesta = "";
            }
            return Respuesta;
        }

        public JsonResult RecuperaInformacion(int IdRolPagina)
        {
            RolPaginaCls objRolPaginaCls = new RolPaginaCls();
            using (var Bd = new BDPasajeEntities())
            {
                RolPagina objRolPagina = Bd.RolPagina.Where(p => p.IIDROLPAGINA == IdRolPagina).First();
                objRolPaginaCls.IdRol = (int)objRolPagina.IIDROL;
                objRolPaginaCls.IdPagina = (int)objRolPagina.IIDPAGINA;
            }
            return Json(objRolPaginaCls, JsonRequestBehavior.AllowGet);
        }

        public int EliminarRolPagina(int IdRolPagina)
        {
            int Respuesta = 0;

            try
            {
                using (var Bd = new BDPasajeEntities())
                {
                    RolPagina objRolPagina = Bd.RolPagina.Where(p => p.IIDROLPAGINA == IdRolPagina).First();
                    objRolPagina.BHABILITADO = 0;
                    Respuesta = Bd.SaveChanges();
                }
            }
            catch (Exception)
            {
                Respuesta = 0;
            }

            return Respuesta;
        }

    }
}