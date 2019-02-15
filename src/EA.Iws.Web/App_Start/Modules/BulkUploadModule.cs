namespace EA.Iws.Web.Modules
{
    using Autofac;
    using Infrastructure.BulkPrenotification;
    using Infrastructure.BulkUpload;

    public class BulkUploadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PrenotificationValidator>().As<IPrenotificationValidator>();
            builder.RegisterType<BulkFileValidator>().As<IBulkFileValidator>();
        }
    }
}