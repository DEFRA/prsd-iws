namespace EA.Iws.DataAccess
{
    using Autofac;

    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IwsContext>().AsSelf().InstancePerRequest();
        }
    }
}