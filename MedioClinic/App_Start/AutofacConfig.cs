using System.Globalization;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Kentico.DI;
using Kentico.Services;
using Kentico.Services.Context;
using Kentico.Services.Menu;
using Kentico.Services.Query;

namespace MedioClinic
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            // Initializes the Autofac builder instance
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Adds a custom registration source (IRegistrationSource) that provides all services from the Kentico API
            builder.RegisterSource(new CmsRegistrationSource());

            // Services
            builder.RegisterType<MenuService>().As<IMenuService>();
            builder.RegisterType<DocumentQueryService>().As<IDocumentQueryService>();
            builder.RegisterType<SiteContext>().As<ISiteContext>()
                .WithParameter((parameter, context) => parameter.Name == "activeCulture",
                    (parameter, context) => CultureInfo.CurrentUICulture.Name);

            // Resolves the dependencies
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}