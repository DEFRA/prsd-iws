namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class SetUserApprovalsHandler : IRequestHandler<SetUserApprovals, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public SetUserApprovalsHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<bool> HandleAsync(SetUserApprovals message)
        {
            var requestUser = await context.InternalUsers.SingleOrDefaultAsync(u => u.UserId == userContext.UserId.ToString());

            if (requestUser == null || requestUser.Status != InternalUserStatus.Approved)
            {
                throw new SecurityException("A user who is not an administrator or not approved may not set user approvals. Id: " + userContext.UserId);
            }

            var userIds = message.UserActions.Select(ua => ua.Key).ToArray();

            var users = await context.InternalUsers.Where(u => userIds.Contains(u.Id)
                    && u.Status == InternalUserStatus.Pending
                    && u.User.EmailConfirmed).ToListAsync();

            if (users.Count() != message.UserActions.Count)
            {
                return false;
            }

            UpdateUsers(users, message);
            
            await context.SaveChangesAsync();

            return true;
        }

        private void UpdateUsers(IList<InternalUser> users, SetUserApprovals message)
        {
            foreach (var user in users)
            {
                var action = message.UserActions.Single(ua => ua.Key == user.Id);

                switch (action.Value)
                {
                    case ApprovalAction.Approve:
                        user.Approve();
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
