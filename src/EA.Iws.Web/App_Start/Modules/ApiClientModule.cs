namespace EA.Iws.Web.Modules
{
    using Api.Client;
    using Autofac;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.OAuth;
    using Prsd.Core.Web.OpenId;
    using Services;

    public class ApiClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                var config = cc.Resolve<AppConfiguration>();
                return new IwsClient(config.ApiUrl);
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
                return new UserInfoClient(config.ApiUrl);
            }).As<IUserInfoClient>().InstancePerRequest();

            builder.RegisterType<ApiMediator>().As<IMediator>().InstancePerRequest();
        }
    }
}