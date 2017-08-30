namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Collections.Generic;
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
            var internalClaim = new Claim(ClaimTypes.Role, UserRole.Internal.ToString().ToLowerInvariant());
            var readOnlyClaim = new Claim(ClaimTypes.Role, UserRole.ReadOnly.ToString().ToLowerInvariant());

            var userClaims = await userManager.GetClaimsAsync(user.UserId);

            if (message.Role == UserRole.Administrator)
            {
                await AddClaimToUser(userClaims, adminClaim, user);
                await AddClaimToUser(userClaims, internalClaim, user);
                await RemoveClaim(userClaims, readOnlyClaim, user);
            }

            if (message.Role == UserRole.Internal)
            {
                await AddClaimToUser(userClaims, internalClaim, user);
                await RemoveClaim(userClaims, adminClaim, user);
                await RemoveClaim(userClaims, readOnlyClaim, user);
            }

            if (message.Role == UserRole.ReadOnly)
            {
                await AddClaimToUser(userClaims, readOnlyClaim, user);
                await RemoveClaim(userClaims, internalClaim, user);
                await RemoveClaim(userClaims, adminClaim, user);
            }

            return Unit.Value;
        }

        private async Task RemoveClaim(IList<Claim> userClaims, Claim claim, InternalUser user)
        {
            if (userClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                await userManager.RemoveClaimAsync(user.UserId, claim);
            }
        }

        private async Task AddClaimToUser(IList<Claim> userClaims, Claim claim, InternalUser user)
        {
            if (!userClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                await userManager.AddClaimAsync(user.UserId, claim);
            }
        }
    }
}