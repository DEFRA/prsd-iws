namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Facilities;
    using Requests.Importer;
    using Requests.Notification;
    using Requests.Shared;
    using ViewModels.Facility;
    using ViewModels.Shared;

    [Authorize]
    public class FacilityController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public FacilityController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? copy)
        {
            var facility = new AddFacilityViewModel();
            using (var client = apiClient())
            {
                if (copy.HasValue && copy.Value)
                {
                    var importer = await client.SendAsync(User.GetAccessToken(), new GetImporterByNotificationId(id));

                    facility.Address = importer.Address;
                    facility.Contact = importer.Contact;
                    facility.Business = (BusinessViewModel)importer.Business;
                }

                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));
                facility.NotificationType = response.NotificationType;
                facility.NotificationId = id;

                await this.BindCountryList(client);
                facility.Address.DefaultCountryId = this.GetDefaultCountryId();
            }
            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddFacilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());

                    return RedirectToAction("MultipleFacilities", "Facility",
                        new { id = model.NotificationId });
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
        public async Task<ActionResult> Edit(Guid id, Guid facilityId)
        {
            using (var client = apiClient())
            {
                var facility =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetFacilityForNotification(id, facilityId));

                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                var model = new EditFacilityViewModel(facility) { NotificationType = response.NotificationType };

                await this.BindCountryList(client);
                facility.Address.DefaultCountryId = this.GetDefaultCountryId();

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditFacilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var request = model.ToRequest();

                    await client.SendAsync(User.GetAccessToken(), request);

                    return RedirectToAction("MultipleFacilities", "Facility",
                        new { id = model.NotificationId });
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
        public async Task<ActionResult> MultipleFacilities(Guid id)
        {
            var model = new MultipleFacilitiesViewModel();

            using (var client = apiClient())
            {
                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetFacilitiesByNotificationId(id));

                var notificationInfo =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));

                model.NotificationId = id;
                model.FacilityData = response.ToList();
                model.NotificationType = notificationInfo.NotificationType;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MultipleFacilities(MultipleFacilitiesViewModel model)
        {
            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), new SetActualSiteOfTreatment(new Guid(model.SelectedValue), model.NotificationId));
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
            }

            if (String.Equals(model.NotificationType.ToString(), NotificationType.Recovery.ToString(), StringComparison.InvariantCulture))
            {
                return RedirectToAction("RecoveryPreconsent", "Facility", new { id = model.NotificationId });
            }

            return RedirectToAction("Add", "Carrier", new { id = model.NotificationId });
        }

        [HttpGet]
        public ActionResult RecoveryPreconsent(Guid id)
        {
            var model = new YesNoChoiceViewModel();
            ViewBag.NotificationId = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryPreconsent(Guid id, YesNoChoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = id;
                return View(model);
            }

            bool isPreconsented = !string.IsNullOrWhiteSpace(model.Choices.SelectedValue) &&
                                  ("Yes".Equals(model.Choices.SelectedValue, StringComparison.InvariantCultureIgnoreCase) ||
                                   "True".Equals(model.Choices.SelectedValue, StringComparison.InvariantCultureIgnoreCase));
            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new SetPreconsentedRecoveryFacility(id, isPreconsented));
            }

            return RedirectToAction("Add", "Carrier", new { id });
        }

        [HttpGet]
        public async Task<ActionResult> CopyFromImporter(Guid id)
        {
            var model = new YesNoChoiceViewModel();
            ViewBag.NotificationId = id;

            using (var client = apiClient())
            {
                var notificationInfo =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));
                ViewBag.NotificationType = notificationInfo.NotificationType.ToString().ToLowerInvariant();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CopyFromImporter(Guid id, YesNoChoiceViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = id;

                using (var client = apiClient())
                {
                    var notificationInfo =
                        await client.SendAsync(User.GetAccessToken(), new GetNotificationInfo(id));
                    ViewBag.NotificationType = notificationInfo.NotificationType.ToString().ToLowerInvariant();
                }

                return View(inputModel);
            }

            if (inputModel.Choices.SelectedValue.Equals("No"))
            {
                return RedirectToAction("Add", new { id });
            }

            return RedirectToAction("Add", new { id, copy = true });
        }
    }
}