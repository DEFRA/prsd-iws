namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.Authorization;
    using DataAccess;
    using DataAccess.Identity;
    using Domain;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class UpdateInternalUserRoleHandler : IRequestHandler<UpdateInternalUserRole, Unit>
    {
        private readonly IwsContext context;
        private readonly IInternalUserRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public UpdateInternalUserRoleHandler(IInternalUserRepository repository, IwsContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> HandleAsync(UpdateInternalUserRole message)
        {
            var user = await repository.GetByUserId(message.UserId);

            var adminClaim = new Claim(ClaimTypes.Role, UserRole.Administrator.ToString().ToLowerInvariant());
            var userClaims = await userManager.GetClaimsAsync(user.UserId);

            if (message.Role == UserRole.Administrator)
            {
                if (!userClaims.Any(c => c.Type == adminClaim.Type && c.Value == adminClaim.Value))
                {
                    await userManager.AddClaimAsync(user.UserId, adminClaim);
                }
            }
            else
            {
                if (userClaims.Any(c => c.Type == adminClaim.Type && c.Value == adminClaim.Value))
                {
                    await userManager.RemoveClaimAsync(user.UserId, adminClaim);
                }
            }

            return Unit.Value;
        }
    }
}