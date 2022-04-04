using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AppWeb.ClasesAuxiliares;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            Session["Rol"] = null;
            return RedirectToAction("Index");
        }

        public string Login(UsuarioCls usuarioCls)
        {
            string Mensaje = "";
            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                Mensaje += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    Mensaje += "<li class='list-group-item'>" + item + "</li>";
                }
                Mensaje += "</ul>";
            }
            else
            {
                string NombreUsuario = usuarioCls.NombreUsuario;
                string Pass = usuarioCls.Contra;
                //Cifrar contraseña
                SHA256Managed sha = new SHA256Managed();
                //Aqui ciframos la cadena recibida de la vista
                byte[] byteContra = Encoding.Default.GetBytes(Pass);
                byte[] byteContraCifrada = sha.ComputeHash(byteContra);
                string CadenaContraCifrada = BitConverter.ToString(byteContraCifrada).Replace("-", "");
                using (var Bd = new BDPasajeEntities())
                {
                    int NumeroVeces = Bd.Usuario.Where(p => p.NOMBREUSUARIO == NombreUsuario && p.CONTRA == CadenaContraCifrada).Count();
                    Mensaje = NumeroVeces.ToString();
                    if (Mensaje == "0")
                        Mensaje = "Usuario o contraseña incorrecta";
                    else
                    {
                        //Objeto usuario
                        Usuario objUsuario = Bd.Usuario.Where(p => p.NOMBREUSUARIO == NombreUsuario && p.CONTRA == CadenaContraCifrada).First();
                        Session["Usuario"] = objUsuario;
                        if (objUsuario.TIPOUSUARIO == "C")
                        {
                            Cliente objCliente = Bd.Cliente.Where(p => p.IIDCLIENTE == objUsuario.IID).First();
                            //Utilizar un modelo en lugar de session
                            Session["NombreCompleto"] = objCliente.NOMBRE + " " + objCliente.APPATERNO + " " + objCliente.APMATERNO;
                        }
                        else
                        {
                            Empleado objEmpleado = Bd.Empleado.Where(p => p.IIDEMPLEADO == objUsuario.IID).First();
                            Session["NombreCompleto"] = objEmpleado.NOMBRE + " " + objEmpleado.APPATERNO + " " + objEmpleado.APMATERNO;
                        }
                        List<MenuCls> ListaMenu = (from Usuario in Bd.Usuario
                                                   join Rol in Bd.Rol
                                                   on Usuario.IIDROL equals Rol.IIDROL
                                                   join RolPagina in Bd.RolPagina
                                                   on Rol.IIDROL equals RolPagina.IIDROL
                                                   join Pagina in Bd.Pagina
                                                   on RolPagina.IIDPAGINA equals Pagina.IIDPAGINA
                                                   where Rol.IIDROL == objUsuario.IIDROL && RolPagina.IIDROL == objUsuario.IIDROL &&
                                                   Usuario.IIDUSUARIO == objUsuario.IIDUSUARIO
                                                   select new MenuCls
                                                   {
                                                       NombreAccion = Pagina.ACCION,
                                                       NombreController = Pagina.CONTROLADOR,
                                                       Mensaje = Pagina.MENSAJE
                                                   }).ToList();
                        Session["Rol"] = ListaMenu;
                    }
                }
            }
            return Mensaje;
        }
        public string RecuperarContra(string IdTipo, string Correo, string TelCelular)
        {
            string Mensaje = "";
            using (var Bd = new BDPasajeEntities())
            {
                int Cantidad = 0;
                int IdCliente = 0;
                if (IdTipo == "C")
                {
                    //Si existe un cliente con está información
                    Cantidad = Bd.Cliente.Where(p => p.EMAIL == Correo && p.TELEFONOCELULAR == TelCelular).Count();
                }
                if (Cantidad == 0)
                    Mensaje = "No existe una persona registrada con esa información!";
                else
                {
                    IdCliente = Bd.Cliente.Where(p => p.EMAIL == Correo && p.TELEFONOCELULAR == TelCelular).First().IIDCLIENTE;
                    //Verificar si existe el usuario
                    int Nveces = Bd.Usuario.Where(p => p.IID == IdCliente && p.TIPOUSUARIO == "C").Count();
                    if (Nveces == 0)
                        Mensaje = "No tiene usuario";
                    else
                    {
                        Usuario objUsuario = Bd.Usuario.Where(p => p.IID == IdCliente && p.TIPOUSUARIO == "C").First();
                        Random Ra = new Random();
                        int N1 = Ra.Next(0, 9);
                        int N2 = Ra.Next(0, 9);
                        int N3 = Ra.Next(0, 9);
                        int N4 = Ra.Next(0, 9);
                        string NuevaContra = (N1 + N2 + N3 + N4).ToString();
                        //Cifrando nueva clave
                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(NuevaContra);
                        byte[] byteContraCifrada = sha.ComputeHash(byteContra);
                        string CadenaContraCifrada = BitConverter.ToString(byteContraCifrada).Replace("-", "");
                        objUsuario.CONTRA = CadenaContraCifrada;
                        Mensaje = Bd.SaveChanges().ToString();
                        CorreoCls.EnviarCorreo(Correo, "Recuperar Clave", "Se reseteo su clave, ahora su clave es: " + NuevaContra);
                    }
                }
            }
            return Mensaje;
        }

    }
}