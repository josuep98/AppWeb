using AppWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWeb.Filters
{
    public class Acceder : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Si session es null, retornar a Login
            var Usuario = HttpContext.Current.Session["Usuario"];
            List<MenuCls> Roles = (List<MenuCls>)HttpContext.Current.Session["Rol"];
            string NombreController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string Accion = filterContext.ActionDescriptor.ActionName;
            int Cantidad = Roles.Where(p => p.NombreController == NombreController).Count();
            if (Usuario == null || Cantidad == 0)
                filterContext.Result = new RedirectResult("~/Login/Index");
            base.OnActionExecuting(filterContext);
        }
    }
}