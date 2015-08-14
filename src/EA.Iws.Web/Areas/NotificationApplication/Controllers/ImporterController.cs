namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Importer;
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
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                ImporterViewModel model;
                var importer = await client.SendAsync(User.GetAccessToken(), new GetImporterByNotificationId(id));
                if (importer.HasImporter)
                {
                    model = new ImporterViewModel(importer);
                }
                else
                {
                    model = new ImporterViewModel { NotificationId = id };
                }

                await this.BindCountryList(apiClient, false);
                model.Address.DefaultCountryId = this.GetDefaultCountryId();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ImporterViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient, false);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());
                    if (backToOverview.GetValueOrDefault())
                    {
                        return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                    }
                    else
                    {
                        return RedirectToAction("List", "Facility", new { id = model.NotificationId });
                    }
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                await this.BindCountryList(client, false);
                return View(model);
            }
        }
    }
}