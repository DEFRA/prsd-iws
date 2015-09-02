namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using Api.Client;
    using Infrastructure;
    using Microsoft.Ajax.Utilities;
    using Requests.Admin.NotificationAssessment;
    using ViewModels;

    [Authorize(Roles = "internal")]
    public class KeyDatesController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public KeyDatesController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, KeyDatesStatusEnum? command)
        {
            using (var client = apiClient())
            {
                var dates = await client.SendAsync(User.GetAccessToken(), new GetDates(id));
                var model = new DateInputViewModel(dates);
                if (command != null)
                {
                    model.Command = command.GetValueOrDefault();
                    AddRelevantDateToNewDate(model);
                }

                return View(model);
            }
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
            else if (model.Command == KeyDatesStatusEnum.PaymentReceived)
            {
                await SetPaymentReceived(model);
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
            else if (model.Command == KeyDatesStatusEnum.NotificationDecisionDateEntered)
            {
                await SetDecisionRequiredByDate(model);
            }
            else
            {
                throw new InvalidOperationException();
            }

            return RedirectToAction("Index", new { id = model.NotificationId });
        }

        private async Task SetNotificationTransmitted(DateInputViewModel model)
        {
            var setNotificationTransmitted = new SetNotificationTransmittedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setNotificationTransmitted);
            }
        }

        private async Task SetNotificationComplete(DateInputViewModel model)
        {
            var setNotificationComplete = new SetNotificationCompleteDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setNotificationComplete);
            }
        }

        private async Task SetPaymentReceived(DateInputViewModel model)
        {
            var setPaymentReceivedDate = new SetPaymentReceivedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setPaymentReceivedDate);
            }
        }

        private async Task SetNotificationReceived(DateInputViewModel model)
        {
            var setNotificationReceivedDate = new SetNotificationReceivedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setNotificationReceivedDate);
            }
        }

        private async Task SetAssessmentCommenced(DateInputViewModel model)
        {
            var setAssessmentCommenced = new SetCommencedDate(model.NotificationId, 
                model.NewDate.AsDateTime().GetValueOrDefault(), model.NameOfOfficer);

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setAssessmentCommenced);
            }
        }

        private async Task SetAcknowledged(DateInputViewModel model)
        {
            var setAcknowledged = new SetNotificationAcknowledgedDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setAcknowledged);
            }
        }

        private async Task SetDecisionRequiredByDate(DateInputViewModel model)
        {
            var setDecisionRequiredByDate = new SetNotificationDecisionRequiredByDate(model.NotificationId,
                model.NewDate.AsDateTime().GetValueOrDefault());

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setDecisionRequiredByDate);
            }
        }

        private void AddRelevantDateToNewDate(DateInputViewModel model)
        {
            var command = model.Command;

            switch (command)
            {
                case KeyDatesStatusEnum.NotificationReceived:
                    model.NewDate = model.NotificationReceivedDate;
                    break;

                case KeyDatesStatusEnum.PaymentReceived:
                    model.NewDate = model.PaymentReceivedDate;
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