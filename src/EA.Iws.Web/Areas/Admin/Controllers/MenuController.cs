namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.EntryOrExitPoints;
    using Requests.Admin.UserAdministration;
    using Requests.ImportNotification;
    using Requests.NotificationAssessment;
    using ViewModels.Menu;

    [Authorize(Roles = "internal")]
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

            var model = new AdminLinksViewModel
            {
                ShowApproveNewInternalUserLink = showApproveNewInternalUserLink,
                ShowAddNewEntryOrExitPointLink = showAddNewEntryOrExitPointLink,
                ShowManageExistingInternalUserLink = showManageExistingInternalUserLink,
                ShowDeleteNotificationLink = showDeleteNotificationLink
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

            var model = new ImportNavigationViewModel
            {
                Details = details,
                ActiveSection = section,
                ShowImportSections = details.Status == ImportNotificationStatus.NotificationReceived,
                AdminLinksModel = CreateAdminLinksViewModel(),
                ShowAssessmentDecision = showAssessmentDecision,
                ShowKeyDatesOverride = showKeyDatesOverride
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

            var model = new ExportNavigationViewModel
            {
                Data = data,
                ActiveSection = section,
                AdminLinksModel = CreateAdminLinksViewModel(),
                ShowAssessmentDecision = showAssessmentDecision,
                ShowKeyDatesOverride = showKeyDatesOverride,
                ShowFinancialGuaranteeDatesOverride = showFinancialGuaranteeDatesOverride
            };

            return PartialView("_ExportNavigation", model);
        }
    }
}