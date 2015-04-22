namespace EA.Iws.IoC
{
    using Autofac;
    using DataAccess;

    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IwsContext>().AsSelf().InstancePerRequest();
        }
    }
}