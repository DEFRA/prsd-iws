namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess.Identity;
    using Microsoft.AspNet.Identity;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class SetExternalUserStatusHandler : IRequestHandler<SetExternalUserStatus, Unit>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public SetExternalUserStatusHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Unit> HandleAsync(SetExternalUserStatus message)
        {
            if (message.Status == ExternalUserStatus.Inactive)
            {
                await userManager.SetLockoutEnabledAsync(message.UserId, true);
                await userManager.SetLockoutEndDateAsync(message.UserId, SystemTime.UtcNow.AddYears(100));
            }
            else
            {
                await userManager.SetLockoutEndDateAsync(message.UserId, new DateTimeOffset(SystemTime.UtcNow));
            }

            return Unit.Value;
        }
    }
}