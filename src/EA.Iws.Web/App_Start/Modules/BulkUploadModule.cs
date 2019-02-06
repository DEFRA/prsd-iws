namespace EA.Iws.Web.Modules
{
    using Autofac;
    using Infrastructure.BulkPrenotification;
    using Infrastructure.BulkReceiptRecovery;

    public class BulkUploadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PrenotificationValidator>().As<IPrenotificationValidator>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IPrenotificationFileRule>()
                .As<IPrenotificationFileRule>();

            builder.RegisterType<ReceiptRecoveryValidator>().As<IReceiptRecoveryValidator>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IReceiptRecoveryFileRule>()
                .As<IReceiptRecoveryFileRule>();
        }
    }
}