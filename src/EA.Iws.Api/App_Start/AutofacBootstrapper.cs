namespace EA.Iws.Api
{
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Cqrs.Autofac;
    using IoC;

    public class AutofacBootstrapper
    {
        public static IContainer Initialize(ContainerBuilder builder, HttpConfiguration config)
        {
            // Register all controllers
            builder.RegisterApiControllers(typeof(Startup).Assembly);

            // Register Autofac filter provider
            builder.RegisterWebApiFilterProvider(config);

            // Register model binders
            builder.RegisterWebApiModelBinders(typeof(Startup).Assembly);
            builder.RegisterWebApiModelBinderProvider();

            // Register all Autofac specific IModule implementations
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
            builder.RegisterAssemblyModules(typeof(AutofacCommandBus).Assembly);
            builder.RegisterAssemblyModules(typeof(AuthorizationModule).Assembly);

            return builder.Build();
        }
    }
}