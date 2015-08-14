namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class GetNewInternalUsersHandler : IRequestHandler<GetNewInternalUsers, IList<InternalUserData>>
    {
        private readonly IwsContext context;
        private readonly IMap<InternalUser, InternalUserData> userMap;
        private readonly IUserContext userContext;

        public GetNewInternalUsersHandler(IwsContext context, IMap<InternalUser, InternalUserData> userMap, IUserContext userContext)
        {
            this.context = context;
            this.userMap = userMap;
            this.userContext = userContext;
        }

        public async Task<IList<InternalUserData>> HandleAsync(GetNewInternalUsers message)
        {
            var user = await context.InternalUsers.SingleOrDefaultAsync(u => u.UserId == userContext.UserId.ToString());

            if (user == null || user.Status != InternalUserStatus.Approved)
            {
                throw new SecurityException("A user who is not an administrator or not approved may not retrieve the pending user list. Id: " + userContext.UserId);
            }

            var users = await context.InternalUsers.Where(u => 
                u.Status == InternalUserStatus.Pending
                && u.UserId != userContext.UserId.ToString()
                && u.User.EmailConfirmed).ToArrayAsync();
            
            return users.Select(userMap.Map).ToArray();
        }
    }
}
