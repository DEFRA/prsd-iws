namespace EA.Iws.RequestHandlers.Admin.UserAdministration
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using DataAccess.Identity;
    using Domain;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;

    internal class GetInternalUserByUserIdHandler : IRequestHandler<GetInternalUserByUserId, InternalUserData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<InternalUser, IEnumerable<Claim>, InternalUserData> mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public GetInternalUserByUserIdHandler(IwsContext context,
            UserManager<ApplicationUser> userManager,
            IMapWithParameter<InternalUser, IEnumerable<Claim>, InternalUserData> mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<InternalUserData> HandleAsync(GetInternalUserByUserId message)
        {
            var user = await context.InternalUsers.SingleAsync(u => u.UserId == message.UserId);
            var claims = await userManager.GetClaimsAsync(message.UserId);

            return mapper.Map(user, claims);
        }
    }
}