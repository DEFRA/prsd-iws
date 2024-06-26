﻿namespace EA.Iws.Web.Modules
{
    using System;
    using Api.Client;
    using Autofac;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.OAuth;
    using Prsd.Core.Web.OpenId;
    using Scanning;
    using Services;

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
        }
    }
}