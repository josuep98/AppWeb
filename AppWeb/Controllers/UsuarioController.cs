using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;
using System.Transactions;
using System.Security.Cryptography;
using System.Text;

namespace AppWeb.Controllers
{
    public class UsuarioController : Controller
    {

        public void ListaPersonas()
        {
            List<SelectListItem> ListaPersonas = new List<SelectListItem>();
            using (var Bd = new BDPasajeEntities())
            {
                List<SelectListItem> ListaClientes = (from item in Bd.Cliente
                                                      where item.BHABILITADO == 1
                                                      && item.bTieneUsuario != 1
                                                      select new SelectListItem
                                                      {
                                                          Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO + " (C)",
                                                          Value = item.IIDCLIENTE.ToString()
                                                      }).ToList();

                List<SelectListItem> ListaEmpleados = (from item in Bd.Empleado
                                                       where item.BHABILITADO == 1
                                                       && item.bTieneUsuario != 1
                                                       select new SelectListItem
                                                       {
                                                           Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO + " (E)",
                                                           Value = item.IIDEMPLEADO.ToString()
                                                       }).ToList();

                ListaPersonas.AddRange(ListaClientes);
                ListaPersonas.AddRange(ListaEmpleados);

                ListaPersonas = ListaPersonas.OrderBy(p => p.Text).ToList();
                ListaPersonas.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.ListaPersonas = ListaPersonas;

            }
        }

        public void ListaRol()
        {
            List<SelectListItem> ListaRol;

            using (var Bd = new BDPasajeEntities())
            {
                ListaRol = (from item in Bd.Rol
                            where item.BHABILITADO == 1
                            select new SelectListItem
                            {
                                Text = item.NOMBRE,
                                Value = item.IIDROL.ToString()
                            }).ToList();
            }
            ViewBag.ListaRol = ListaRol;
        }

        // GET: Usuario
        public ActionResult Index()
        {
            ListaPersonas();
            ListaRol();

            List<UsuarioCls> ListaUsuario = new List<UsuarioCls>();

            using (var Bd = new BDPasajeEntities())
            {
                List<UsuarioCls> ListaUsuarioCliente = (from Usuario in Bd.Usuario
                                                        join Cliente in Bd.Cliente
                                                        on Usuario.IID equals
                                                        Cliente.IIDCLIENTE
                                                        join Rol in Bd.Rol
                                                        on Usuario.IIDROL equals
                                                        Rol.IIDROL
                                                        where Usuario.bhabilitado == 1
                                                        && Usuario.TIPOUSUARIO == "C"
                                                        select new UsuarioCls
                                                        {
                                                            IdUsuario = Usuario.IIDUSUARIO,
                                                            NombrePersona = Cliente.NOMBRE + " " + Cliente.APPATERNO + " " + Cliente.APMATERNO,
                                                            NombreUsuario = Usuario.NOMBREUSUARIO,
                                                            NombreRol = Rol.NOMBRE,
                                                            NombreTipoEmpleado = "Cliente"
                                                        }).ToList();

                List<UsuarioCls> ListaUsuarioEmpleado = (from Usuario in Bd.Usuario
                                                         join Empleado in Bd.Empleado
                                                         on Usuario.IID equals
                                                         Empleado.IIDEMPLEADO
                                                         join Rol in Bd.Rol
                                                         on Usuario.IIDROL equals
                                                         Rol.IIDROL
                                                         where Usuario.bhabilitado == 1
                                                         && Usuario.TIPOUSUARIO == "E"
                                                         select new UsuarioCls
                                                         {
                                                             IdUsuario = Usuario.IIDUSUARIO,
                                                             NombrePersona = Empleado.NOMBRE + " " + Empleado.APPATERNO + " " + Empleado.APMATERNO,
                                                             NombreUsuario = Usuario.NOMBREUSUARIO,
                                                             NombreRol = Rol.NOMBRE,
                                                             NombreTipoEmpleado = "Empleado"
                                                         }).ToList();

                ListaUsuario.AddRange(ListaUsuarioCliente);
                ListaUsuario.AddRange(ListaUsuarioEmpleado);
                ListaUsuario = ListaUsuario.OrderBy(p => p.IdUsuario).ToList();


            }

            return View(ListaUsuario);
        }

        public ActionResult Filtrar(UsuarioCls objUsuarioCls)
        {
            string NombrePersona = objUsuarioCls.NombrePersona;
            ListaPersonas();
            ListaRol();

            List<UsuarioCls> ListaUsuario = new List<UsuarioCls>();

            using (var Bd = new BDPasajeEntities())
            {
                if (objUsuarioCls.NombrePersona == null)
                {
                    List<UsuarioCls> ListaUsuarioCliente = (from Usuario in Bd.Usuario
                                                            join Cliente in Bd.Cliente
                                                            on Usuario.IID equals
                                                            Cliente.IIDCLIENTE
                                                            join Rol in Bd.Rol
                                                            on Usuario.IIDROL equals
                                                            Rol.IIDROL
                                                            where Usuario.bhabilitado == 1
                                                            && Usuario.TIPOUSUARIO == "C"
                                                            select new UsuarioCls
                                                            {
                                                                IdUsuario = Usuario.IIDUSUARIO,
                                                                NombrePersona = Cliente.NOMBRE + " " + Cliente.APPATERNO + " " + Cliente.APMATERNO,
                                                                NombreUsuario = Usuario.NOMBREUSUARIO,
                                                                NombreRol = Rol.NOMBRE,
                                                                NombreTipoEmpleado = "Cliente"
                                                            }).ToList();

                    List<UsuarioCls> ListaUsuarioEmpleado = (from Usuario in Bd.Usuario
                                                             join Empleado in Bd.Empleado
                                                             on Usuario.IID equals
                                                             Empleado.IIDEMPLEADO
                                                             join Rol in Bd.Rol
                                                             on Usuario.IIDROL equals
                                                             Rol.IIDROL
                                                             where Usuario.bhabilitado == 1
                                                             && Usuario.TIPOUSUARIO == "E"
                                                             select new UsuarioCls
                                                             {
                                                                 IdUsuario = Usuario.IIDUSUARIO,
                                                                 NombrePersona = Empleado.NOMBRE + " " + Empleado.APPATERNO + " " + Empleado.APMATERNO,
                                                                 NombreUsuario = Usuario.NOMBREUSUARIO,
                                                                 NombreRol = Rol.NOMBRE,
                                                                 NombreTipoEmpleado = "Empleado"
                                                             }).ToList();

                    ListaUsuario.AddRange(ListaUsuarioCliente);
                    ListaUsuario.AddRange(ListaUsuarioEmpleado);
                    ListaUsuario = ListaUsuario.OrderBy(p => p.IdUsuario).ToList();
                }
                else
                {
                    List<UsuarioCls> ListaUsuarioCliente = (from Usuario in Bd.Usuario
                                                            join Cliente in Bd.Cliente
                                                            on Usuario.IID equals
                                                            Cliente.IIDCLIENTE
                                                            join Rol in Bd.Rol
                                                            on Usuario.IIDROL equals
                                                            Rol.IIDROL
                                                            where Usuario.bhabilitado == 1
                                                            && (
                                                            Cliente.NOMBRE.Contains(NombrePersona)
                                                            || Cliente.APPATERNO.Contains(NombrePersona)
                                                            || Cliente.APMATERNO.Contains(NombrePersona)
                                                            )
                                                            && Usuario.TIPOUSUARIO == "C"
                                                            select new UsuarioCls
                                                            {
                                                                IdUsuario = Usuario.IIDUSUARIO,
                                                                NombrePersona = Cliente.NOMBRE + " " + Cliente.APPATERNO + " " + Cliente.APMATERNO,
                                                                NombreUsuario = Usuario.NOMBREUSUARIO,
                                                                NombreRol = Rol.NOMBRE,
                                                                NombreTipoEmpleado = "Cliente"
                                                            }).ToList();

                    List<UsuarioCls> ListaUsuarioEmpleado = (from Usuario in Bd.Usuario
                                                             join Empleado in Bd.Empleado
                                                             on Usuario.IID equals
                                                             Empleado.IIDEMPLEADO
                                                             join Rol in Bd.Rol
                                                             on Usuario.IIDROL equals
                                                             Rol.IIDROL
                                                             where Usuario.bhabilitado == 1
                                                             && (
                                                             Empleado.NOMBRE.Contains(NombrePersona)
                                                             || Empleado.APPATERNO.Contains(NombrePersona)
                                                             || Empleado.APMATERNO.Contains(NombrePersona)
                                                             )
                                                             && Usuario.TIPOUSUARIO == "E"

                                                             select new UsuarioCls
                                                             {
                                                                 IdUsuario = Usuario.IIDUSUARIO,
                                                                 NombrePersona = Empleado.NOMBRE + " " + Empleado.APPATERNO + " " + Empleado.APMATERNO,
                                                                 NombreUsuario = Usuario.NOMBREUSUARIO,
                                                                 NombreRol = Rol.NOMBRE,
                                                                 NombreTipoEmpleado = "Empleado"
                                                             }).ToList();

                    ListaUsuario.AddRange(ListaUsuarioCliente);
                    ListaUsuario.AddRange(ListaUsuarioEmpleado);
                    ListaUsuario = ListaUsuario.OrderBy(p => p.IdUsuario).ToList();
                }

            }

            return PartialView("_TablaUsuario",ListaUsuario);
        }

        public int Guardar(UsuarioCls objUsuarioCls, int Titulo)
        {
            int Respuesta = 0;
            try
            {
                using (var Bd = new BDPasajeEntities())
                {
                    using (var Transaction = new TransactionScope())
                    {
                        if (Titulo == 1)
                        {
                            Usuario objUsuario = new Usuario();
                            objUsuario.NOMBREUSUARIO = objUsuarioCls.NombreUsuario;
                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(objUsuarioCls.Contra);
                            byte[] byteContraCifrada = sha.ComputeHash(byteContra);
                            string CadenaContraCifrada = BitConverter.ToString(byteContraCifrada).Replace("-", "");
                            objUsuario.CONTRA = CadenaContraCifrada;
                            //Obtiene posición de una cadena de caracteres, de izquierda a derecha posición y # de caracteres a obtener
                            objUsuario.TIPOUSUARIO = objUsuarioCls.NombrePersona.Substring(objUsuarioCls.NombrePersona.Length - 2, 1);
                            objUsuario.IID = objUsuarioCls.Id;
                            objUsuario.IIDROL = objUsuarioCls.IdRol;
                            objUsuario.bhabilitado = 1;
                            Bd.Usuario.Add(objUsuario);
                            if (objUsuario.TIPOUSUARIO == "C")
                            {
                                Cliente objCliente = Bd.Cliente.Where(p => p.IIDCLIENTE.Equals(objUsuarioCls.Id)).First();
                                objCliente.bTieneUsuario = 1;
                            }
                            else
                            {
                                Empleado objEmpleado = Bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(objUsuarioCls.Id)).First();
                                objEmpleado.bTieneUsuario = 1;
                            }

                            Respuesta = Bd.SaveChanges();
                            Transaction.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Respuesta = 0;
            }
            return Respuesta;
        }
    }
}