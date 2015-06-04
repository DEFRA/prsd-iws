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
    using ViewModels.Shared;

    [Authorize]
    public class ImporterController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ImporterController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        private async Task<bool> HasImporter(Guid notificationId)
        {
            using (var client = apiClient())
            {
                return await client.SendAsync(User.GetAccessToken(), new NotificationHasImporter(notificationId));
            }
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            if (await HasImporter(id))
            {
                return RedirectToAction("Edit", "Importer", new { id = id });
            }

            var model = new ImporterViewModel { NotificationId = id };

            model.Address.DefaultCountryId = await this.BindCountryList(apiClient);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ImporterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            var importer = new AddImporterToNotification
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
                    await client.SendAsync(User.GetAccessToken(), importer);

                    return RedirectToAction("CopyFromImporter", "Facility", new { id = model.NotificationId });
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

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!await HasImporter(id))
            {
                return RedirectToAction("Add", "Importer", new { id = id });
            }

            using (var client = apiClient())
            {
                var model = new ImporterViewModel();
                var importer = await client.SendAsync(User.GetAccessToken(), new GetImporterByNotificationId(id));

                model.NotificationId = id;
                model.Address = importer.Address;
                model.Contact = importer.Contact;
                model.Business = (BusinessViewModel)importer.Business;

                model.Address.DefaultCountryId = await this.BindCountryList(apiClient);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ImporterViewModel model)
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
                    var importer = new UpdateImporterForNotification
                    {
                        NotificationId = model.NotificationId,
                        Address = model.Address,
                        Business = (BusinessData)model.Business,
                        Contact = model.Contact
                    };

                    await client.SendAsync(User.GetAccessToken(), importer);

                    return RedirectToAction("NotificationOverview", "NotificationApplication", new { id = model.NotificationId });
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