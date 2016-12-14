namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.Admin;
    using Core.Authorization;
    using DataAccess;
    using DataAccess.Identity;
    using Domain;
    using Microsoft.AspNet.Identity;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class UpdateInternalUserHandler : IRequestHandler<UpdateInternalUser, Unit>
    {
        private readonly IwsContext context;
        private readonly IInternalUserRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public UpdateInternalUserHandler(IInternalUserRepository repository, IwsContext context,
            UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> HandleAsync(UpdateInternalUser message)
        {
            var user = await repository.GetByUserId(message.UserId);

            if (message.Status == InternalUserStatus.Approved && user.Status == InternalUserStatus.Inactive)
            {
                user.Activate();
                await context.SaveChangesAsync();

                await userManager.SetLockoutEndDateAsync(user.UserId, SystemTime.UtcNow.AddDays(-1));
            }

            if (message.Status == InternalUserStatus.Inactive && user.Status == InternalUserStatus.Approved)
            {
                user.Deactivate();
                await context.SaveChangesAsync();

                await userManager.SetLockoutEnabledAsync(user.UserId, true);
                await userManager.SetLockoutEndDateAsync(user.UserId, DateTimeOffset.MaxValue);
            }

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