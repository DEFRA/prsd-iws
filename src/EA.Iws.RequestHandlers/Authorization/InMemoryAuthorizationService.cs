namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InMemoryAuthorizationService : IAuthorizationService
    {
        private readonly IDictionary<string, IList<UserRole>> authorizations = new Dictionary<string, IList<UserRole>>
        {
            { "Get Exporter For Export Notification", new[] { UserRole.External, UserRole.TeamLeader } },
            { "Set Exporter For Export Notification", new[] { UserRole.External } },
            { "Create Import Notification", new[] { UserRole.Internal, UserRole.TeamLeader } },
            { "Create Export Notification", new[] { UserRole.External } }
        }; 

        public Task<bool> HasAccess(UserRole userRole, string name)
        {
            var hasAccess = false;

            IList<UserRole> roles;
            if (authorizations.TryGetValue(name, out roles))
            {
                hasAccess = roles.Contains(userRole);
            }

            return Task.FromResult(hasAccess);
        }

        public Task<IEnumerable<string>> AuthorizedRequests(UserRole userRole)
        {
            return Task.FromResult(authorizations.Where(a => a.Value.Contains(userRole)).Select(a => a.Key));
        }
    }
}
