namespace EA.Iws.Api.Modules
{
    using Autofac;
    using Core.Domain;
    using Identity;

    public class UserContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //TODO: Remove this and replace with an actual user context implementation.
            builder.RegisterType<UserContext>().As<IUserContext>().SingleInstance();
        }
    }
}