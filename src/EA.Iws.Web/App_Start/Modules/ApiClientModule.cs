namespace EA.Iws.Web.Modules
{
    using Api.Client;
    using Autofac;
    using Prsd.Core.Mediator;
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
            }).As<IIwsClient>();

            builder.Register(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                var config = cc.Resolve<AppConfiguration>();
                return new IwsOAuthClient(config.ApiUrl, config.ApiSecret);
            }).As<IIwsOAuthClient>();
        }
    }
}