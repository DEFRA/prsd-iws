namespace EA.Iws.DataAccess
{
    using Autofac;
    using Domain.FileStore;
    using Draft;
    using Draft.BulkUpload;
    using Filestore;
    using Prsd.Core.DataAccess.Mapper;
    using Prsd.Core.Mapper;

    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IwsContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ImportNotificationContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<IwsFileStoreContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<DraftContext>().AsSelf().InstancePerRequest();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Repositories"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Security"))
                .AsImplementedInterfaces();

            builder.RegisterType<DbFileRepository>().As<IFileRepository>();
            builder.RegisterType<DraftImportNotificationRepository>().As<IDraftImportNotificationRepository>();
            builder.RegisterType<DraftMovementRepository>().As<IDraftMovementRepository>();

            builder.RegisterType<EfTypeResolver>().As<ITypeResolver>();
        }
    }
}