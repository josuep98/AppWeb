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
            if (Usuario == null)
                filterContext.Result = new RedirectResult("~/Login/Index");
            base.OnActionExecuting(filterContext);
        }
    }
}