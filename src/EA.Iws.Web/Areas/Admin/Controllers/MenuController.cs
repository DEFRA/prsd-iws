namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Iws.Requests.Notification;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using Requests.Admin.EntryOrExitPoints;
    using Requests.Admin.UserAdministration;
    using Requests.ImportNotification;
    using Requests.NotificationAssessment;
    using ViewModels.Menu;

    [Authorize(Roles = "internal,readonly")]
    public class MenuController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IMediator mediator;

        public MenuController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult HomeNavigation(AdminHomeNavigationSection section)
        {
            var model = CreateAdminLinksViewModel(section);

            return PartialView("_HomeNavigation", model);
        }

        private AdminLinksViewModel CreateAdminLinksViewModel(AdminHomeNavigationSection? section = null)
        {
            var showApproveNewInternalUserLink = Task.Run(() =>
                authorizationService.AuthorizeActivity(typeof(SetUserApprovals)))
                .Result;

            var showAddNewEntryOrExitPointLink = Task.Run(() =>
                authorizationService.AuthorizeActivity(typeof(AddEntryOrExitPoint)))
                .Result;

            var showManageExistingInternalUserLink = Task.Run(() =>
                authorizationService.AuthorizeActivity(typeof(GetExistingInternalUsers)))
                .Result;

            var showDeleteNotificationLink = Task.Run(() =>
                authorizationService.AuthorizeActivity(typeof(DeleteExportNotification)))
                .Result;

            var showManageExternalUserLink = Task.Run(() =>
                authorizationService.AuthorizeActivity(typeof(SetExternalUserStatus)))
                .Result;

            var showArchiveNotificationsLink = Task.Run(() =>
                authorizationService.AuthorizeActivity(typeof(GetArchiveNotificationsByUser)))
                .Result;

            var model = new AdminLinksViewModel
            {
                ShowApproveNewInternalUserLink = showApproveNewInternalUserLink,
                ShowAddNewEntryOrExitPointLink = showAddNewEntryOrExitPointLink,
                ShowManageExistingInternalUserLink = showManageExistingInternalUserLink,
                ShowDeleteNotificationLink = showDeleteNotificationLink,
                ShowManageExternalUserLink = showManageExternalUserLink,
                ShowArchiveNotificationsLink = showArchiveNotificationsLink,
                ShowNotificationLinks = !User.IsInRole("readonly")
            };

            if (section.HasValue)
            {
                model.ActiveSection = section.Value;
            }

            return model;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ImportNavigation(Guid id, ImportNavigationSection section)
        {
            var details = Task.Run(() => mediator.SendAsync(new GetNotificationDetails(id))).Result;

            var showAssessmentDecision = Task.Run(() =>
                authorizationService.AuthorizeActivity(
                    ImportNotificationPermissions.CanMakeImportNotificationAssessmentDecision))
                .Result;

            var showKeyDatesOverride = Task.Run(() =>
                authorizationService.AuthorizeActivity(
                    UserAdministrationPermissions.CanOverrideKeyDates))
                .Result;

            var hasComments = Task.Run(() => mediator.SendAsync(new CheckImportNotificationHasComments(id))).Result;

            var keyDates = Task.Run(() => mediator.SendAsync(new GetKeyDates(id))).Result;

            var decisions = new List<NotificationAssessmentDecision>(
                keyDates.DecisionHistory.Where(d => d.Status == NotificationStatus.Consented));
            var mostRecentConsentedDecision = decisions.OrderByDescending(d => d.Date).FirstOrDefault(d => d.Status == NotificationStatus.Consented);

            DateTime? consentExpiryDate = null;
            if (mostRecentConsentedDecision != null)
            {
                consentExpiryDate = mostRecentConsentedDecision.ConsentedTo ?? null;
            }

            var showConsentedDateInRed = ShowConsentExpiryDateInRed(details.AllFacilitiesPreconsented, consentExpiryDate);

            var model = new ImportNavigationViewModel
            {
                Details = details,
                ActiveSection = section,
                ShowImportSections = details.Status == ImportNotificationStatus.NotificationReceived,
                AdminLinksModel = CreateAdminLinksViewModel(),
                ShowAssessmentDecision = showAssessmentDecision,
                ShowKeyDatesOverride = showKeyDatesOverride,
                HasComments = hasComments,
                ShowConsentExpiryDateInRed = showConsentedDateInRed,
                ConsentExpiryDate = consentExpiryDate
            };

            return PartialView("_ImportNavigation", model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ExportNavigation(Guid id, ExportNavigationSection section)
        {
            var data = Task.Run(() => mediator.SendAsync(new GetNotificationAssessmentSummaryInformation(id))).Result;

            var showAssessmentDecision = Task.Run(() =>
                authorizationService.AuthorizeActivity(
                    ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision))
                .Result;

            var showKeyDatesOverride = Task.Run(() =>
                authorizationService.AuthorizeActivity(
                    UserAdministrationPermissions.CanOverrideKeyDates))
                .Result;

            var showFinancialGuaranteeDatesOverride = Task.Run(() =>
                authorizationService.AuthorizeActivity(
                    UserAdministrationPermissions.CanOverrideFinancialGuaranteeDates))
                .Result;

            var hasComments = Task.Run(() => mediator.SendAsync(new CheckNotificationHasComments(id))).Result;

            var keyDates = Task.Run(() => mediator.SendAsync(new GetKeyDatesSummaryInformation(id))).Result;

            var decisions = new List<NotificationAssessmentDecision>(
                keyDates.DecisionHistory.Where(d => d.Status == NotificationStatus.Consented));
            var mostRecentConsentedDecision = decisions.OrderByDescending(d => d.Date).FirstOrDefault(d => d.Status == NotificationStatus.Consented);

            DateTime? consentExpiryDate = null;
            if (mostRecentConsentedDecision != null)
            {
                consentExpiryDate = mostRecentConsentedDecision.ConsentedTo ?? null;
            }

            var showConsentedDateInRed = ShowConsentExpiryDateInRed(data.AllFacilitiesPreconsented, consentExpiryDate);

            var model = new ExportNavigationViewModel
            {
                Data = data,
                ActiveSection = section,
                AdminLinksModel = CreateAdminLinksViewModel(),
                ShowAssessmentDecision = showAssessmentDecision,
                ShowKeyDatesOverride = showKeyDatesOverride,
                ShowFinancialGuaranteeDatesOverride = showFinancialGuaranteeDatesOverride,
                HasComments = hasComments,
                ShowConsentExpiryDateInRed = showConsentedDateInRed,
                ConsentExpiryDate = consentExpiryDate
            };

            return PartialView("_ExportNavigation", model);
        }

        private bool ShowConsentExpiryDateInRed(bool? allFacilitiesPreConsented, DateTime? consentExpiryDate)
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);
            var threeYearsAgo = DateTime.UtcNow.AddYears(-3);

            if (consentExpiryDate == null)
            {
                return false;
            }   

            bool moreThanOneYearAgo = (oneYearAgo >= consentExpiryDate);
            bool moreThanThreeYearAgo = (threeYearsAgo >= consentExpiryDate);

            if (allFacilitiesPreConsented == null)
            {
                allFacilitiesPreConsented = false;
            }

            var allFacilitiesPreConsentedValue = (bool)allFacilitiesPreConsented;

            if (allFacilitiesPreConsentedValue && moreThanThreeYearAgo)
            {
                return true;
            }

            if (!allFacilitiesPreConsentedValue && moreThanOneYearAgo)
            {
                return true;
            }

            return false;
        }
    }
}