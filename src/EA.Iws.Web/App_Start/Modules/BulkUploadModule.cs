namespace EA.Iws.Web.App_Start.Modules
{
    using Autofac;
    using Infrastructure.BulkUpload;
    using Infrastructure.BulkUploadReceiptRecovery;

    public class BulkUploadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Infrastructure.BulkUpload.BulkMovementValidator>().As<Infrastructure.BulkUpload.IBulkMovementValidator>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IBulkMovementPrenotificationFileRule>()
                .As<IBulkMovementPrenotificationFileRule>();

            builder.RegisterType<Infrastructure.BulkUploadReceiptRecovery.ReceiptRecoveryValidator>().As<Infrastructure.BulkUploadReceiptRecovery.IReceiptRecoveryValidator>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IReceiptRecoveryFileRule>()
                .As<IReceiptRecoveryFileRule>();
        }
    }
}