namespace EA.Iws.RequestHandlers.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    public class InMemoryAuthorizationManager : IAuthorizationManager
    {
        private static readonly IDictionary<string, IList<UserRole>> authorizations = new Dictionary<string, IList<UserRole>>
        {
            { GeneralPermissions.CanAuthorizeActivity, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanReadCountryData, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanReadUserData, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanEditUserData, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanReadOrganisationData, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanEditOrganisationData, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanReadAddressBook, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanEditAddressBook, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanViewSearchResults, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { GeneralPermissions.CanReadImportExportNotificationData, new[] { UserRole.Internal, UserRole.Administrator } },
            { SystemConfigurationPermissions.CanAddNewEntryOrExitPoint, new[] { UserRole.Administrator } },
            { SystemConfigurationPermissions.CanSendTestEmail, new[] { UserRole.Administrator } },
            { ReportingPermissions.CanViewExportStatsReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ReportingPermissions.CanViewExportNotificationsReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ReportingPermissions.CanViewFinanceReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ReportingPermissions.CanViewMissingShipmentsReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ReportingPermissions.CanViewFoiReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ReportingPermissions.CanViewImportNotificationsReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ReportingPermissions.CanViewImportStatsReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ReportingPermissions.CanViewExportMovementsReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ReportingPermissions.CanViewBlanketBondsReport, new[] { UserRole.Internal, UserRole.Administrator } },
            { ExportNotificationPermissions.CanCreateExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.Administrator } },
            { ExportNotificationPermissions.CanEditExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.Administrator } },
            { ExportNotificationPermissions.CanReadExportNotification, new[] { UserRole.External, UserRole.Internal, UserRole.Administrator } },
            { ExportNotificationPermissions.CanSubmitExportNotification, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { ExportNotificationPermissions.CanReadExportNotificationInternal, new[] { UserRole.Internal, UserRole.Administrator } },
            { ExportNotificationPermissions.CanReadExportNotificationAssessment, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { ExportNotificationPermissions.CanEditExportNotificationAssessment, new[] { UserRole.Internal, UserRole.Administrator } },
            { ExportNotificationPermissions.CanChangeExportNotificationOwner, new[] { UserRole.External, UserRole.Internal, UserRole.Administrator } },
            { ExportNotificationPermissions.CanCreateLegacyExportNotification, new[] { UserRole.Internal, UserRole.Administrator } },
            { ExportNotificationPermissions.CanGetNotificationsForApplicantHome, new[] { UserRole.External } },
            { ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision, new[] { UserRole.Administrator } },
            { ExportNotificationPermissions.CanChangeNumberOfShipmentsOnExportNotification, new[] { UserRole.Administrator } },
            { ExportNotificationPermissions.CanChangeEntryExitPoint, new[] { UserRole.Administrator } },
            { ExportNotificationPermissions.CanAddProducer, new[] { UserRole.Administrator } },
            { ExportNotificationPermissions.CanAddRemoveTransitState, new[] { UserRole.Administrator } },
            { ExportMovementPermissions.CanCreateExportMovements, new[] { UserRole.External, UserRole.Internal, UserRole.Administrator } },
            { ExportMovementPermissions.CanReadExportMovements, new[] { UserRole.Internal, UserRole.External, UserRole.Administrator } },
            { ExportMovementPermissions.CanReadExportMovementsExternal, new[] { UserRole.External } },
            { ExportMovementPermissions.CanCreateExportMovementsExternal, new[] { UserRole.External } },
            { ExportMovementPermissions.CanEditExportMovements, new[] { UserRole.External, UserRole.Internal, UserRole.Administrator } },
            { ExportMovementPermissions.CanEditExportMovementsExternal, new[] { UserRole.External } },
            { ExportMovementPermissions.CanCreateExportMovementsInternal, new[] { UserRole.Internal, UserRole.Administrator } },
            { ExportMovementPermissions.CanReadExportMovementsInternal, new[] { UserRole.Internal, UserRole.Administrator } },
            { ExportMovementPermissions.CanEditExportMovementsInternal, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportNotificationPermissions.CanCreateImportNotification, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportNotificationPermissions.CanEditImportNotification, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportNotificationPermissions.CanReadImportNotification, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportNotificationPermissions.CanCompleteImportNotification, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportNotificationPermissions.CanReadImportNotificationAssessment, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportNotificationPermissions.CanEditImportNotificationAssessment, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportNotificationPermissions.CanMakeImportNotificationAssessmentDecision, new[] { UserRole.Administrator } },
            { ImportNotificationPermissions.CanChangeNumberOfShipmentsOnImportNotification, new[] { UserRole.Administrator } },
            { ImportNotificationPermissions.CanChangeImportEntryExitPoint, new[] { UserRole.Administrator } },
            { ImportMovementPermissions.CanCreateImportMovements, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportMovementPermissions.CanEditImportMovements, new[] { UserRole.Internal, UserRole.Administrator } },
            { ImportMovementPermissions.CanReadImportMovements, new[] { UserRole.Internal, UserRole.Administrator } },
            { UserAdministrationPermissions.CanCreateInternalUser, new[] { UserRole.Internal, UserRole.Administrator } },
            { UserAdministrationPermissions.CanReadInternalUserData, new[] { UserRole.Internal, UserRole.Administrator } },
            { UserAdministrationPermissions.CanApproveNewInternalUser, new[] { UserRole.Administrator } },
            { UserAdministrationPermissions.CanOverrideKeyDates, new[] { UserRole.Administrator } },
            { UserAdministrationPermissions.CanOverrideFinancialGuaranteeDates, new[] { UserRole.Administrator } },
            { UserAdministrationPermissions.CanDeleteMovements, new[] { UserRole.Administrator } },
            { UserAdministrationPermissions.CanUpdateInterimStatus, new[] { UserRole.Administrator } },
            { UserAdministrationPermissions.CanManageExistingInternalUser, new[] { UserRole.Administrator } },
            { UserAdministrationPermissions.CanDeleteTransaction, new[] { UserRole.Administrator } },
            { UserAdministrationPermissions.CanDeleteNotification, new[] { UserRole.Administrator } },
            { ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee, new[] { UserRole.Internal, UserRole.Administrator } },
            { UserAdministrationPermissions.CanOverrideShipmentData, new[] { UserRole.Administrator } },
            { UserAdministrationPermissions.CanManageExternalUsers, new[] { UserRole.Administrator } }
        };

        public Task<bool> CheckAccessAsync(AuthorizationContext context)
        {
            var userRoles = GetUserRoles(context.Principal);

            var hasAccess = false;
            IList<UserRole> roles;

            if (authorizations.TryGetValue(context.Activity, out roles))
            {
                hasAccess = roles.Intersect(userRoles).Any();
            }

            return Task.FromResult(hasAccess);
        }

        private static IEnumerable<UserRole> GetUserRoles(ClaimsPrincipal principal)
        {
            var roleClaims = principal.FindAll(ClaimTypes.Role);

            foreach (var claim in roleClaims)
            {
                UserRole role;
                if (Enum.TryParse(claim.Value, true, out role))
                {
                    yield return role;
                }
            }
        }
    }
}