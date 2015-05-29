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
    using Requests.Registration;
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
        public async Task<ActionResult> Add(Guid id, string facilityType, bool? copy)
        {
            var facility = new FacilityViewModel();
            using (var client = apiClient())
            {
                if (copy.HasValue && copy.Value)
                {
                    var importer = await client.SendAsync(User.GetAccessToken(), new GetImporterByNotificationId(id));

                    facility.Address = importer.Address;
                    facility.Contact = importer.Contact;
                    facility.Business = (BusinessViewModel)importer.Business;
                }

                await this.BindCountryList(client);
            }

            facility.NotificationId = id;
            facility.NotificationType = (NotificationType)Enum.Parse(typeof(NotificationType), facilityType, true);

            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(FacilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            var facility = new FacilityData
                    {
                        NotificationType = model.NotificationType,
                        NotificationId = model.NotificationId,
                        IsActualSiteOfTreatment = model.IsActualSiteOfTreatment,
                        Address = model.Address,
                        Contact = model.Contact,
                        Business = (BusinessData)model.Business
                    };

            using (var client = apiClient())
            {
                try
                {
                    var response =
                        await client.SendAsync(User.GetAccessToken(), new AddFacilityToNotification(facility));

                    return RedirectToAction("MultipleFacilities", "Facility",
                        new { notificationID = model.NotificationId });
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
        public async Task<ActionResult> MultipleFacilities(Guid notificationId, string errorMessage = "")
        {
            var model = new MultipleFacilitiesViewModel();

            if (!String.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            using (var client = apiClient())
            {
                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetFacilitiesByNotificationId(notificationId));

                model.NotificationId = notificationId;
                model.FacilityData = response.ToList();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MultipleFacilities(MultipleFacilitiesViewModel model)
        {
            return RedirectToAction("Add", "Carrier", new { id = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> CopyFromImporter(Guid id)
        {
            var model = new YesNoChoiceViewModel();
            ViewBag.NotificationId = id;

            using (var client = apiClient())
            {
                NotificationType notificationType = await client.SendAsync(User.GetAccessToken(), new NotificationTypeByNotificationId(id));
                ViewBag.NotificationType = notificationType.ToString().ToLowerInvariant();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CopyFromImporter(Guid id, string facilityType, YesNoChoiceViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = id;
                ViewBag.NotificationType = facilityType;
                return View(inputModel);
            }

            if (inputModel.Choices.SelectedValue.Equals("No"))
            {
                return RedirectToAction("Add", new { id, facilityType });
            }

            return RedirectToAction("Add", new { id, facilityType, copy = true });
        }
    }
}