using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    //Entity framework solo se utiliza en el controlador, aqui hacemos nuestra interacción con nuestro mapeo
    public class MarcaController : Controller
    {
        //GET: Marca
        //Los métodos del controller son las acciones que se utilizan en el RouteConfig
        public ActionResult Index(MarcaCls objMarcaCls)
        {
            //Para retornar la vista se debe declarar la lista fuera del using
            string NombreMarca = objMarcaCls.Nombre;
            List<MarcaCls> ListaMarca = null;
            //abre la conexion al abrir y cerrar el using y al instanciar a mi mapeo
            using (var bd = new BDPasajeEntities())
            {
                if (objMarcaCls.Nombre == null)
                {
                    //Marca recorre cada fila de nuestra tabla
                    ListaMarca = (from Obj in bd.Marca
                                  where Obj.BHABILITADO == 1
                                  select new MarcaCls
                                  {
                                      IidMarca = Obj.IIDMARCA,
                                      Nombre = Obj.NOMBRE,
                                      Descripcion = Obj.DESCRIPCION
                                  }).ToList();
                }
                else
                {
                    ListaMarca = (from Obj in bd.Marca
                                  where Obj.BHABILITADO == 1
                                  //el método contains funciona como un %like%
                                  && Obj.NOMBRE.Contains(NombreMarca)
                                  select new MarcaCls
                                  {
                                      IidMarca = Obj.IIDMARCA,
                                      Nombre = Obj.NOMBRE,
                                      Descripcion = Obj.DESCRIPCION
                                  }).ToList();
                }
            }
            return View(ListaMarca);
        }

        //Solo retorna la vista para el menú
        public ActionResult Agregar()
        {
            return View();
        }


        //Este realiza la inserción
        [HttpPost]
        public ActionResult Agregar(MarcaCls objMarcaCls)
        {
            int NumRegEncontrados = 0;
            string NombreMarca = objMarcaCls.Nombre;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegEncontrados = Bd.Marca.Where(p => p.NOMBRE.Equals(NombreMarca)).Count();
            }

            if (!ModelState.IsValid || NumRegEncontrados >= 1)
            {
                if (NumRegEncontrados >= 1) objMarcaCls.MensajeError = "El nombre de la marca ya existe";
                return View(objMarcaCls);
            }
            else
            {
                using (var Bd = new BDPasajeEntities())
                {

                    Marca objMarca = new Marca();
                    objMarca.NOMBRE = objMarcaCls.Nombre;
                    objMarca.DESCRIPCION = objMarcaCls.Descripcion;
                    objMarca.BHABILITADO = 1;
                    Bd.Marca.Add(objMarca);
                    Bd.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        //Vista para cargar la ventana con los datos extraidos de la base de datos mediante Id
        public ActionResult Editar(int Id)
        {
            MarcaCls objMarcaCls = new MarcaCls();

            using (var Bd = new BDPasajeEntities())
            {
                Marca objMarca = Bd.Marca.Where(p => p.IIDMARCA.Equals(Id)).First();
                objMarcaCls.IidMarca = objMarca.IIDMARCA;
                objMarcaCls.Nombre = objMarca.NOMBRE;
                objMarcaCls.Descripcion = objMarca.DESCRIPCION;
            }

            return View(objMarcaCls);
        }

        [HttpPost]
        public ActionResult Editar(MarcaCls objMarcaCls)
        {
            int NumRegEncontrados = 0;
            string NombreMarca = objMarcaCls.Nombre;
            int IdMarca1 = objMarcaCls.IidMarca;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegEncontrados = Bd.Marca.Where(p => p.NOMBRE.Equals(NombreMarca) && !p.IIDMARCA.Equals(IdMarca1)).Count();
            }

            if (!ModelState.IsValid || NumRegEncontrados >= 1)
            {
                if (NumRegEncontrados >= 1) objMarcaCls.MensajeError = "Esa marca ya se encunetra registrada";
                return View(objMarcaCls);
            }

            int IdMarca = objMarcaCls.IidMarca;
            using (var Bd = new BDPasajeEntities())
            {
                Marca objMarca = Bd.Marca.Where(p => p.IIDMARCA.Equals(IdMarca)).First();

                objMarca.NOMBRE = objMarcaCls.Nombre;
                objMarca.DESCRIPCION = objMarcaCls.Descripcion;
                Bd.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int Id)
        {

            using (var Bd = new BDPasajeEntities())
            {

                Marca objMarca = Bd.Marca.Where(p => p.IIDMARCA.Equals(Id)).First();
                objMarca.BHABILITADO = 0;
                Bd.SaveChanges();

            }

            return RedirectToAction("Index");
        }


    }
}