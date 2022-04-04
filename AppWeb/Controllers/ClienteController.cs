using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;
using AppWeb.Filters;
using AppWeb.ClasesAuxiliares;

namespace AppWeb.Controllers
{
    public class ClienteController : Controller
    {
        [Acceder]
        // GET: Cliente
        public ActionResult Index(ClienteCls objClienteCls)
        {

            List<ClienteCls> ListaCliente = null;
            int IdSexo = objClienteCls.IdSexo;

            //Llenan nuestro DropDownList
            LlenarSexo();
            ViewBag.Lista = ListaSexo;

            using (var Bd = new BDPasajeEntities())
            {
                if (objClienteCls.IdSexo == 0)
                {
                    ListaCliente = (from Cliente in Bd.Cliente
                                    where Cliente.BHABILITADO == 1
                                    select new ClienteCls
                                    {
                                        IdCliente = Cliente.IIDCLIENTE,
                                        Nombres = Cliente.NOMBRE,
                                        ApellidoPa = Cliente.APPATERNO,
                                        ApellidoMa = Cliente.APMATERNO,
                                        Email = Cliente.EMAIL,
                                        Direccion = Cliente.DIRECCION,
                                        TelefonoFijo = Cliente.TELEFONOFIJO,
                                        Celular = Cliente.TELEFONOCELULAR
                                    }).ToList();
                }
                else
                {
                    ListaCliente = (from Cliente in Bd.Cliente
                                    where Cliente.BHABILITADO == 1
                                    && Cliente.IIDSEXO == IdSexo
                                    select new ClienteCls
                                    {
                                        IdCliente = Cliente.IIDCLIENTE,
                                        Nombres = Cliente.NOMBRE,
                                        ApellidoPa = Cliente.APPATERNO,
                                        ApellidoMa = Cliente.APMATERNO,
                                        Email = Cliente.EMAIL,
                                        Direccion = Cliente.DIRECCION,
                                        TelefonoFijo = Cliente.TELEFONOFIJO,
                                        Celular = Cliente.TELEFONOCELULAR
                                    }).ToList();
                }
            }
            return View(ListaCliente);
        }

        public ActionResult Agregar()
        {
            LlenarSexo();
            ViewBag.Lista = ListaSexo;

            return View();
        }

        [HttpPost]
        public ActionResult Agregar(ClienteCls objClienteCls)
        {
            int NumRegistros = 0;
            string Nombre = objClienteCls.Nombres;
            string ApPaterno = objClienteCls.ApellidoPa;
            string ApMaterno = objClienteCls.ApellidoMa;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegistros = Bd.Cliente.Where(p => p.NOMBRE.Equals(Nombre) && p.APPATERNO.Equals(ApPaterno) &&
                p.APMATERNO.Equals(ApMaterno)).Count();
            }

            if (!ModelState.IsValid || NumRegistros >= 1)
            {
                if (NumRegistros >= 1) objClienteCls.MensajeError = "El cliente ya está registrado";

                LlenarSexo();
                ViewBag.Lista = ListaSexo;
                return View(objClienteCls);
            }
            using (var Bd = new BDPasajeEntities())
            {
                Cliente objCliente = new Cliente();
                objCliente.NOMBRE = objClienteCls.Nombres;
                objCliente.APPATERNO = objClienteCls.ApellidoPa;
                objCliente.APMATERNO = objClienteCls.ApellidoMa;
                objCliente.EMAIL = objClienteCls.Email;
                objCliente.DIRECCION = objClienteCls.Direccion;
                objCliente.IIDSEXO = objClienteCls.IdSexo;
                objCliente.TELEFONOFIJO = objClienteCls.TelefonoFijo;
                objCliente.TELEFONOCELULAR = objClienteCls.Celular;
                objCliente.BHABILITADO = 1;


                var obj = CorreoCls.EnviarCorreo(objClienteCls.Email, "Testito", "Prueba please :v");
                if (obj == 1)
                {
                    Bd.Cliente.Add(objCliente);
                    Bd.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int Id)
        {
            //Debemos llenar nuestro modelo con la información de la base de datos
            ClienteCls objClienteCls = new ClienteCls();

            using (var Bd = new BDPasajeEntities())
            {
                //Métodos para cargar el sexo y la lista almacenados en un ViewBag
                LlenarSexo();
                ViewBag.Lista = ListaSexo;

                //Recuperamos todo el registro de la base de datos con el Id obtenido de la vista pasado como parámetro
                Cliente objCliente = Bd.Cliente.Where(p => p.IIDCLIENTE.Equals(Id)).First();

                objClienteCls.IdCliente = objCliente.IIDCLIENTE;
                objClienteCls.Nombres = objCliente.NOMBRE;
                objClienteCls.ApellidoPa = objCliente.APPATERNO;
                objClienteCls.ApellidoMa = objCliente.APMATERNO;
                objClienteCls.Direccion = objCliente.DIRECCION;
                objClienteCls.Email = objCliente.EMAIL;
                objClienteCls.IdSexo = (int)objCliente.IIDSEXO;
                objClienteCls.Celular = objCliente.TELEFONOCELULAR;
                objClienteCls.TelefonoFijo = objCliente.TELEFONOFIJO;
            }

            return View(objClienteCls);
        }

        [HttpPost]
        public ActionResult Editar(ClienteCls objClienteCls)
        {
            int NumRegistros = 0;
            int IdCliente = objClienteCls.IdCliente;
            string Nombre = objClienteCls.Nombres;
            string ApPaterno = objClienteCls.ApellidoPa;
            string ApMaterno = objClienteCls.ApellidoMa;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegistros = Bd.Cliente.Where(p => p.NOMBRE.Equals(Nombre) && p.APPATERNO.Equals(ApPaterno) &&
                p.APMATERNO.Equals(ApMaterno) && !p.IIDCLIENTE.Equals(IdCliente)).Count();
            }

            if (!ModelState.IsValid || NumRegistros >= 1)
            {
                if (NumRegistros >= 1) objClienteCls.MensajeError = "El cliente ya existe";
                LlenarSexo();
                ViewBag.Lista = ListaSexo;
                return View(objClienteCls);
            }

            using (var Bd = new BDPasajeEntities())
            {

                Cliente objCliente = Bd.Cliente.Where(p => p.IIDCLIENTE.Equals(IdCliente)).First();
                objCliente.NOMBRE = objClienteCls.Nombres;
                objCliente.APPATERNO = objClienteCls.ApellidoPa;
                objCliente.APMATERNO = objClienteCls.ApellidoMa;
                objCliente.EMAIL = objClienteCls.Email;
                objCliente.DIRECCION = objClienteCls.Direccion;
                objCliente.IIDSEXO = objClienteCls.IdSexo;
                objCliente.TELEFONOFIJO = objClienteCls.TelefonoFijo;
                objCliente.TELEFONOCELULAR = objClienteCls.Celular;
                Bd.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        List<SelectListItem> ListaSexo;
        private void LlenarSexo()
        {
            using (var Bd = new BDPasajeEntities())
            {
                ListaSexo = (from sexo in Bd.Sexo
                             where sexo.BHABILITADO == 1
                             select new SelectListItem
                             {
                                 Text = sexo.NOMBRE,
                                 Value = sexo.IIDSEXO.ToString()
                             }).ToList();
                ListaSexo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
        }

        //Cuando pasamos un parámetro desde el formulario con método post debe tener el mismo nombre del textbox para eliminar
        [HttpPost]
        public ActionResult Eliminar(int IdCliente)
        {
            using (var Bd = new BDPasajeEntities())
            {
                Cliente objCliente = Bd.Cliente.Where(p => p.IIDCLIENTE.Equals(IdCliente)).First();
                objCliente.BHABILITADO = 0;
                Bd.SaveChanges();

                return RedirectToAction("Index");
            }

        }

    }
}