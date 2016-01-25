namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using ViewModels.Facility;

    [Authorize(Roles = "internal")]
    public class FacilityController : Controller
    {
        private readonly IMediator mediator;

        public FacilityController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var facilityCollection = await mediator.SendAsync(new GetDraftData<FacilityCollection>(id));
            var details = await mediator.SendAsync(new GetNotificationDetails(id));

            var model = new MultipleFacilitiesViewModel
            {
                NotificationId = details.ImportNotificationId,
                NotificationType = details.NotificationType,
                Facilities = facilityCollection.Facilities
            };
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, MultipleFacilitiesViewModel model)
        {
            var facilityCollection = await mediator.SendAsync(new GetDraftData<FacilityCollection>(id));

            foreach (var f in facilityCollection.Facilities)
            {
                f.IsActualSite = f.Id == model.SelectedSiteOfTreatment;
            }

            await mediator.SendAsync(new SetDraftData<FacilityCollection>(id, facilityCollection));

            return RedirectToAction("Index", "Shipment");
        }
            
        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var model = new FacilityViewModel
            {
                Address = { Countries = await mediator.SendAsync(new GetCountries()) }
            };

            model.DefaultUkIfUnselected(model.Address.Countries);

            var details = await mediator.SendAsync(new GetNotificationDetails(id));
            model.NotificationType = details.NotificationType;
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Guid id, FacilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var facilityCollection = await mediator.SendAsync(new GetDraftData<FacilityCollection>(id));

            var newFacility = new Facility(id)
            {
                Address = model.Address.AsAddress(),
                BusinessName = model.BusinessName,
                Contact = model.Contact.AsContact(),
                RegistrationNumber = model.RegistrationNumber,
                Type = model.Type,
                Id = Guid.NewGuid(),
                IsActualSite = model.IsActualSite
            };

            facilityCollection.Facilities.Add(newFacility);

            await mediator.SendAsync(new SetDraftData<FacilityCollection>(id, facilityCollection));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Remove(Guid id, Guid? facilityId)
        {
            var facilityCollection = await mediator.SendAsync(new GetDraftData<FacilityCollection>(id));
            var facilityToRemove = facilityCollection.Facilities.SingleOrDefault(f => f.Id == facilityId.GetValueOrDefault());

            if (facilityToRemove != null)
            {
                facilityCollection.Facilities.Remove(facilityToRemove);
            }

            await mediator.SendAsync(new SetDraftData<FacilityCollection>(id, facilityCollection));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid? facilityId)
        {
            var facilityCollection = await mediator.SendAsync(new GetDraftData<FacilityCollection>(id));
            var facilityToEdit = facilityCollection.Facilities.SingleOrDefault(f => f.Id == facilityId.GetValueOrDefault());
            
            if (facilityToEdit == null)
            {
                return RedirectToAction("index");
            }

            var model = new FacilityViewModel(facilityToEdit);
            model.Address.Countries = await mediator.SendAsync(new GetCountries());
            model.DefaultUkIfUnselected(model.Address.Countries);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, FacilityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var facilityCollection = await mediator.SendAsync(new GetDraftData<FacilityCollection>(id));
            var facilityToEdit = facilityCollection.Facilities.SingleOrDefault(f => f.Id == model.FacilityId);

            if (facilityToEdit != null)
            {
                facilityCollection.Facilities.Remove(facilityToEdit);

                var newFacility = new Facility(id)
                {
                    Address = model.Address.AsAddress(),
                    BusinessName = model.BusinessName,
                    Contact = model.Contact.AsContact(),
                    RegistrationNumber = model.RegistrationNumber,
                    Type = model.Type,
                    Id = model.FacilityId,
                    IsActualSite = model.IsActualSite
                };

                facilityCollection.Facilities.Add(newFacility);

                await mediator.SendAsync(new SetDraftData<FacilityCollection>(id, facilityCollection));
            }

            return RedirectToAction("Index");
        }
    }
}