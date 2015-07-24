namespace EA.Iws.Api.Modules
{
    using Autofac;
    using EmailMessaging;
    using Services;

    public class EmailServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var componentContext = c.Resolve<IComponentContext>();
                var config = componentContext.Resolve<AppConfiguration>();
                return new SiteInformation(config.SiteRoot, config.WebSiteRoot, config.SendEmail);
            }).SingleInstance();

            builder.RegisterAssemblyModules(typeof(EmailService).Assembly);
        }
    }
}