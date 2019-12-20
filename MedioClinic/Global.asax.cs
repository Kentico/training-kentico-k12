using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

using MedioClinic.Utils;

namespace MedioClinic
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Enables and configures selected Kentico ASP.NET MVC integration features
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            // Configures Web API 2
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Registers routes including system routes for enabled features
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Dependency injection
            AutofacConfig.ConfigureContainer();

            // Registers custom model binders.
            ModelBindingConfig.RegisterModelBinders();

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            PageBuilderFilters.PageTemplates.Add(new LandingPageTemplateFilter());
        }

        protected void Application_Error()
        {
            // Sets 404 HTTP exceptions to be handled via IIS (behavior is specified in the "httpErrors" section in the MedioClinic.config file)
            var error = Server.GetLastError();
            if ((error as HttpException)?.GetHttpCode() == 404)
            {
                Server.ClearError();
                Response.StatusCode = 404;
            }
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            var options = OutputCacheKeyHelper.CreateOptions();

            switch (custom)
            {
                case "DefaultSet":
                default:
                    options
                        .VaryByBrowser()
                        .VaryByHost();
                    break;
            }

            var cacheKey = OutputCacheKeyHelper.GetVaryByCustomString(context, custom, options);

            return !string.IsNullOrEmpty(cacheKey) ? cacheKey : base.GetVaryByCustomString(context, custom);
        }
    }
}
