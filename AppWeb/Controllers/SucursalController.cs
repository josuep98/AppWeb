using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class SucursalController : Controller
    {
        // GET: Sucursal
        public ActionResult Index(SucursalCls ObjSucursalCls)
        {
            List<SucursalCls> ListaSucursal = null;
            string NombreSucursal = ObjSucursalCls.Nombre;

            using (var Bd = new BDPasajeEntities())
            {
                if (ObjSucursalCls.Nombre == null)
                {
                    ListaSucursal = (from Sucursal in Bd.Sucursal
                                     where Sucursal.BHABILITADO == 1
                                     select new SucursalCls
                                     {
                                         IdSucursal = Sucursal.IIDSUCURSAL,
                                         Nombre = Sucursal.NOMBRE,
                                         Direccion = Sucursal.DIRECCION,
                                         Telefono = Sucursal.TELEFONO,
                                         Email = Sucursal.EMAIL
                                     }).ToList();
                }
                else
                {
                    ListaSucursal = (from Sucursal in Bd.Sucursal
                                     where Sucursal.BHABILITADO == 1
                                     && Sucursal.NOMBRE.Contains(NombreSucursal)
                                     select new SucursalCls
                                     {
                                         IdSucursal = Sucursal.IIDSUCURSAL,
                                         Nombre = Sucursal.NOMBRE,
                                         Direccion = Sucursal.DIRECCION,
                                         Telefono = Sucursal.TELEFONO,
                                         Email = Sucursal.EMAIL
                                     }).ToList();
                }
            }
            return View(ListaSucursal);
        }

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(SucursalCls objSucursalCls)
        {
            int NumRegEncontrados = 0;
            string NombreSucursal = objSucursalCls.Nombre;
            using (var Bd = new BDPasajeEntities())
            {
                NumRegEncontrados = Bd.Sucursal.Where(p => p.NOMBRE.Equals(NombreSucursal)).Count();
            }

            if (!ModelState.IsValid || NumRegEncontrados >= 1)
            {
                if (NumRegEncontrados >= 1) objSucursalCls.MensajeError = "Ya existe la sucursal ingresada";
                //Se pasa el modelo para conservar los datos en los controles
                return View(objSucursalCls);
            }

            using (var Bd = new BDPasajeEntities())
            {
                Sucursal objSucursal = new Sucursal();
                objSucursal.NOMBRE = objSucursalCls.Nombre;
                objSucursal.DIRECCION = objSucursalCls.Direccion;
                objSucursal.TELEFONO = objSucursalCls.Telefono;
                objSucursal.EMAIL = objSucursalCls.Email;
                objSucursal.FECHAAPERTURA = objSucursalCls.FechaApertura;
                objSucursal.BHABILITADO = 1;

                Bd.Sucursal.Add(objSucursal);
                Bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Editar(int Id)
        {
            SucursalCls objSucursalCls = new SucursalCls();

            using (var Bd = new BDPasajeEntities())
            {
                Sucursal objSucursal = Bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(Id)).First();
                objSucursalCls.IdSucursal = objSucursal.IIDSUCURSAL;
                objSucursalCls.Nombre = objSucursal.NOMBRE;
                objSucursalCls.Direccion = objSucursal.DIRECCION;
                objSucursalCls.Telefono = objSucursal.TELEFONO;
                objSucursalCls.Email = objSucursal.EMAIL;
                objSucursalCls.FechaApertura = (DateTime)objSucursal.FECHAAPERTURA;
            }

            return View(objSucursalCls);
        }

        [HttpPost]
        public ActionResult Editar(SucursalCls objSucursalCls)
        {
            int NumRegAfectados = 0;
            int IdSucursal = objSucursalCls.IdSucursal;
            string NombreSucursal = objSucursalCls.Nombre;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegAfectados = Bd.Sucursal.Where(p => p.NOMBRE.Equals(NombreSucursal) && !p.IIDSUCURSAL.Equals(IdSucursal)).Count();
            }

            if (!ModelState.IsValid || NumRegAfectados >= 1)
            {
                if (NumRegAfectados >= 1) objSucursalCls.MensajeError = "La sucursal ya existe";
                return View(objSucursalCls);
            }

            using (var Bd = new BDPasajeEntities())
            {
                Sucursal objSucursal = Bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(IdSucursal)).First();

                objSucursal.NOMBRE = objSucursalCls.Nombre;
                objSucursal.DIRECCION = objSucursalCls.Direccion;
                objSucursal.TELEFONO = objSucursalCls.Telefono;
                objSucursal.EMAIL = objSucursalCls.Email;
                objSucursal.FECHAAPERTURA = objSucursalCls.FechaApertura;

                Bd.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int Id)
        {
            using (var Bd = new BDPasajeEntities())
            {
                Sucursal objSucursal = Bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(Id)).First();
                objSucursal.BHABILITADO = 0;
                Bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}