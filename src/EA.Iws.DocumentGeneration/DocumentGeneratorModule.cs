namespace EA.Iws.DocumentGeneration
{
    using Autofac;
    using DocumentGenerator;
    using Domain;

    public class DocumentGeneratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationDocumentGenerator>().As<INotificationDocumentGenerator>();
            builder.RegisterType<MovementDocumentGenerator>().As<IMovementDocumentGenerator>();
            builder.RegisterType<FinancialGuaranteeDocumentGenerator>().As<IFinancialGuaranteeDocumentGenerator>();
        }
    }
}