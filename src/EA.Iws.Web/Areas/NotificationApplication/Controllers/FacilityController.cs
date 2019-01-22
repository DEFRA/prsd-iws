namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Core.Shared;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.AddressBook;
    using Requests.Facilities;
    using Requests.Notification;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Facility;
    using Web.ViewModels.Shared;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class FacilityController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<AddFacilityViewModel, AddAddressBookEntry> addFacilityAddressBookMap;
        private readonly IAuditService auditService;

        public FacilityController(IMediator mediator, IMap<AddFacilityViewModel, AddAddressBookEntry> addFacilityAddressBookMap, IAuditService auditService)
        {
            this.mediator = mediator;
            this.addFacilityAddressBookMap = addFacilityAddressBookMap;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? backToOverview = null)
        {
            var facility = new AddFacilityViewModel();

            var response =
                await mediator.SendAsync(new GetNotificationBasicInfo(id));
            facility.NotificationType = response.NotificationType;
            facility.NotificationId = id;

            await this.BindCountryList(mediator, false);
            facility.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddFacilityViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator, false);
                return View(model);
            }

            try
            {
                await mediator.SendAsync(model.ToRequest());

                await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Added,
                    model.NotificationType == NotificationType.Disposal ? NotificationAuditScreenType.DisposalFacilities : NotificationAuditScreenType.RecoveryFacilities);

                if (model.IsAddedToAddressBook)
                {
                    await mediator.SendAsync(addFacilityAddressBookMap.Map(model));
                }

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
            await this.BindCountryList(mediator, false);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid entityId, bool? backToOverview = null)
        {
            var facility =
                await
                    mediator.SendAsync(new GetFacilityForNotification(id, entityId));

            var response =
                await mediator.SendAsync(new GetNotificationBasicInfo(id));

            var model = new EditFacilityViewModel(facility) { NotificationType = response.NotificationType };

            await this.BindCountryList(mediator, false);
            facility.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditFacilityViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator, false);
                return View(model);
            }

            try
            {
                var request = model.ToRequest();

                await mediator.SendAsync(request);

                await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    model.NotificationType == NotificationType.Disposal ? NotificationAuditScreenType.DisposalFacilities : NotificationAuditScreenType.RecoveryFacilities);

                if (model.IsAddedToAddressBook)
                {
                    await mediator.SendAsync(addFacilityAddressBookMap.Map(model));
                }

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
            await this.BindCountryList(mediator, false);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> List(Guid id, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var model = new MultipleFacilitiesViewModel();

            var response =
                await mediator.SendAsync(new GetFacilitiesByNotificationId(id));

            var notificationInfo =
                await mediator.SendAsync(new GetNotificationBasicInfo(id));

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
                await mediator.SendAsync(new GetFacilitiesByNotificationId(id));

            var notificationInfo =
                await mediator.SendAsync(new GetNotificationBasicInfo(id));

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
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var existingFacilities = await mediator.SendAsync(new GetFacilitiesByNotificationId(model.NotificationId));

                await
                    mediator.SendAsync(
                        new SetActualSiteOfTreatment(model.SelectedSiteOfTreatment.GetValueOrDefault(),
                            model.NotificationId));

                NotificationAuditType type = NotificationAuditType.Added;

                if (existingFacilities != null && existingFacilities.Count(p => p.IsActualSiteOfTreatment) > 0)
                {
                    type = NotificationAuditType.Updated;
                }

                await this.auditService.AddAuditEntry(mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    type,
                    model.NotificationType == NotificationType.Disposal ? NotificationAuditScreenType.DisposalSite : NotificationAuditScreenType.RecoverySite);

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
                await mediator.SendAsync(new GetIsPreconsentedRecoveryFacility(id));

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

            var preconsentedFacilityData = await mediator.SendAsync(new GetIsPreconsentedRecoveryFacility(id));

            var isPreconsented = model.Value.Value;

            await mediator.SendAsync(new SetPreconsentedRecoveryFacility(id, isPreconsented));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    preconsentedFacilityData.IsPreconsentedRecoveryFacility == null ? NotificationAuditType.Added : NotificationAuditType.Updated,
                    NotificationAuditScreenType.PreConsentedFacility);

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
                await mediator.SendAsync(new GetNotificationBasicInfo(id));

            var response = await mediator.SendAsync(new GetFacilitiesByNotificationId(id));
            var facility = response.Single(p => p.Id == entityId);

            var model = new RemoveFacilityViewModel
            {
                NotificationId = id,
                FacilityId = entityId,
                FacilityName = facility.Business.Name,
                NotificationType = notificationInfo.NotificationType,
                IsOnlySiteOfTreatment = facility.IsActualSiteOfTreatment && response.Count == 1 ? true : false
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
                await mediator.SendAsync(new DeleteFacilityForNotification(model.NotificationId, model.FacilityId));

                await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Deleted,
                    model.NotificationType == NotificationType.Disposal ? NotificationAuditScreenType.DisposalFacilities : NotificationAuditScreenType.RecoveryFacilities);

                if (model.IsOnlySiteOfTreatment)
                {
                    await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Delete,
                    NotificationAuditScreenType.SiteOfExport);
                }

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