namespace EA.Iws.Api
{
    using System;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using DataAccess;
    using DataAccess.Identity;
    using DocumentGeneration;
    using Domain.NotificationAssessment;
    using EA.Iws.Api.Client;
    using EA.Iws.Api.Client.GovUkPay;
    using EA.Iws.Api.Client.HttpClients;
    using EA.Iws.Api.Client.Polly;
    using EA.Iws.Api.Client.Serlializer;
    using Identity;
    using Infrastructure.Services;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Autofac;
    using RequestHandlers;
    using Serilog;
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

            // GOV.UK Pay
            builder.RegisterType<HttpClientWrapperFactory>()
                   .As<IHttpClientWrapperFactory>()
                   .InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var logger = c.Resolve<ILogger>();
                return new RetryPolicyWrapper(PollyPolicies.GetRetryPolicy(logger));
            }).As<IRetryPolicyWrapper>().SingleInstance();

            builder.RegisterType<EA.Iws.Api.Client.Serlializer.JsonSerializer>()
                   .As<IJsonSerializer>()
                   .SingleInstance();

            builder.RegisterType<GovUkPayConfiguration>().As<Domain.NotificationAssessment.IGovUkPayConfiguration>().SingleInstance();

            builder.Register(c =>
            {
                var appConfig = c.Resolve<AppConfiguration>();
                var httpClientFactory = c.Resolve<IHttpClientWrapperFactory>();
                var retryPolicy = c.Resolve<IRetryPolicyWrapper>();
                var jsonSerializer = c.Resolve<IJsonSerializer>();
                var logger = c.Resolve<ILogger>();

                var httpClientHandlerConfig = new HttpClientHandlerConfig();

                return new PayClient(appConfig.GovUkPayBaseUrl,
                    appConfig.GovUkPayApiKey,
                    httpClientFactory,
                    retryPolicy,
                    jsonSerializer,
                    httpClientHandlerConfig,
                    logger);
            }).As<IPayClient>().InstancePerRequest();

            return builder.Build();
        }
    }
}