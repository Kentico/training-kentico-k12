using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Kentico.DI;
using Kentico.Services;

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

            // Abstraction services
            builder.RegisterType<MenuService>().As<IMenuService>();

            // Resolves the dependencies
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}