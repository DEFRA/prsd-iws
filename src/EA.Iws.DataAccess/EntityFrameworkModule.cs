namespace EA.Iws.DataAccess
{
    using Autofac;

    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IwsContext>().AsSelf().InstancePerRequest();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Repositories"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Namespace != null && t.Namespace.Contains("Security"))
                .AsImplementedInterfaces();
        }
    }
}