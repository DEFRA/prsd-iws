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
    using Requests.Shared;
    using ViewModels.Importer;
    using Web.ViewModels.Shared;

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

            await this.BindCountryList(apiClient, false);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ImporterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient, false);
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
                await this.BindCountryList(client, false);
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

                await this.BindCountryList(apiClient, false);
                model.Address.DefaultCountryId = this.GetDefaultCountryId();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ImporterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient, false);
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

                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
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

            await this.BindCountryList(apiClient, false);
            return View(model);
        }
    }
}