using System.Web.Mvc;
using System.Web.Routing;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace MedioClinic
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Gets the ApplicationBuilder instance
            // Allows you to enable and configure selected Kentico MVC integration features
            ApplicationBuilder builder = ApplicationBuilder.Current;

            // Enables the preview feature
            builder.UsePreview();

            // MVC routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // DI
            AutofacConfig.ConfigureContainer();
        }
    }
}
