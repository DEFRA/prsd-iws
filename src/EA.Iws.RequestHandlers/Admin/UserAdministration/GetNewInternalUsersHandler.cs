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

    internal class GetNewInternalUsersHandler : IRequestHandler<GetNewInternalUsers, IList<InternalUser>>
    {
        private readonly IwsContext context;
        private readonly IMap<User, InternalUser> userMap;
        private readonly IUserContext userContext;

        public GetNewInternalUsersHandler(IwsContext context, IMap<User, InternalUser> userMap, IUserContext userContext)
        {
            this.context = context;
            this.userMap = userMap;
            this.userContext = userContext;
        }

        public async Task<IList<InternalUser>> HandleAsync(GetNewInternalUsers message)
        {
            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());

            if (!user.IsInternal || user.InternalUserStatus != InternalUserStatus.Approved)
            {
                throw new SecurityException("A user who is not an administrator or not approved may not retrieve the pending user list. Id: " + user.Id);
            }

            var users = await context.Users.Where(u => u.IsInternal 
                && u.InternalUserStatus == InternalUserStatus.Pending
                && u.Id != userContext.UserId.ToString()
                && u.EmailConfirmed).ToArrayAsync();
            
            return users.Select(userMap.Map).ToArray();
        }
    }
}
