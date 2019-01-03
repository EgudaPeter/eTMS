using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eTMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*Enable Attribute Routing
             *Attribute Routing allows you to define custom route right 
             * within your controller
             * and even your actions
             * eg: /books
               eg: /books/1430210079
             * could be define as [Route("books/{isbn?}")]
             */
            routes.MapMvcAttributeRoutes();
            //</End Attribute Routing

            AreaRegistration.RegisterAllAreas(); //http://blogs.msdn.com/b/webdev/archive/2013/10/17/attribute-routing-in-asp-net-mvc-5.aspx#route-areas
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional },
                namespaces: new[] { "eTMS.Controllers" }
            );
            //(RouteTable.Routes[routes.Count - 1] as Route).DataTokens["Area"] = "Security";
        }
    }
}
