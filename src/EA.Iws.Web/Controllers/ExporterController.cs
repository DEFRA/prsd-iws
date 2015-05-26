namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
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
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            try
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

                    var response =
                        await client.SendAsync(User.GetAccessToken(), new AddExporterToNotification(exporter));

                    return RedirectToAction("Add", "Producer", new { id = model.NotificationId });
                }
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
                return View(model);
            }
        }
    }
}