namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Data.Entity;
    using System.Linq;
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
            var requestUser = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());

            if (!requestUser.IsInternal || requestUser.InternalUserStatus != InternalUserStatus.Approved)
            {
                return false;
            }

            var userIds = message.UserActions.Select(ua => ua.Key).ToArray();

            var users =
                context.Users.Where(u => userIds.Contains(u.Id)
                    && u.InternalUserStatus == InternalUserStatus.Pending);

            if (users.Count() != message.UserActions.Count)
            {
                return false;
            }

            UpdateUsers(users, message);
            
            await context.SaveChangesAsync();

            return true;
        }

        private void UpdateUsers(IQueryable<User> users, SetUserApprovals message)
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
