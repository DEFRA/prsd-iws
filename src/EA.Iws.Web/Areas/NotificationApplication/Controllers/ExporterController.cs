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
    public class ExporterController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ExporterController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
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
        public async Task<ActionResult> Index(ExporterViewModel model)
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

                    return RedirectToAction("CopyFromExporter", "Producer", new { id = model.NotificationId });
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