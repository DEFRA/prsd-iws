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
            { GeneralPermissions.CanReadCountryData, new[] { UserRole.Unauthenticated, UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { GeneralPermissions.CanReadUserData, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { GeneralPermissions.CanEditUserData, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { GeneralPermissions.CanReadOrganisationData, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { GeneralPermissions.CanEditOrganisationData, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { GeneralPermissions.CanReadAddressBook, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { GeneralPermissions.CanEditAddressBook, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { GeneralPermissions.CanViewSearchResults, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { GeneralPermissions.CanSendFeedbackData, new[] { UserRole.Unauthenticated, UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { SystemConfigurationPermissions.CanAddNewEntryOrExitPoint, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { SystemConfigurationPermissions.CanSendTestEmail, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { SystemConfigurationPermissions.CanViewSmokeTest, new[] { UserRole.Unauthenticated, UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { ReportingPermissions.CanViewExportStatsReport, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ReportingPermissions.CanViewExportNotificationsReport, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ReportingPermissions.CanViewFinanceReport, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ReportingPermissions.CanViewMissingShipmentsReport, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanCreateExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanEditExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanReadExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanSubmitExportNotification, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanReadExportNotificationInternal, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanReadExportNotificationAssessment, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanEditExportNotificationAssessment, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ExportNotificationPermissions.CanChangeExportNotificationOwner, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ExportMovementPermissions.CanCreateExportMovements, new[] { UserRole.External, UserRole.Internal } },
            { ExportMovementPermissions.CanReadExportMovements, new[] { UserRole.Internal, UserRole.External, UserRole.TeamLeader } },
            { ExportMovementPermissions.CanEditExportMovements, new[] { UserRole.External, UserRole.Internal } },
            { ExportMovementPermissions.CanCreateExportMovementsInternal, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ExportMovementPermissions.CanEditExportMovementsInternal, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanCreateImportNotification, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanEditImportNotification, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanReadImportNotification, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanCompleteImportNotification, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanReadImportNotificationAssessment, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportNotificationPermissions.CanEditImportNotificationAssessment, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportMovementPermissions.CanCreateImportMovements, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportMovementPermissions.CanEditImportMovements, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { ImportMovementPermissions.CanReadImportMovements, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { UserAdministrationPermissions.CanCreateInternalUser, new[] { UserRole.External } },
            { UserAdministrationPermissions.CanReadInternalUserData, new[] { UserRole.Internal, UserRole.TeamLeader } },
            { UserAdministrationPermissions.CanApproveNewInternalUser, new[] { UserRole.Internal, UserRole.TeamLeader } }
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