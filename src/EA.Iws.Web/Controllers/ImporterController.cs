namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Importer;
    using Requests.Shared;
    using ViewModels.Importer;

    [Authorize]
    public class ImporterController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ImporterController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new ImporterViewModel { NotificationId = id };

            await this.BindCountryList(apiClient);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ImporterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            var importer = new ImporterData
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
                    var response = await client.SendAsync(User.GetAccessToken(), new AddImporterToNotification(importer));

                    return RedirectToAction("Add", "Facility", new { id = model.NotificationId });
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