using System.Web.Mvc;
using System.Web.Routing;

using Kentico.Web.Mvc;

namespace MedioClinic
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Map routes to Kentico HTTP handlers first as some Kentico URLs might be matched by the default ASP.NET MVC route resulting in displaying pages without images
            routes.Kentico().MapRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
