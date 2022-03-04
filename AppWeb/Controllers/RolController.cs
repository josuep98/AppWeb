using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class RolController : Controller
    {
        // GET: Rol
        public ActionResult Index()
        {

            List<RolCls> ListaRol = new List<RolCls>();

            using (var Bd = new BDPasajeEntities())
            {
                ListaRol = (from Rol in Bd.Rol
                            where Rol.BHABILITADO == 1
                            select new RolCls
                            {
                                IdRol = Rol.IIDROL,
                                Nombre = Rol.NOMBRE,
                                Descripcion = Rol.DESCRIPCION
                            }).ToList();
            }

            return View(ListaRol);
        }

        public ActionResult Filtro(string NombreRol)
        {
            List<RolCls> ListaRol = new List<RolCls>();

            using (var Bd = new BDPasajeEntities())
            {
                if (NombreRol == null)
                {
                    ListaRol = (from Rol in Bd.Rol
                                where Rol.BHABILITADO == 1
                                select new RolCls
                                {
                                    IdRol = Rol.IIDROL,
                                    Nombre = Rol.NOMBRE,
                                    Descripcion = Rol.DESCRIPCION
                                }).ToList();
                }
                else
                {
                    ListaRol = (from Rol in Bd.Rol
                                where Rol.BHABILITADO == 1
                                && Rol.NOMBRE.Contains(NombreRol)
                                select new RolCls
                                {
                                    IdRol = Rol.IIDROL,
                                    Nombre = Rol.NOMBRE,
                                    Descripcion = Rol.DESCRIPCION
                                }).ToList();
                }
                return PartialView("_TablaRol", ListaRol);
            }
        }

        public string Guardar(RolCls objRolCls, int Titulo)
        {
            //Si respuesta = 0 hay error
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
                        if (Titulo.Equals(-1))
                        {
                            Rol objRol = new Rol();
                            objRol.NOMBRE = objRolCls.Nombre;
                            objRol.DESCRIPCION = objRolCls.Descripcion;
                            objRol.BHABILITADO = 1;
                            Bd.Rol.Add(objRol);
                            Respuesta = Bd.SaveChanges().ToString();
                            if (Respuesta == "0") Respuesta = "";
                        }
                        else
                        {
                            Rol objRol = Bd.Rol.Where(p => p.IIDROL == Titulo).First();
                            objRol.NOMBRE = objRolCls.Nombre;
                            objRol.DESCRIPCION = objRolCls.Descripcion;
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

        //No se retornan objetos a la vista
        public JsonResult RecuperaDatos(int Titulo)
        {
            RolCls objRolCls = new RolCls();
            using (var Bd = new BDPasajeEntities())
            {
                Rol objRol = Bd.Rol.Where(p => p.IIDROL == Titulo).First();
                objRolCls.Nombre = objRol.NOMBRE;
                objRolCls.Descripcion = objRol.DESCRIPCION;
            }
            return Json(objRolCls, JsonRequestBehavior.AllowGet);
        }

        public string Eliminar(RolCls objRolCls)
        {
            string Respuesta = "";
            try
            {
                int IdRol = objRolCls.IdRol;
                using (var Bd = new BDPasajeEntities())
                {
                    Rol objRol = Bd.Rol.Where(p => p.IIDROL == IdRol).First();
                    objRol.BHABILITADO = 0;
                    Respuesta = Bd.SaveChanges().ToString();
                }
            }
            catch (Exception ex)
            {
                Respuesta = "";
            }
            return Respuesta;
        }


    }
}