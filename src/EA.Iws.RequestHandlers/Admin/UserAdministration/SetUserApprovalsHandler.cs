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
        private readonly IMessageService messageService;

        public SetUserApprovalsHandler(IwsContext context, IUserContext userContext, IMessageService messageService)
        {
            this.context = context;
            this.userContext = userContext;
            this.messageService = messageService;
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
                    && u.InternalUserStatus == InternalUserStatus.Pending
                    && u.EmailConfirmed);

            if (users.Count() != message.UserActions.Count)
            {
                return false;
            }

            await UpdateUsers(users, message);
            
            await context.SaveChangesAsync();

            return true;
        }

        private async Task UpdateUsers(IQueryable<User> users, SetUserApprovals message)
        {
            foreach (var user in users)
            {
                var action = message.UserActions.Single(ua => ua.Key == user.Id);

                switch (action.Value)
                {
                    case ApprovalAction.Approve:
                        await user.Approve(messageService);
                        break;
                    case ApprovalAction.Reject:
                        await user.Reject(messageService);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
