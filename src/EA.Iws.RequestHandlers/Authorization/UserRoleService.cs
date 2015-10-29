namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Linq;
    using System.Security.Claims;
    using DataAccess;
    using Prsd.Core.Domain;

    public class UserRoleService : IUserRoleService
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public UserRoleService(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public UserRole Get(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null
                || claimsPrincipal.Identity == null 
                || !claimsPrincipal.Identity.IsAuthenticated)
            {
                return UserRole.Unauthenticated;
            }

            return Get(userContext.UserId.ToString());
        }

        public UserRole Get(string userId)
        {
            var userIsInternal = context.InternalUsers.Any(iu => iu.UserId == userId.ToString());

            return (userIsInternal) ? UserRole.Internal : UserRole.External;
        }
    }
}
