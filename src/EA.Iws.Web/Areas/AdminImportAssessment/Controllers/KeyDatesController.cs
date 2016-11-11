namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using ViewModels.KeyDates;

    [Authorize(Roles = "internal")]
    public class KeyDatesController : Controller
    {
        private readonly IMediator mediator;

        public KeyDatesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, KeyDatesCommand? command)
        {
            var data = await mediator.SendAsync(new GetKeyDates(id));

            var model = new KeyDatesViewModel(data);

            if (command.HasValue)
            {
                model.Command = command.Value;
                AddRelevantDateToNewDate(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, KeyDatesViewModel model)
        {
            if (!ModelState.IsValid)
            {
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