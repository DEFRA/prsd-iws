namespace EA.Iws.IoC
{
    using Autofac;
    using Core.Cqrs;

    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResourceAuthorizationManager>().As<IResourceAuthorizationManager>();
        }
    }
}