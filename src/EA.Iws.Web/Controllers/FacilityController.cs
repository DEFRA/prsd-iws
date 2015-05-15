namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Facilities;
    using Requests.Registration;
    using Requests.Shared;

    [Authorize]
    public class FacilityController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public FacilityController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            NotificationType notificationType;

            using (var client = apiClient())
            {
                var response = await client.SendAsync(new NotificationTypeByNotificationId(id));

                await BindCountrySelectList(client);

                if (response.HasErrors)
                {
                    // TODO: Error handling
                }

                notificationType = response.Result;
            }

            var facility = new FacilityData
            {
                NotificationId = id,
                NotificationType = notificationType
            };

            return View(facility);
        }

        [HttpPost]
        public async Task<ActionResult> Add(FacilityData model)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new AddFacilityToNotification(model));

                if (response.HasErrors)
                {
                    await BindCountrySelectList(client);

                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }
        }

        private async Task BindCountrySelectList()
        {
            using (var client = apiClient())
            {
                await BindCountrySelectList(client);
            }
        }

        private async Task BindCountrySelectList(IIwsClient client)
        {
            var response = await client.SendAsync(new GetCountries());

            if (response.HasErrors)
            {
                // TODO: Error handling
            }

            ViewBag.Countries = new SelectList(response.Result, "Id", "Name", 
                response.Result.Single(c => c.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Id);
        }
    }
}