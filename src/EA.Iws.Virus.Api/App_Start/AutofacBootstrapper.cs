namespace EA.Iws.Virus.Api.App_Start
{
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using EA.Iws.DataAccess;
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

            //builder.RegisterModule(new RequestHandlerModule());
            builder.RegisterModule(new EntityFrameworkModule());
            //builder.RegisterModule(new DocumentGeneratorModule());

            // http://www.talksharp.com/configuring-autofac-to-work-with-the-aspnet-identity-framework-in-mvc-5

            return builder.Build();
        }
    }
}