namespace EA.Iws.EmailMessaging
{
    using Autofac;
    using Domain;

    public class MessageHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IMessageHandler<>));

            builder.RegisterType<EmailService>().As<IMessageService>();

            builder.RegisterType<EmailTemplateService>().As<IEmailTemplateService>();
        }
    }
}
