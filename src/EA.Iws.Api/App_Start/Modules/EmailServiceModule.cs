namespace EA.Iws.Api.Modules
{
    using Autofac;
    using EmailMessaging;
    using RequestHandlers.Feedback;
    using Services;

    public class EmailServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var componentContext = c.Resolve<IComponentContext>();
                var config = componentContext.Resolve<AppConfiguration>();
                return new SiteInformation(config.SiteRoot, config.WebSiteRoot);
            }).SingleInstance();

            builder.Register(c =>
            {
                var componentContext = c.Resolve<IComponentContext>();
                var configFeedback = componentContext.Resolve<AppConfiguration>();
                return new FeedbackInformation(configFeedback.FeedbackEmailTo);
            }).SingleInstance();

            builder.RegisterModule(new EmailMessagingModule());
        }
    }
}