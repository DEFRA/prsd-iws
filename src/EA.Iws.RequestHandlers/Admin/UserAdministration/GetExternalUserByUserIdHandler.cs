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

    internal class GetExternalUserByUserIdHandler : IRequestHandler<GetExternalUserByUserId, ExternalUserData>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public GetExternalUserByUserIdHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ExternalUserData> HandleAsync(GetExternalUserByUserId message)
        {
            var user = await userManager.FindByIdAsync(message.UserId.ToString());

            return new ExternalUserData
            {
                Status = GetStatus(user.LockoutEndDateUtc),
                Email = user.Email,
                UserId = message.UserId
            };
        }

        private static ExternalUserStatus GetStatus(DateTime? lockoutEndDateUtc)
        {
            if (lockoutEndDateUtc == null)
            {
                return ExternalUserStatus.Active;
            }

            if (lockoutEndDateUtc.Value <= SystemTime.UtcNow)
            {
                return ExternalUserStatus.Active;
            }

            return ExternalUserStatus.Inactive;
        }
    }
}