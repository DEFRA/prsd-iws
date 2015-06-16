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
    using ViewModels.Exporter;

    [Authorize]
    public class ExporterController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ExporterController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        private async Task<bool> HasExporter(Guid notificationId)
        {
            using (var client = apiClient())
            {
                return await client.SendAsync(User.GetAccessToken(), new NotificationHasExporter(notificationId));
            }
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            if (await HasExporter(id))
            {
                return RedirectToAction("Edit", "Exporter", new { id });
            }

            var model = new ExporterViewModel
            {
                NotificationId = id
            };

            await this.BindCountryList(apiClient);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    await client.SendAsync(User.GetAccessToken(), model.ToAddRequest());

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

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!await HasExporter(id))
            {
                return RedirectToAction("Add", "Exporter", new { id });
            }

            using (var client = apiClient())
            {
                var exporter = await client.SendAsync(User.GetAccessToken(), new GetExporterByNotificationId(id));
                var model = new ExporterViewModel(exporter);

                await this.BindCountryList(apiClient);
                model.Address.DefaultCountryId = this.GetDefaultCountryId();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ExporterViewModel model)
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
                    await client.SendAsync(User.GetAccessToken(), model.ToUpdateRequest());

                    return RedirectToAction("NotificationOverview", "NotificationApplication",
                        new { id = model.NotificationId });
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