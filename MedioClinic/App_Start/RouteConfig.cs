using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using Kentico.Web.Mvc;
using MedioClinic.Infrastructure;


namespace MedioClinic
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var defaultCulture = CultureInfo.GetCultureInfo("en-US");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Map routes to Kentico HTTP handlers first as some Kentico URLs might be matched by the default ASP.NET MVC route resulting in displaying pages without images
            routes.Kentico().MapRoutes();

            var route = routes.MapRoute(
                name: "DoctorWithAlias",
                url: "{culture}/Doctors/Detail/{nodeId}/{nodeAlias}",
                defaults: new { action = "Detail", controller = "Doctors", culture = defaultCulture.Name, nodeId = 0, nodeAlias = "" }
            );

            // A route value determines the culture of the current thread
            route.RouteHandler = new MultiCultureMvcRouteHandler(defaultCulture);

            route = routes.MapRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { culture = defaultCulture.Name, controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
          
            // A route value determines the culture of the current thread
            route.RouteHandler = new MultiCultureMvcRouteHandler(defaultCulture);
        }
    }
}
