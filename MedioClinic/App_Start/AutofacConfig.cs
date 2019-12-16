using System;
using System.Globalization;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Business.DependencyInjection;
using Business.Repository;
using Business.Repository.LandingPage;
using Business.Services;
using Business.Services.Context;
using MedioClinic.Config;
using MedioClinic.Controllers;
using MedioClinic.Utils;

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

            // Register dependencies in Web API 2 controllers
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            // Adds a custom registration source (IRegistrationSource) that provides all services from the Kentico API
            builder.RegisterSource(new CmsRegistrationSource());

            // Registers all services that implement IService interface
            builder.RegisterAssemblyTypes(typeof(IService).Assembly)
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IService).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // Registers site context
            builder.RegisterType<SiteContextService>().As<ISiteContextService>()
                .WithParameter((parameter, context) => parameter.Name == "currentCulture",
                    (parameter, context) => CultureInfo.CurrentUICulture.Name)
                .WithParameter((parameter, context) => parameter.Name == "sitename",
                    (parameter, context) => AppConfig.Sitename)
                .InstancePerRequest();

            // Registers business dependencies
            builder.RegisterType<BusinessDependencies>().As<IBusinessDependencies>()
                .InstancePerRequest();

            // Registers all repositories that implement IRepository interface
            builder.RegisterAssemblyTypes(typeof(IRepository).Assembly)
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IRepository).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            // Registers the common file management helper
            builder.RegisterType<FileManager>().As<IFileManager>()
                .InstancePerRequest();

            // Registers the common error handler
            builder.RegisterType<ErrorHelper>().As<IErrorHelper>()
                .InstancePerRequest();

            var container = builder.Build();

            // Sets the dependency resolver for Web API 2
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Resolves the dependencies
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}