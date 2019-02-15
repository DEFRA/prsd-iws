namespace EA.Iws.Web.Modules
{
    using Autofac;
    using Infrastructure.BulkPrenotification;
    using Infrastructure.BulkReceiptRecovery;
    using Infrastructure.BulkUpload;

    public class BulkUploadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PrenotificationValidator>().As<IPrenotificationValidator>();
            builder.RegisterType<ReceiptRecoveryValidator>().As<IReceiptRecoveryValidator>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IReceiptRecoveryFileRule>()
                .As<IReceiptRecoveryFileRule>();
            builder.RegisterType<BulkFileValidator>().As<IBulkFileValidator>();
        }
    }
}