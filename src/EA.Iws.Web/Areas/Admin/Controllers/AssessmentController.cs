namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Admin;
    using ViewModels;

    public class AssessmentController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public AssessmentController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult DateInput(Guid id)
        {
            return View(new DateInputViewModel{NotificationId = id});
        }

        [HttpPost]
        public async Task<ViewResult> DateInput(DateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var setDates = new SetDates
            {
                NotificationReceivedDate = GetDateFromUserInput(model.NotificationReceivedDay, model.NotificationReceivedMonth, model.NotificationReceivedYear),
                PaymentRecievedDate = GetDateFromUserInput(model.PaymentReceivedDay, model.PaymentReceivedMonth, model.PaymentReceivedYear),
                CommencementDate = GetDateFromUserInput(model.CommencementDay, model.CommencementMonth, model.CommencementYear),
                CompleteDate = GetDateFromUserInput(model.NotificationCompleteDay, model.NotificationCompleteMonth, model.NotificationCompleteYear),
                TransmittedDate = GetDateFromUserInput(model.NotificationTransmittedDay, model.NotificationTransmittedMonth, model.NotificationTransmittedYear),
                AcknowledgedDate = GetDateFromUserInput(model.NotificationAcknowledgedDay, model.NotificationAcknowledgedMonth, model.NotificationAcknowledgedYear),
                DecisionDate = GetDateFromUserInput(model.DecisionDay, model.DecisionMonth, model.DecisionYear),
                NotificationApplicationId = model.NotificationId
            };

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setDates);
            }
            return View(model);
        }

        private DateTime? GetDateFromUserInput(int? day, int? month, int? year)
        {
            if (day.HasValue && month.HasValue && year.HasValue)
            {
                return new DateTime(year.Value, month.Value, day.Value);
            }
            return null;
        }
    }
}