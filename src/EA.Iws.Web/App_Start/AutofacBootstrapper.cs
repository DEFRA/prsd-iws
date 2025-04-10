﻿namespace EA.Iws.Web
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Modules;
    using Prsd.Core.Web.Mvc;
    using MappingModule = Prsd.Core.Autofac.MappingModule;

    public class AutofacBootstrapper
    {
        public static IContainer Initialize(ContainerBuilder builder)
        {
            // Register all controllers
            builder.RegisterControllers(typeof(Startup).Assembly)
                .OnActivating(e =>
                {
                    var controller = (Controller)e.Instance;
                    controller.TempDataProvider = new CookieTempDataProvider();
                });

            // Register model binders
            builder.RegisterModelBinders(typeof(Startup).Assembly);
            builder.RegisterModelBinderProvider();

            // Register all HTTP abstractions
            builder.RegisterModule<AutofacWebTypesModule>();

            //Register mapper module
            builder.RegisterModule(new MappingModule());

            builder.RegisterModule(new Api.Client.ApiClientModule());

            // Allow property injection in views
            builder.RegisterSource(new ViewRegistrationSource());

            // Allow property injection in action filters
            builder.RegisterFilterProvider();

            // Register all Autofac specific IModule implementations
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);

            return builder.Build();
        }
    }
}