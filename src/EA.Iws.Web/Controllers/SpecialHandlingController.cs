namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Notification;
    using ViewModels.SpecialHandling;

    public class SpecialHandlingController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public SpecialHandlingController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var model = new SpecialHandlingViewModel { NotificationId = id };
            using (var client = apiClient())
            {
                var specialHandlingData =
                    await client.SendAsync(User.GetAccessToken(), new GetSpecialHandingForNotification(id));

                if (specialHandlingData.HasSpecialHandlingRequirements.HasValue)
                {
                    model.HasSpecialHandlingRequirements = specialHandlingData.HasSpecialHandlingRequirements;
                    model.SpecialHandlingDetails = specialHandlingData.SpecialHandlingDetails;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SpecialHandlingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetSpecialHandling(model.NotificationId, model.HasSpecialHandlingRequirements.GetValueOrDefault(),
                            model.SpecialHandlingDetails));
            }
            return RedirectToAction("Add", "StateOfExport", new { id = model.NotificationId });
        }
    }
}