namespace EA.Iws.Api
{
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using DataAccess;
    using DataAccess.Identity;
    using DocumentGeneration;
    using Identity;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Autofac;
    using RequestHandlers;
    using Services;

    public class AutofacBootstrapper
    {
        public static IContainer Initialize(ContainerBuilder builder, HttpConfiguration config, ConfigurationService configurationService)
        {
            // Config
            builder.Register(c => configurationService).As<ConfigurationService>().SingleInstance();
            builder.Register(c => configurationService.CurrentConfiguration).As<AppConfiguration>().SingleInstance();

            // Register all controllers
            builder.RegisterApiControllers(typeof(Startup).Assembly);

            // Register Autofac filter provider
            builder.RegisterWebApiFilterProvider(config);

            // Register model binders
            builder.RegisterWebApiModelBinderProvider();

            // Register all Autofac specific IModule implementations
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
            builder.RegisterAssemblyModules(typeof(AutofacMediator).Assembly);

            builder.RegisterModule(new RequestHandlerModule());
            builder.RegisterModule(new EntityFrameworkModule());
            builder.RegisterModule(new DocumentGeneratorModule());

            // http://www.talksharp.com/configuring-autofac-to-work-with-the-aspnet-identity-framework-in-mvc-5
            builder.RegisterType<IwsIdentityContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().As<UserManager<ApplicationUser>>().InstancePerRequest();

            return builder.Build();
        }
    }
}