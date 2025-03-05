namespace EA.Iws.Web.Modules
{
    using Api.Client;
    using Autofac;
    using EA.Iws.Api.Client.CompaniesHouseAPI;
    using EA.Iws.Api.Client.HttpClients;
    using EA.Iws.Api.Client.OAuthTokenProvider;
    using EA.Iws.Api.Client.Polly;
    using EA.Iws.Api.Client.Serlializer;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.OAuth;
    using Prsd.Core.Web.OpenId;
    using Serilog;
    using Services;
    using System;

    public class ApiClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                var config = cc.Resolve<AppConfiguration>();
                TimeSpan timeout = TimeSpan.FromSeconds(config.ApiTimeoutInSeconds);
                return new IwsClient(config.ApiUrl, timeout);
            }).As<IIwsClient>().InstancePerRequest();

            builder.Register(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                var config = cc.Resolve<AppConfiguration>();
                return new OAuthClient(config.ApiUrl, config.ApiClientId, config.ApiSecret);
            }).As<IOAuthClient>().SingleInstance();

            builder.Register(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                var config = cc.Resolve<AppConfiguration>();
                return new OAuthClientCredentialClient(config.ApiUrl, config.ApiClientCredentialId, config.ApiClientCredentialSecret);
            }).As<IOAuthClientCredentialClient>().SingleInstance();

            builder.Register(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                var config = cc.Resolve<AppConfiguration>();
                return new UserInfoClient(config.ApiUrl);
            }).As<IUserInfoClient>().InstancePerRequest();

            builder.RegisterType<ApiMediator>().As<IMediator>().InstancePerRequest();

            builder.Register(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                var config = cc.Resolve<AppConfiguration>();
                var httpClient = cc.Resolve<IHttpClientWrapperFactory>();
                var retryPolicy = cc.Resolve<IRetryPolicyWrapper>();
                var jsonSerializer = cc.Resolve<IJsonSerializer>();
                var logger = cc.Resolve<ILogger>();

                var httpClientHandlerConfig = new HttpClientHandlerConfig
                {
                    ProxyEnabled = config.ProxyEnabled,
                    ProxyUseDefaultCredentials = config.ProxyUseDefaultCredentials,
                    ProxyWebAddress = config.ProxyWebAddress,
                    ByPassProxyOnLocal = config.ByPassProxyOnLocal
                };

                var oauthProvider = new OAuthTokenProvider(httpClient, httpClientHandlerConfig, retryPolicy, logger,
                    config.OAuthTokenClientId, config.OAuthTokenClientSecret, config.CompaniesHouseScope,
                    config.OAuthTokenEndpoint);

                return new CompaniesHouseClient(config.CompaniesHouseBaseUrl, httpClient, retryPolicy,
                        jsonSerializer, httpClientHandlerConfig, logger, oauthProvider);
            }).As<ICompaniesHouseClient>();
        }
    }
}