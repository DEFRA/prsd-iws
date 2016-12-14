namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using DataAccess.Identity;
    using Domain;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class GetExistingInternalUsersHandler : IRequestHandler<GetExistingInternalUsers, InternalUserData[]>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<InternalUser, IEnumerable<Claim>, InternalUserData> mapper;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;

        public GetExistingInternalUsersHandler(IwsContext context, IUserContext userContext,
            UserManager<ApplicationUser> userManager, IMapWithParameter<InternalUser, IEnumerable<Claim>, InternalUserData> mapper)
        {
            this.context = context;
            this.userContext = userContext;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<InternalUserData[]> HandleAsync(GetExistingInternalUsers message)
        {
            var user = await context.InternalUsers.SingleOrDefaultAsync(u => u.UserId == userContext.UserId.ToString());

            if (user == null || user.Status != InternalUserStatus.Approved)
            {
                throw new SecurityException(
                    "A user who is not an administrator or not approved may not retrieve the existing user list. Id: " +
                    userContext.UserId);
            }

            var users = await context.InternalUsers.Where(u =>
                    (u.Status == InternalUserStatus.Approved || u.Status == InternalUserStatus.Inactive)
                    && u.UserId != userContext.UserId.ToString()
                    && u.User.EmailConfirmed
                    && u.CompetentAuthority == user.CompetentAuthority)
                .OrderBy(u => u.User.Surname)
                .ThenBy(u => u.User.FirstName)
                .ToArrayAsync();

            var result = new List<InternalUserData>();

            foreach (var internalUser in users)
            {
                var claims = await userManager.GetClaimsAsync(internalUser.UserId);
                result.Add(mapper.Map(internalUser, claims));
            }

            return result.ToArray();
        }
    }
}