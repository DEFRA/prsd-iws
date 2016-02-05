namespace EA.Iws.Web.App_Start.Modules
{
    using Autofac;
    using Infrastructure;

    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorizationService>().AsSelf();
        }
    }
}