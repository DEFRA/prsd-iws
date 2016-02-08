namespace EA.Iws.Web.App_Start.Modules
{
    using Autofac;
    using Core.Authorization;
    using Infrastructure.Authorization;

    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorizationService>().AsSelf();

            builder.RegisterType<RequestAuthorizationAttributeCache>().AsSelf().SingleInstance();
        }
    }
}