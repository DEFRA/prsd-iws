namespace EA.Iws.Api.IdSrv
{
    using System.Web;
    using DataAccess;
    using DataAccess.Identity;
    using Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.DataProtection;
    using Owin;
    using Prsd.Core.Domain;
    using Services;
    using Thinktecture.IdentityServer.Core.Configuration;
    using Thinktecture.IdentityServer.Core.Services;

    public static class UserServiceExtensions
    {
        public static void ConfigureUserService(this IdentityServerServiceFactory factory, IAppBuilder app)
        {
            factory.UserService = new Registration<IUserService, UserService>() { Mode = RegistrationMode.InstancePerHttpRequest };
            factory.Register(new Registration<ApplicationUserManager>() { Mode = RegistrationMode.InstancePerHttpRequest });
            factory.Register(new Registration<IUserStore<ApplicationUser>, ApplicationUserStore>() { Mode = RegistrationMode.InstancePerHttpRequest });
            factory.Register(new Registration<IwsIdentityContext>() { Mode = RegistrationMode.InstancePerHttpRequest });
            factory.Register(new Registration<IDataProtectionProvider>(f => app.GetDataProtectionProvider()) { Mode = RegistrationMode.InstancePerHttpRequest });
            factory.Register(new Registration<ConfigurationService>() { Mode = RegistrationMode.InstancePerHttpRequest });
            factory.Register(new Registration<IwsContext>() { Mode = RegistrationMode.InstancePerHttpRequest });
            factory.Register(new Registration<IUserContext, UserContext>() { Mode = RegistrationMode.InstancePerHttpRequest });
            factory.Register(new Registration<IAuthenticationManager>(resolver => HttpContext.Current.GetOwinContext().Authentication) { Mode = RegistrationMode.InstancePerHttpRequest });
            factory.Register(new Registration<IEventDispatcher, NullEventDispatcher>() { Mode = RegistrationMode.Singleton });
        }
    }
}