namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IClaimsRepository
    {
        Task<IEnumerable<Claim>> GetUserClaims(string userId);
    }
}