namespace EA.Iws.Api.IdSrv
{
    using Identity;
    using Thinktecture.IdentityServer.AspNetIdentity;

    public class UserService : AspNetIdentityUserService<ApplicationUser, string>
    {
        public UserService(ApplicationUserManager userMgr)
            : base(userMgr)
        {
        }
    }
}