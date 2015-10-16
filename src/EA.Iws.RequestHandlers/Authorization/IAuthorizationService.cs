namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAuthorizationService
    {
        Task<bool> HasAccess(UserRole userRole, string name);

        Task<IEnumerable<string>> AuthorizedRequests(UserRole userRole);
    }
}