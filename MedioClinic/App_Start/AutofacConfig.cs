using System.Globalization;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Kentico.DI;
using Kentico.Repository;
using Kentico.Services.Context;
using Kentico.Services.Query;

namespace MedioClinic
{
    public class AutofacConfig
    {
        private const string Sitename = "MedioClinic";

        public static void ConfigureContainer()
        {
            // Initializes the Autofac builder instance
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Adds a custom registration source (IRegistrationSource) that provides all services from the Kentico API
            builder.RegisterSource(new CmsRegistrationSource());

            // Services
            builder.RegisterType<DocumentQueryService>().As<IDocumentQueryService>();
            builder.RegisterType<SiteContextService>().As<ISiteContextService>()
                .WithParameter((parameter, context) => parameter.Name == "currentCulture",
                    (parameter, context) => CultureInfo.CurrentUICulture.Name)
                .WithParameter((parameter, context) => parameter.Name == "sitename",
                    (parameter, context) => Sitename)
                .InstancePerRequest();

            // Business
            builder.RegisterType<BusinessDependencies>().As<IBusinessDependencies>()
                .InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(IRepository).Assembly)
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IRepository).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // Resolves the dependencies
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}