namespace EA.Iws.Web.App_Start.Modules
{
    using Autofac;
    using Infrastructure.BulkPrenotification;

    public class BulkUploadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BulkMovementValidator>().As<IBulkMovementValidator>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AssignableTo<IBulkMovementPrenotificationFileRule>()
                .As<IBulkMovementPrenotificationFileRule>();
        }
    }
}