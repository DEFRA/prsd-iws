namespace EA.Iws.Api
{
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;

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

            return builder.Build();
        }
    }
}