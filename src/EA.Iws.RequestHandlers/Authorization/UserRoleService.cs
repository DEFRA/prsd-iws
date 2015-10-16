namespace EA.Iws.RequestHandlers.Authorization
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using DataAccess;
    using Microsoft.AspNet.Identity;

    public class UserRoleService : IUserRoleService
    {
        private readonly IwsContext context;

        public UserRoleService(IwsContext context)
        {
            this.context = context;
        }

        public UserRole Get(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null
                || claimsPrincipal.Identity == null 
                || !claimsPrincipal.Identity.IsAuthenticated)
            {
                return UserRole.Unauthenticated;
            }

            var userId = claimsPrincipal.Identity.GetUserId();

            return Get(userId);
        }

        public UserRole Get(string userId)
        {
            var userIsInternal = context.InternalUsers.Any(iu => iu.UserId == userId);

            return (userIsInternal) ? UserRole.Internal : UserRole.External;
        }
    }
}
