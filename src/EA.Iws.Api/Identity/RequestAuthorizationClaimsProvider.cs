namespace EA.Iws.Api.Identity
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using RequestHandlers.Authorization;
    using ClaimTypes = Core.Shared.ClaimTypes;

    public class RequestAuthorizationClaimsProvider
    {
        private readonly IUserRoleService userRoleService;
        private readonly IAuthorizationService authorizationService;

        public RequestAuthorizationClaimsProvider(IUserRoleService userRoleService, IAuthorizationService authorizationService)
        {
            this.userRoleService = userRoleService;
            this.authorizationService = authorizationService;
        }

        public async Task<IEnumerable<Claim>> GetClaims(string userId)
        {
            var role = userRoleService.Get(userId);

            return (await authorizationService.AuthorizedRequests(role))
                .Select(r => new Claim(ClaimTypes.AuthorizedRequest, r));
        }
    }
}