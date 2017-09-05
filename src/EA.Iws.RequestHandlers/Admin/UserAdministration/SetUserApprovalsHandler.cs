namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.Admin;
    using Core.Authorization;
    using DataAccess;
    using DataAccess.Identity;
    using Domain;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class SetUserApprovalsHandler : IRequestHandler<SetUserApprovals, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly UserManager<ApplicationUser> userManager;

        public SetUserApprovalsHandler(IwsContext context, IUserContext userContext, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userContext = userContext;
            this.userManager = userManager;
        }

        public async Task<bool> HandleAsync(SetUserApprovals message)
        {
            var requestUser = await context.InternalUsers.SingleOrDefaultAsync(u => u.UserId == userContext.UserId.ToString());

            if (requestUser == null || requestUser.Status != InternalUserStatus.Approved)
            {
                throw new SecurityException("A user who is not an administrator or not approved may not set user approvals. Id: " + userContext.UserId);
            }

            var userIds = message.UserApprovals.Select(ua => ua.UserId).ToArray();

            var users = await context.InternalUsers.Where(u => userIds.Contains(u.Id)
                    && u.Status == InternalUserStatus.Pending
                    && u.User.EmailConfirmed).ToListAsync();

            if (users.Count() != message.UserApprovals.Count)
            {
                return false;
            }

            await UpdateUsers(users, message);
            
            await context.SaveChangesAsync();

            return true;
        }

        private async Task UpdateUsers(IList<InternalUser> users, SetUserApprovals message)
        {
            foreach (var user in users)
            {
                var action = message.UserApprovals.Single(ua => ua.UserId == user.Id);

                switch (action.ApprovalAction)
                {
                    case ApprovalAction.Approve:
                        if (!action.AssignedRole.HasValue)
                        {
                            throw new InvalidOperationException(string.Format("User {0} can't be approved without assigning a role", user.UserId));
                        }

                        user.Approve();

                        // Currently all internal user registrations get given the Internal role,
                        // so only add Administrator role if that was selected.
                        if (action.AssignedRole.Value == UserRole.Administrator)
                        {
                            await
                                userManager.AddClaimAsync(user.UserId,
                                    new Claim(ClaimTypes.Role, UserRole.Administrator.ToString().ToLowerInvariant()));
                        }

                        if (action.AssignedRole.Value == UserRole.ReadOnly)
                        {
                            // Read only role needs internal role removing
                            await userManager.RemoveClaimAsync(user.UserId,
                                new Claim(ClaimTypes.Role, UserRole.Internal.ToString().ToLowerInvariant()));

                            // Add read-only claim
                            await userManager.AddClaimAsync(user.UserId,
                                new Claim(ClaimTypes.Role, UserRole.ReadOnly.ToString().ToLowerInvariant()));
                        }

                        break;
                    case ApprovalAction.Reject:
                        user.Reject();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}