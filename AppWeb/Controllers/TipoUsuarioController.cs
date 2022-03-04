using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class TipoUsuarioController : Controller
    {
        // GET: TipoUsuario
        private TipoUsuarioCls objTipoVal;

        private bool BuscarTipoUsuario(TipoUsuarioCls objTipoUsuarioCls)
        {
            bool BusquedaId = true;
            bool BusquedaNombre = true;
            bool BusquedaDescripcion = true;

            if (objTipoVal.IdTipoUsuario > 0)
                BusquedaId = objTipoUsuarioCls.IdTipoUsuario.ToString().Contains(objTipoVal.IdTipoUsuario.ToString());

            if (objTipoVal.Nombre != null)
                BusquedaNombre = objTipoUsuarioCls.Nombre.ToString().Contains(objTipoVal.Nombre);

            if (objTipoVal.Descripcion != null)
                BusquedaDescripcion = objTipoUsuarioCls.Descripcion.ToString().Contains(objTipoVal.Descripcion);

            return (BusquedaId && BusquedaNombre && BusquedaDescripcion);
        }

        public ActionResult Index(TipoUsuarioCls objTipoUsuarioCls)
        {
            objTipoVal = objTipoUsuarioCls;

            List<TipoUsuarioCls> ListaTipoUsuario = null;
            //Variable
            List<TipoUsuarioCls> ListaFiltrado = null;

            using (var Bd = new BDPasajeEntities())
            {

                ListaTipoUsuario = (from TipoUsuario in Bd.TipoUsuario
                                    where TipoUsuario.BHABILITADO == 1
                                    select new TipoUsuarioCls
                                    {
                                        IdTipoUsuario = TipoUsuario.IIDTIPOUSUARIO,
                                        Nombre = TipoUsuario.NOMBRE,
                                        Descripcion = TipoUsuario.DESCRIPCION
                                    }).ToList();

                if (objTipoUsuarioCls.IdTipoUsuario == 0 && objTipoUsuarioCls.Nombre == null
                    && objTipoUsuarioCls.Descripcion == null)
                {
                    ListaFiltrado = ListaTipoUsuario;
                }
                else
                {
                    Predicate<TipoUsuarioCls> Pred = new Predicate<TipoUsuarioCls>(BuscarTipoUsuario);
                    ListaFiltrado = ListaTipoUsuario.FindAll(Pred);
                }

            }

            return View(ListaFiltrado);
        }
    }
}