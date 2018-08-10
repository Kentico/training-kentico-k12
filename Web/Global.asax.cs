using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace Web
{
    public class MvcApplication : HttpApplication
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

            // Bundles
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            // Sets 404 HTTP exceptions to be handled via IIS (behavior is specified in the "httpErrors" section in the Web.config file)
            var error = Server.GetLastError();
            if ((error as HttpException)?.GetHttpCode() == 404)
            {
                Server.ClearError();
                Response.StatusCode = 404;
            }
        }
    }
}
