﻿namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Infrastructure.Authorization;
    using NotificationApplication.ViewModels.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationAssessment;

    [AuthorizeActivity(typeof(GetNotificationOverviewInternal))]
    public class OverviewController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IMediator mediator;

        public OverviewController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetNotificationOverviewInternal(id));
            var canChangeNumberOfShipments = Task.Run(() => authorizationService.AuthorizeActivity(typeof(GetOriginalNumberOfShipments))).Result;
            var canChangeEntryExitPoints = Task.Run(() => authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanChangeEntryExitPoint)).Result;
            var canAddProducer = Task.Run(() => authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanAddProducer)).Result;
            var canAddRemoveTransitState = Task.Run(() => authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanAddRemoveTransitState)).Result;
            var canEditContactDetails = Task.Run(() => authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanEditContactDetails)).Result;
            var canEditEwcCodes = Task.Run(() => authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanEditEwcCodes)).Result;
            var canEditYCodes = Task.Run(() => authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanEditYCodes)).Result;
            var canEditOperationCodes = Task.Run(() => authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanEditOperationCodes)).Result;
            var canEditHCodes = Task.Run(() => authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanEditHCodes)).Result;

            var model = new NotificationOverviewViewModel(result);
            model.AmountsAndDatesViewModel.CanChangeNumberOfShipments = canChangeNumberOfShipments;
            model.JourneyViewModel.CanChangeEntryExitPoint = canChangeEntryExitPoints;
            model.JourneyViewModel.CanAddRemoveTransitState = canAddRemoveTransitState && IsEditableStatus(result.SubmitSummaryData.Status);
            model.OrganisationsInvolvedViewModel.CanAddProducer = canAddProducer;
            model.OrganisationsInvolvedViewModel.CanEditContactDetails = canEditContactDetails;
            model.WasteCodeOverviewViewModel.CanEditEWCCodes = canEditEwcCodes && result.SubmitSummaryData.Status == NotificationStatus.Consented;
            model.WasteCodeOverviewViewModel.CanEditYCodes = canEditYCodes && result.SubmitSummaryData.Status == NotificationStatus.Consented;
            model.RecoveryOperationViewModel.CanEditCodes = canEditOperationCodes && result.SubmitSummaryData.Status == NotificationStatus.Consented;
            model.WasteCodeOverviewViewModel.CanEditHCodes = canEditHCodes && result.SubmitSummaryData.Status == NotificationStatus.Consented;

            return View(model);
        }

        private static bool IsEditableStatus(NotificationStatus status)
        {
            var allowedStatuses = new[]
            {
                NotificationStatus.Submitted, NotificationStatus.NotificationReceived, NotificationStatus.InAssessment,
                NotificationStatus.ReadyToTransmit, NotificationStatus.Transmitted,
                NotificationStatus.DecisionRequiredBy, NotificationStatus.Unlocked, NotificationStatus.Reassessment
            };

            return allowedStatuses.Contains(status);
        }
    }
}