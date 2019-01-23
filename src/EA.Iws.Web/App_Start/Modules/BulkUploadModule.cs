namespace EA.Iws.Web.App_Start.Modules
{
    using Autofac;
    using Infrastructure.BulkUpload;

    public class BulkUploadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BulkMovementValidator>().As<IBulkMovementValidator>();
        }
    }
}