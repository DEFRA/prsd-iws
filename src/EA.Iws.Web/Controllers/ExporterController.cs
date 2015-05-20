namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Exporters;
    using Requests.Shared;
    using ViewModels.Exporter;

    [Authorize]
    public class ExporterController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ExporterController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var facility = new ExporterViewModel
            {
                NotificationId = id
            };

            await this.BindCountryList(apiClient);

            return View(facility);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ExporterViewModel model)
        {
            using (var client = apiClient())
            {
                var exporter = new ExporterData
                {
                    NotificationId = model.NotificationId,
                    Address = model.Address,
                    Business = (BusinessData)model.Business,
                    Contact = model.Contact
                };

                var response = await client.SendAsync(User.GetAccessToken(), new AddExporterToNotification(exporter));

                if (response.HasErrors)
                {
                    await this.BindCountryList(client);

                    return View(model);
                }

                return RedirectToAction("Add", "Producer", new { id = model.NotificationId});
            }
        }
    }
}