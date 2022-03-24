using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
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
                }
            }

            return Mensaje;
        }

    }
}