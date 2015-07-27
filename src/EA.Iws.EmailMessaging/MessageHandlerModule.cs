namespace EA.Iws.EmailMessaging
{
    using Autofac;
    using Prsd.Core.Domain;

    public class MessageHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IEventHandler<>));

            builder.RegisterType<EmailTemplateService>().As<IEmailTemplateService>();
        }
    }
}
