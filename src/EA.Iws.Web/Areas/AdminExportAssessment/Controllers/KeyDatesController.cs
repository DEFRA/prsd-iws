namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;
    using Requests.NotificationAssessment;
    using ViewModels;

    [Authorize(Roles = "internal")]
    public class KeyDatesController : Controller
    {
        private readonly IMediator mediator;

        public KeyDatesController(IMediator mediator)
        {
            this.mediator = mediator;
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
                FinancialGuaranteeDecisions = data.FinancialGuaranteeDecisions
            };

            if (command != null)
            {
                model.Command = command.GetValueOrDefault();
                AddRelevantDateToNewDate(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(DateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AcceptChanges(Guid id)
        {
            await mediator.SendAsync(new AcceptChanges(id));

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectChanges(Guid id)
        {
            await mediator.SendAsync(new RejectChanges(id));

            return RedirectToAction("Index");
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