namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Carriers;
    using Requests.Shared;
    using ViewModels.Carrier;

    public class CarrierController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public CarrierController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new CarrierViewModel { NotificationId = id };
            await this.BindCountryList(apiClient);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CarrierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            var carrier = new AddCarrierToNotification
            {
                NotificationId = model.NotificationId,
                Address = model.Address,
                Business = (BusinessData)model.Business,
                Contact = model.Contact
            };
            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), carrier);

                    // TODO: navigate to multiple carriers screen once implemented
                    return RedirectToAction("Home", "Applicant", new { id = model.NotificationId });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }

                await this.BindCountryList(client);
                return View(model);
            }
        }
    }
}