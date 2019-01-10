namespace EA.Iws.DocumentGeneration
{
    using Autofac;
    using DocumentGenerator;
    using Domain;
    using Movement;
    using Movement.Blocks.Factories;
    using Notification;
    using Notification.Blocks.Factories;

    public class DocumentGeneratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationDocumentGenerator>().As<INotificationDocumentGenerator>();
            builder.RegisterType<NotificationPreviewWatermarkGenerator>().As<INotificationPreviewWatermarkGenerator>();
            builder.RegisterType<MovementDocumentGenerator>().As<IMovementDocumentGenerator>();
            builder.RegisterType<FinancialGuaranteeDocumentGenerator>().As<IFinancialGuaranteeDocumentGenerator>();
            builder.RegisterType<PostageLabelGenerator>().As<IPostageLabelGenerator>();
            builder.RegisterType<InterimMovementDocumentGenerator>().As<IInterimMovementDocumentGenerator>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<INotificationBlockFactory>()
                .As<INotificationBlockFactory>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IMovementBlockFactory>()
                .As<IMovementBlockFactory>();

            builder.RegisterType<NotificationBlocksFactory>().AsSelf();
            builder.RegisterType<MovementBlocksFactory>().AsSelf();
        }
    }
}