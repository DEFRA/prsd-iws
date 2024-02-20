namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.Shared;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;
    using Requests.NotificationAssessment;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels;

    [AuthorizeActivity(typeof(GetKeyDatesSummaryInformation))]
    public class KeyDatesController : Controller
    {
        private readonly IMediator mediator;
        private readonly AuthorizationService authorizationService;
        private readonly IAdditionalChargeService additionalChargeService;

        public KeyDatesController(IMediator mediator, AuthorizationService authorizationService, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, KeyDatesStatusEnum? command)
        {
            var data = await mediator.SendAsync(new GetKeyDatesSummaryInformation(id));

            var model = new DateInputViewModel(data.Dates)
            {
                IsAreaAssigned = data.IsLocalAreaSet,
                CompetentAuthority = data.CompetentAuthority,
                AssessmentDecisions = data.DecisionHistory,
                IsInterim = data.IsInterim,
                AdditionalCharge = new AdditionalChargeData()
                {
                    NotificationId = id
                },
                CurrentStatus = data.Dates.CurrentStatus,
                ShowAdditionalCharge = (data.CompetentAuthority == UKCompetentAuthority.England || data.CompetentAuthority == UKCompetentAuthority.Scotland) ? true : false
            };

            if (command != null)
            {
                model.Command = command.GetValueOrDefault();
                AddRelevantDateToNewDate(model);
            }

            model.ShowAssessmentDecisionLink = await authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision);

            if (data.Dates.CurrentStatus == Core.NotificationAssessment.NotificationStatus.Reassessment)
            {
                return RedirectToAction("AcceptChanges", id);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(DateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ShowAssessmentDecisionLink = await authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision);

                return View(model);
            }

            if (model.Command == KeyDatesStatusEnum.NotificationReceived)
            {
                await SetNotificationReceived(model);
            }
            else if (model.Command == KeyDatesStatusEnum.AssessmentCommenced)
            {
                await SetAssessmentCommenced(model);
            }
            else if (model.Command == KeyDatesStatusEnum.NotificationComplete)
            {
                await SetNotificationComplete(model);
            }
            else if (model.Command == KeyDatesStatusEnum.NotificationTransmitted)
            {
                await SetNotificationTransmitted(model);
            }
            else if (model.Command == KeyDatesStatusEnum.NotificationAcknowledged)
            {
                await SetAcknowledged(model);
            }
            else if (model.Command == KeyDatesStatusEnum.FileClosed)
            {
                await FileClosed(model);
            }
            else if (model.Command == KeyDatesStatusEnum.ArchiveReference)
            {
                await SetArchiveReference(model);
            }
            else
            {
                throw new InvalidOperationException();
            }

            return RedirectToAction("Index", new { id = model.NotificationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnlockNotification(Guid id)
        {
            await mediator.SendAsync(new UnlockNotification(id));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> AcceptChanges(Guid id)
        {
            var data = await mediator.SendAsync(new GetKeyDatesSummaryInformation(id));

            var model = new DateInputViewModel(data.Dates)
            {
                IsAreaAssigned = data.IsLocalAreaSet,
                CompetentAuthority = data.CompetentAuthority,
                AssessmentDecisions = data.DecisionHistory,
                IsInterim = data.IsInterim,
                AdditionalCharge = new AdditionalChargeData()
                {
                    NotificationId = id
                },
                CurrentStatus = data.Dates.CurrentStatus,
                ShowAdditionalCharge = (data.CompetentAuthority == UKCompetentAuthority.England || data.CompetentAuthority == UKCompetentAuthority.Scotland) ? true : false
            };

            model.ShowAssessmentDecisionLink = await authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AcceptChanges(DateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ShowAssessmentDecisionLink = await authorizationService.AuthorizeActivity(ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision);

                return View(model);
            }

            await mediator.SendAsync(new AcceptChanges(model.NotificationId));

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = CreateAdditionalChargeData(model.NotificationId, model.AdditionalCharge, AdditionalChargeType.AcceptNotification);

                    await additionalChargeService.AddAdditionalCharge(mediator, addtionalCharge);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectChanges(Guid id)
        {
            await mediator.SendAsync(new RejectChanges(id));

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetDefaultAdditionalChargeAmount(UKCompetentAuthority competentAuthority)
        {
            var response = new Core.SystemSetting.SystemSettingData();
            if (competentAuthority == UKCompetentAuthority.England)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(4)); //EA
            }
            else if (competentAuthority == UKCompetentAuthority.Scotland)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(5)); //SEPA
            }

            return Json(response.Value);
        }

        private static CreateAdditionalCharge CreateAdditionalChargeData(Guid notificationId, AdditionalChargeData model, AdditionalChargeType additionalChargeType)
        {
            var createAddtionalCharge = new CreateAdditionalCharge()
            {
                ChangeDetailType = additionalChargeType,
                ChargeAmount = model.Amount,
                Comments = model.Comments,
                NotificationId = notificationId
            };

            return createAddtionalCharge;
        }

        private async Task SetNotificationTransmitted(DateInputViewModel model)
        {
            var setNotificationTransmitted = new SetNotificationTransmittedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            await mediator.SendAsync(setNotificationTransmitted);
        }

        private async Task SetNotificationComplete(DateInputViewModel model)
        {
            var setNotificationComplete = new SetNotificationCompleteDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            await mediator.SendAsync(setNotificationComplete);
        }

        private async Task SetNotificationReceived(DateInputViewModel model)
        {
            var setNotificationReceivedDate = new SetNotificationReceivedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            await mediator.SendAsync(setNotificationReceivedDate);
        }

        private async Task SetAssessmentCommenced(DateInputViewModel model)
        {
            var setAssessmentCommenced = new SetCommencedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault(), model.NameOfOfficer);

            await mediator.SendAsync(setAssessmentCommenced);
        }

        private async Task SetAcknowledged(DateInputViewModel model)
        {
            var setAcknowledged = new SetNotificationAcknowledgedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            await mediator.SendAsync(setAcknowledged);
        }

        private async Task FileClosed(DateInputViewModel model)
        {
            var fileClosed = new SetNotifcationFileClosedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            await mediator.SendAsync(fileClosed);
        }

        private async Task SetArchiveReference(DateInputViewModel model)
        {
            await mediator.SendAsync(new SetArchiveReference(model.NotificationId, model.ArchiveReference));
        }

        private void AddRelevantDateToNewDate(DateInputViewModel model)
        {
            var command = model.Command;

            switch (command)
            {
                case KeyDatesStatusEnum.NotificationReceived:
                    model.NewDate = model.NotificationReceivedDate;
                    break;

                case KeyDatesStatusEnum.AssessmentCommenced:
                    model.NewDate = model.CommencementDate;
                    break;

                case KeyDatesStatusEnum.NotificationComplete:
                    model.NewDate = model.NotificationCompleteDate;
                    break;

                case KeyDatesStatusEnum.NotificationTransmitted:
                    model.NewDate = model.NotificationTransmittedDate;
                    break;

                case KeyDatesStatusEnum.NotificationAcknowledged:
                    model.NewDate = model.NotificationAcknowledgedDate;
                    break;

                case KeyDatesStatusEnum.NotificationDecisionDateEntered:
                    model.NewDate = model.DecisionDate;
                    break;
            }
        }
    }
}