namespace EA.Iws.Api.Modules
{
    using System.Security.Claims;
    using System.Web;
    using Autofac;
    using Identity;
    using Prsd.Core.Domain;

    public class UserContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => HttpContext.Current.User as ClaimsPrincipal).As<ClaimsPrincipal>().InstancePerRequest();

            builder.RegisterType<UserContext>().As<IUserContext>().SingleInstance();
        }
    }
}