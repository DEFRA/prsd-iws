namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using Infrastructure;
    using Prsd.Core.Mediator;
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
        public ActionResult HomeNavigation()
        {
            var canApproveNewInternalUser = Task.Run(() => 
                authorizationService.AuthorizeActivity(UserAdministrationPermissions.CanApproveNewInternalUser))
                .Result;

            var canAddNewEntryOrExitPoint = Task.Run(() => 
                authorizationService.AuthorizeActivity(SystemConfigurationPermissions.CanAddNewEntryOrExitPoint))
                .Result;

            var model = new AdminLinksViewModel
            {
                CanApproveNewInternalUser = canApproveNewInternalUser,
                CanAddNewEntryOrExitPoint = canAddNewEntryOrExitPoint
            };

            return PartialView("_HomeNavigation", model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ImportNavigation(Guid id, ImportNavigationSection section)
        {
            var details = Task.Run(() => mediator.SendAsync(new GetNotificationDetails(id))).Result;

            var canApproveNewInternalUser = Task.Run(() =>
                authorizationService.AuthorizeActivity(UserAdministrationPermissions.CanApproveNewInternalUser))
                .Result;

            var canAddNewEntryOrExitPoint = Task.Run(() =>
                authorizationService.AuthorizeActivity(SystemConfigurationPermissions.CanAddNewEntryOrExitPoint))
                .Result;

            var model = new ImportNavigationViewModel
            {
                Details = details,
                ActiveSection = section,
                ShowImportSections = details.Status == ImportNotificationStatus.NotificationReceived,
                AdminLinksModel = new AdminLinksViewModel
                {
                    CanApproveNewInternalUser = canApproveNewInternalUser,
                    CanAddNewEntryOrExitPoint = canAddNewEntryOrExitPoint
                }
            };

            return PartialView("_ImportNavigation", model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ExportNavigation(Guid id, ExportNavigationSection section)
        {
            var data = Task.Run(() => mediator.SendAsync(new GetNotificationAssessmentSummaryInformation(id))).Result;

            var canApproveNewInternalUser = Task.Run(() =>
                authorizationService.AuthorizeActivity(UserAdministrationPermissions.CanApproveNewInternalUser))
                .Result;

            var canAddNewEntryOrExitPoint = Task.Run(() =>
                authorizationService.AuthorizeActivity(SystemConfigurationPermissions.CanAddNewEntryOrExitPoint))
                .Result;

            var model = new ExportNavigationViewModel
            {
                Data = data,
                ActiveSection = section,
                AdminLinksModel = new AdminLinksViewModel
                {
                    CanApproveNewInternalUser = canApproveNewInternalUser,
                    CanAddNewEntryOrExitPoint = canAddNewEntryOrExitPoint
                }
            };

            return PartialView("_ExportNavigation", model);
        }
    }
}