namespace EA.Iws.Api.Client
{
    using Autofac;
    using EA.Iws.Api.Client.HttpClients;
    using EA.Iws.Api.Client.Polly;
    using EA.Iws.Api.Client.Serlializer;
    using Serilog;

    public class ApiClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClientWrapper>()
                   .As<IHttpClientWrapper>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<HttpClientWrapperFactory>()
                   .As<IHttpClientWrapperFactory>()
                   .InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                var logger = cc.Resolve<ILogger>();
                return new RetryPolicyWrapper(PollyPolicies.GetRetryPolicy(logger));
            }).As<IRetryPolicyWrapper>().SingleInstance();

            builder.RegisterType<JsonSerializer>()
                   .As<IJsonSerializer>()
                   .SingleInstance();
        }
    }
}
