namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Authorization.Permissions;

    public class InMemoryAuthorizationService : IAuthorizationService
    {
        private static readonly IDictionary<string, IList<UserRole>> authorizations = new Dictionary<string, IList<UserRole>>
        {
            { SystemConfigurationPermissions.CanAddNewEntryOrExitPoint, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ReportingPermissions.CanViewExportStatsReport, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ReportingPermissions.CanViewExportNotificationsReport, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ReportingPermissions.CanViewFinanceReport, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ReportingPermissions.CanViewMissingShipmentsReport, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanCreateExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanEditExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanReadExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanCreateImportNotification, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanEditImportNotification, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanReadImportNotification, new[] { UserRole.Internal, UserRole.TeamLeader } }
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