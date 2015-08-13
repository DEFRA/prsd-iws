namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Admin;
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
        public ActionResult DateInput(Guid id)
        {
            return View(new DateInputViewModel{NotificationId = id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> DateInput(DateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var setDates = new SetDates
            {
                NotificationReceivedDate = model.NotificationReceivedDate.AsDateTime(),
                PaymentReceivedDate = model.PaymentReceivedDate.AsDateTime(),
                CommencementDate = model.CommencementDate.AsDateTime(),
                CompleteDate = model.NotificationCompleteDate.AsDateTime(),
                TransmittedDate = model.NotificationTransmittedDate.AsDateTime(),
                AcknowledgedDate = model.NotificationAcknowledgedDate.AsDateTime(),
                DecisionDate = model.DecisionDate.AsDateTime(),
                NameOfOfficer = model.NameOfOfficer,
                NotificationApplicationId = model.NotificationId
            };

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setDates);
            }
            return View(model);
        }
    }
}