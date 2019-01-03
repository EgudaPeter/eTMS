using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace eTMS
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //GlobalFilters.Filters.Add(new AuthorizeAttribute());
            // Removing all the view engines
            ViewEngines.Engines.Clear();

            //Add Razor Engine (which we are using)
            ViewEngines.Engines.Add(new RazorViewEngine());
            DevExpressHelper.Theme = "MetropolisBlue";
            DevExpressHelper.Theme = "Office2010Black";
            DevExpressHelper.Theme = "iOS";
            ModelBinders.Binders.DefaultBinder = new DevExpressEditorsBinder();
        }
    }
}
