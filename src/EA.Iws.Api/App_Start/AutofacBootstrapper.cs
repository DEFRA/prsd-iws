namespace EA.Iws.Api
{
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Cqrs.Autofac;
    using Identity;
    using IoC;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security.DataProtection;
    using Owin;
    using Services;

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

            // http://www.talksharp.com/configuring-autofac-to-work-with-the-aspnet-identity-framework-in-mvc-5
            builder.RegisterType<IwsIdentityContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();

            return builder.Build();
        }
    }
}