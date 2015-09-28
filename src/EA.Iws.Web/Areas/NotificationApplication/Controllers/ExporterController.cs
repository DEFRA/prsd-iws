namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Exporters;
    using ViewModels.Exporter;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ExporterController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ExporterController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                ExporterViewModel model;
                var exporter = await client.SendAsync(User.GetAccessToken(), new GetExporterByNotificationId(id));
                if (exporter.HasExporter)
                {
                    model = new ExporterViewModel(exporter);
                }
                else
                {
                    model = new ExporterViewModel
                    {
                        NotificationId = id
                    };
                }

                await this.BindCountryList(apiClient);
                model.Address.DefaultCountryId = this.GetDefaultCountryId();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ExporterViewModel model, bool? backToOverview = null)
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
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());

                    if (backToOverview.GetValueOrDefault())
                    {
                        return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                    }
                    else
                    {
                        return RedirectToAction("List", "Producer", new { id = model.NotificationId });
                    }
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

            await this.BindCountryList(apiClient);
            return View(model);
        }
    }
}