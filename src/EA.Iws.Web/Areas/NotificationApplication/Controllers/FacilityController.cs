namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Facilities;
    using Requests.Notification;
    using ViewModels.Facility;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class FacilityController : Controller
    {
        private readonly IMediator client;

        public FacilityController(IMediator client)
        {
            this.client = client;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? backToOverview = null)
        {
            var facility = new AddFacilityViewModel();

            var response =
                await client.SendAsync(new GetNotificationBasicInfo(id));
            facility.NotificationType = response.NotificationType;
            facility.NotificationId = id;

            await this.BindCountryList(client, false);
            facility.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddFacilityViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(client, false);
                return View(model);
            }

            try
            {
                await client.SendAsync(model.ToRequest());

                return RedirectToAction("List", "Facility",
                    new { id = model.NotificationId, backToOverview });
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

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid entityId, bool? backToOverview = null)
        {
            var facility =
                await
                    client.SendAsync(new GetFacilityForNotification(id, entityId));

            var response =
                await client.SendAsync(new GetNotificationBasicInfo(id));

            var model = new EditFacilityViewModel(facility) { NotificationType = response.NotificationType };

            await this.BindCountryList(client, false);
            facility.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditFacilityViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(client, false);
                return View(model);
            }

            try
            {
                var request = model.ToRequest();

                await client.SendAsync(request);

                return RedirectToAction("List", "Facility",
                    new { id = model.NotificationId, backToOverview });
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

        [HttpGet]
        public async Task<ActionResult> List(Guid id, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var model = new MultipleFacilitiesViewModel();

            var response =
                await client.SendAsync(new GetFacilitiesByNotificationId(id));

            var notificationInfo =
                await client.SendAsync(new GetNotificationBasicInfo(id));

            model.NotificationId = id;
            model.FacilityData = response;
            model.NotificationType = notificationInfo.NotificationType;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> SiteOfTreatment(Guid id, bool? backToList, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var response =
                await client.SendAsync(new GetFacilitiesByNotificationId(id));

            var notificationInfo =
                await client.SendAsync(new GetNotificationBasicInfo(id));

            var model = new SiteOfTreatmentViewModel
            {
                NotificationId = id,
                Facilities = response,
                NotificationType = notificationInfo.NotificationType
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SiteOfTreatment(SiteOfTreatmentViewModel model, bool? backToList,
            bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview ?? false;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await
                    client.SendAsync(
                        new SetActualSiteOfTreatment(model.SelectedSiteOfTreatment.GetValueOrDefault(),
                            model.NotificationId));

                if (backToList.GetValueOrDefault())
                {
                    return RedirectToAction("List", "Facility", new { id = model.NotificationId, backToOverview });
                }
                if (backToOverview.GetValueOrDefault())
                {
                    return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                }
                if (model.NotificationType == NotificationType.Recovery)
                {
                    return RedirectToAction("RecoveryPreconsent", "Facility", new { id = model.NotificationId });
                }
                return RedirectToAction("OperationCodes", "WasteOperations", new { id = model.NotificationId });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> RecoveryPreconsent(Guid id, bool? backToOverview = null)
        {
            var preconsentedFacilityData =
                await client.SendAsync(new GetIsPreconsentedRecoveryFacility(id));

            if (preconsentedFacilityData.NotificationType == NotificationType.Disposal)
            {
                return RedirectToAction("OperationCodes", "WasteOperations", new { id });
            }

            var model = new TrueFalseViewModel { Value = preconsentedFacilityData.IsPreconsentedRecoveryFacility };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryPreconsent(Guid id, TrueFalseViewModel model,
            bool? backToOverview = null)
        {
            if (!ModelState.IsValid || !model.Value.HasValue)
            {
                ViewBag.NotificationId = id;
                return View(model);
            }

            var isPreconsented = model.Value.Value;

            await client.SendAsync(new SetPreconsentedRecoveryFacility(id, isPreconsented));

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }
            return RedirectToAction("OperationCodes", "WasteOperations", new { id });
        }

        [HttpGet]
        public async Task<ActionResult> Remove(Guid id, Guid entityId, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var notificationInfo =
                await client.SendAsync(new GetNotificationBasicInfo(id));

            var response = await client.SendAsync(new GetFacilitiesByNotificationId(id));
            var facility = response.Single(p => p.Id == entityId);

            var model = new RemoveFacilityViewModel
            {
                NotificationId = id,
                FacilityId = entityId,
                FacilityName = facility.Business.Name,
                NotificationType = notificationInfo.NotificationType
            };

            if (facility.IsActualSiteOfTreatment && response.Count > 1)
            {
                ViewBag.Error =
                    string.Format("You have chosen to remove {0} which is the site of {1}. " +
                                  "You will need to select an alternative site of {1} before you can remove this facility.",
                        model.FacilityName, model.NotificationType.ToString().ToLowerInvariant());
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(RemoveFacilityViewModel model, bool? backToOverview = null)
        {
            try
            {
                await client.SendAsync(new DeleteFacilityForNotification(model.NotificationId, model.FacilityId));
                return RedirectToAction("List", "Facility", new { id = model.NotificationId, backToOverview });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            return View(model);
        }
    }
}