namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotificationAssessment;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.KeyDates;

    [AuthorizeActivity(typeof(GetKeyDates))]
    public class KeyDatesController : Controller
    {
        private readonly IMediator mediator;
        private readonly AuthorizationService authorizationService;

        public KeyDatesController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, KeyDatesCommand? command)
        {
            var data = await mediator.SendAsync(new GetKeyDates(id));

            var model = new KeyDatesViewModel(data);
            model.NotificationId = id;

            if (command.HasValue)
            {
                model.Command = command.Value;
                AddRelevantDateToNewDate(model);
            }

            model.ShowAssessmentDecisionLink = await authorizationService.AuthorizeActivity(ImportNotificationPermissions.CanMakeImportNotificationAssessmentDecision);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, KeyDatesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ShowAssessmentDecisionLink = await authorizationService.AuthorizeActivity(ImportNotificationPermissions.CanMakeImportNotificationAssessmentDecision);

                return View(model);
            }

            switch (model.Command)
            {
                case KeyDatesCommand.BeginAssessment:
                    await
                        mediator.SendAsync(new SetAssessmentStartedDate(id, model.NewDate.AsDateTime().Value,
                            model.NameOfOfficer));
                    break;
                case KeyDatesCommand.NotificationComplete:
                    await mediator.SendAsync(new SetNotificationCompletedDate(id, model.NewDate.AsDateTime().Value));
                    break;
                case KeyDatesCommand.NotificationAcknowledged:
                    await mediator.SendAsync(new SetAcknowlegedDate(id, model.NewDate.AsDateTime().Value));
                    break;
                case KeyDatesCommand.FileClosed:
                    await mediator.SendAsync(new SetNotificationFileClosedDate(id, model.NewDate.AsDateTime().Value));
                    break;
                case KeyDatesCommand.ArchiveReference:
                    await mediator.SendAsync(new SetArchiveReference(id, model.ArchiveReference));
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Suspend(Guid id)
        {
            await mediator.SendAsync(new SetUnderProhibitionStatus(id, DateTime.Now));

            return RedirectToAction("Index", "KeyDates");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnSuspend(Guid id)
        {
            await mediator.SendAsync(new SetUnderProhibitionStatus(id, DateTime.Now));

            return RedirectToAction("Index", "KeyDates");
        }

        private void AddRelevantDateToNewDate(KeyDatesViewModel model)
        {
            var command = model.Command;

            switch (command)
            {
                case KeyDatesCommand.BeginAssessment:
                    model.NewDate = model.AssessmentStartedDate;
                    break;

                case KeyDatesCommand.NotificationComplete:
                    model.NewDate = model.NotificationCompleteDate;
                    break;

                case KeyDatesCommand.NotificationAcknowledged:
                    model.NewDate = model.NotificationAcknowledgedDate;
                    break;

                default:
                    break;
            }
        }
    }
}