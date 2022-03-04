using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult Index(EmpleadoCls objEmpleadoCls)
        {
            List<EmpleadoCls> ListaEmpleado = null;
            ListarComboTipoUsuario();
            int IdUsuario = objEmpleadoCls.IdTipoUsuario;

            using (var Bd = new BDPasajeEntities())
            {
                if (IdUsuario == 0)
                {

                    ListaEmpleado = (from Empleado in Bd.Empleado
                                     join TipoUsuario in Bd.TipoUsuario
                                     on Empleado.IIDTIPOUSUARIO equals TipoUsuario.IIDTIPOUSUARIO
                                     join TipoContrato in Bd.TipoContrato
                                     on Empleado.IIDTIPOCONTRATO equals TipoContrato.IIDTIPOCONTRATO
                                     where Empleado.BHABILITADO == 1
                                     select new EmpleadoCls
                                     {
                                         IdEmpleado = Empleado.IIDEMPLEADO,
                                         Nombre = Empleado.NOMBRE,
                                         ApPaterno = Empleado.APPATERNO,
                                         NombreTipoUsuario = TipoUsuario.NOMBRE,
                                         NombreTipoContrato = TipoContrato.NOMBRE
                                     }).ToList();
                }
                else
                {
                    ListaEmpleado = (from Empleado in Bd.Empleado
                                     join TipoUsuario in Bd.TipoUsuario
                                     on Empleado.IIDTIPOUSUARIO equals TipoUsuario.IIDTIPOUSUARIO
                                     join TipoContrato in Bd.TipoContrato
                                     on Empleado.IIDTIPOCONTRATO equals TipoContrato.IIDTIPOCONTRATO
                                     where Empleado.BHABILITADO == 1
                                     && Empleado.IIDTIPOUSUARIO == IdUsuario
                                     select new EmpleadoCls
                                     {
                                         IdEmpleado = Empleado.IIDEMPLEADO,
                                         Nombre = Empleado.NOMBRE,
                                         ApPaterno = Empleado.APPATERNO,
                                         NombreTipoUsuario = TipoUsuario.NOMBRE,
                                         NombreTipoContrato = TipoContrato.NOMBRE
                                     }).ToList();
                }
            }
            return View(ListaEmpleado);
        }

        public void ListarComboSexo()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Sexo in Bd.Sexo
                         where Sexo.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Sexo.NOMBRE,
                             Value = Sexo.IIDSEXO.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                //Pasa información del controller a mi lista
                ViewBag.ListaSexo = Lista;
            }
        }

        public void ListarComboTipoContrato()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Item in Bd.TipoContrato
                         where Item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Item.NOMBRE,
                             Value = Item.IIDTIPOCONTRATO.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                //Pasa información del controller a mi lista
                ViewBag.ListaTipoContrato = Lista;
            }
        }
        public void ListarComboTipoUsuario()
        {
            //Agregar
            List<SelectListItem> Lista;
            using (var Bd = new BDPasajeEntities())
            {
                Lista = (from Item in Bd.TipoUsuario
                         where Item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = Item.NOMBRE,
                             Value = Item.IIDTIPOUSUARIO.ToString()
                         }).ToList();
                Lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                //Pasa información del controller a mi lista
                ViewBag.ListaTipoUsuario = Lista;
            }
        }

        public void ListarCombos()
        {
            ListarComboSexo();
            ListarComboTipoContrato();
            ListarComboTipoUsuario();
        }

        public ActionResult Agregar()
        {
            ListarCombos();
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(EmpleadoCls objEmpleadoCls)
        {
            int NumRegistros = 0;

            string Nombre = objEmpleadoCls.Nombre;
            string ApPaterno = objEmpleadoCls.ApPaterno;
            string ApMaterno = objEmpleadoCls.ApMaterno;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegistros = Bd.Empleado.Where(p => p.NOMBRE.Equals(Nombre) && p.APPATERNO.Equals(ApPaterno)
                && p.APMATERNO.Equals(ApMaterno)).Count();
            }

            if (!ModelState.IsValid || NumRegistros >= 1)
            {
                if (NumRegistros >= 1) objEmpleadoCls.MensajeError = "El empleado ya existe";
                ListarCombos();
                return View(objEmpleadoCls);
            }

            using (var Bd = new BDPasajeEntities())
            {

                Empleado objEmpleado = new Empleado();
                objEmpleado.NOMBRE = objEmpleadoCls.Nombre;
                objEmpleado.APPATERNO = objEmpleadoCls.ApPaterno;
                objEmpleado.APMATERNO = objEmpleadoCls.ApMaterno;
                objEmpleado.FECHACONTRATO = objEmpleadoCls.FechaContrato;
                objEmpleado.SUELDO = objEmpleadoCls.Sueldo;
                objEmpleado.IIDTIPOUSUARIO = objEmpleadoCls.IdTipoUsuario;
                objEmpleado.IIDTIPOCONTRATO = objEmpleadoCls.IdTipoContrato;
                objEmpleado.IIDSEXO = objEmpleadoCls.IdSexo;
                objEmpleado.BHABILITADO = 1;

                Bd.Empleado.Add(objEmpleado);
                Bd.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        public ActionResult Editar(int Id)
        {
            ListarCombos();
            EmpleadoCls objEmpleadoCls = new EmpleadoCls();

            using (var Bd = new BDPasajeEntities())
            {
                Empleado objEmpleado = Bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(Id)).First();
                objEmpleadoCls.IdEmpleado = objEmpleado.IIDEMPLEADO;
                objEmpleadoCls.Nombre = objEmpleado.NOMBRE;
                objEmpleadoCls.ApPaterno = objEmpleado.APPATERNO;
                objEmpleadoCls.ApMaterno = objEmpleado.APMATERNO;
                objEmpleadoCls.FechaContrato = (DateTime)objEmpleado.FECHACONTRATO;
                objEmpleadoCls.Sueldo = (Decimal)objEmpleado.SUELDO;
                objEmpleadoCls.IdTipoUsuario = (int)objEmpleado.IIDTIPOUSUARIO;
                objEmpleadoCls.IdTipoContrato = (int)objEmpleado.IIDTIPOCONTRATO;
                objEmpleadoCls.IdSexo = (int)objEmpleado.IIDSEXO;
            }

            return View(objEmpleadoCls);
        }

        [HttpPost]
        public ActionResult Editar(EmpleadoCls objEmpleadoCls)
        {
            int IdEmpleado = objEmpleadoCls.IdEmpleado;

            int NumRegistros = 0;

            string Nombre = objEmpleadoCls.Nombre;
            string ApPaterno = objEmpleadoCls.ApPaterno;
            string ApMaterno = objEmpleadoCls.ApMaterno;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegistros = Bd.Empleado.Where(p => p.NOMBRE.Equals(Nombre) && p.APPATERNO.Equals(ApPaterno)
                && p.APMATERNO.Equals(ApMaterno) && !p.IIDEMPLEADO.Equals(IdEmpleado) && p.BHABILITADO == 1).Count();
            }

            if (!ModelState.IsValid || NumRegistros >= 1)
            {
                if (NumRegistros >= 1) objEmpleadoCls.MensajeError = "El Empleado ya está registrado";
                ListarCombos();
                return View(objEmpleadoCls);
            }

            using (var Bd = new BDPasajeEntities())
            {
                Empleado objEmpleado = Bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(IdEmpleado)).First();
                objEmpleado.NOMBRE = objEmpleadoCls.Nombre;
                objEmpleado.APMATERNO = objEmpleadoCls.ApMaterno;
                objEmpleado.APPATERNO = objEmpleadoCls.ApPaterno;
                objEmpleado.FECHACONTRATO = objEmpleadoCls.FechaContrato;
                objEmpleado.SUELDO = objEmpleadoCls.Sueldo;
                objEmpleado.IIDTIPOUSUARIO = objEmpleadoCls.IdTipoUsuario;
                objEmpleado.IIDTIPOCONTRATO = objEmpleadoCls.IdTipoContrato;
                objEmpleado.IIDSEXO = objEmpleadoCls.IdSexo;
                Bd.SaveChanges();

            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult Eliminar(int TxtIdEmpleado)
        {
            using (var Bd = new BDPasajeEntities())
            {
                Empleado objEmpleado = Bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(TxtIdEmpleado)).First();
                objEmpleado.BHABILITADO = 0;
                Bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}