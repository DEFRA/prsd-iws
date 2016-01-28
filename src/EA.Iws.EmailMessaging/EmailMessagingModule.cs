namespace EA.Iws.EmailMessaging
{
    using System.Configuration;
    using Autofac;
    using Prsd.Core.Domain;
    using Prsd.Email;
    using Prsd.Email.Rules;
    using RazorEngine.Templating;

    public class EmailMessagingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IEventHandler<>));

            builder.Register(c => ConfigurationManager.GetSection("emailRules")).As<IRuleSet>();
            builder.Register(c => new ConfigReader("Iws"))
                .As<IEmailConfiguration>();

            builder.Register(c => new ResourceTemplateLoader(ThisAssembly, "EA.Iws.EmailMessaging.Templates"))
                .As<ITemplateLoader>();

            builder.RegisterType<RazorTemplateExecutor>().As<ITemplateExecutor>();
            builder.RegisterType<MessageCreator>().As<IMessageCreator>();
            builder.RegisterType<Sender>().As<ISender>();
            builder.RegisterType<SmtpClientProxy>().As<ISmtpClient>();
            builder.RegisterType<EmailService>().As<IEmailService>();
            builder.RegisterType<ResourceTemplateManager>().As<ITemplateManager>();

            builder.RegisterType<RazorEngineSetup>().As<IStartable>().SingleInstance();
        }
    }
}