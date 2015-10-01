namespace EA.Iws.DataAccess
{
    using Autofac;
    using Domain.FileStore;
    using Filestore;

    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IwsContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<IwsFileStoreContext>().AsSelf().InstancePerRequest();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Repositories"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Security"))
                .AsImplementedInterfaces();

            builder.RegisterType<DbFileRepository>().As<IFileRepository>();
        }
    }
}