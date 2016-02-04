namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using DataAccess;
    using ClaimTypes = Core.Shared.ClaimTypes;

    [AutoRegister]
    public class ClaimsRepository : IClaimsRepository
    {
        private readonly IwsContext context;

        public ClaimsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Claim>> GetUserClaims(string userId)
        {
            var claims = new List<Claim>();

            var internalUser = await context.InternalUsers.SingleOrDefaultAsync(p => p.UserId == userId);

            if (internalUser != null)
            {
                claims.Add(new Claim(ClaimTypes.InternalUserStatus, internalUser.Status.ToString()));
            }

            return claims;
        }
    }
}