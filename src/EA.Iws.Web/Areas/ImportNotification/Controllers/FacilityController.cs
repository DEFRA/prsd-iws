namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Collections.Generic;
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

            var countries = await mediator.SendAsync(new GetCountries());
            var details = await mediator.SendAsync(new GetNotificationDetails(id));

            ViewBag.Type = details.NotificationType;

            return View(facilityCollection.Facilities.Select(f =>
            {
                var model = new FacilityViewModel(f);
                model.Address.Countries = countries;
                model.NotificationType = details.NotificationType;
                return model;
            }).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, List<FacilityViewModel> model)
        {
            if (model == null)
            {
                model = new List<FacilityViewModel>();
            }

            if (!ModelState.IsValid)
            {
                await BindCountries(model);
                await BindNotificationType(id, model);

                return View(model);
            }

            var facilityCollection = new FacilityCollection
            {
                Facilities = model.Select(f => new Facility
                {
                    Address = f.Address.AsAddress(),
                    BusinessName = f.BusinessName,
                    Contact = f.Contact.AsContact(),
                    RegistrationNumber = f.RegistrationNumber,
                    Id = f.Id,
                    Type = f.Type,
                    IsActualSite = f.IsActualSite
                }).ToList()
            };

            await mediator.SendAsync(new SetDraftData<FacilityCollection>(id, facilityCollection));

            return RedirectToAction("Index", "WasteType");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Guid id, List<FacilityViewModel> model)
        {
            ClearModelState();

            if (model == null)
            {
                model = new List<FacilityViewModel>();
            }

            model.Add(new FacilityViewModel());

            await BindCountries(model);

            await BindNotificationType(id, model);

            return PartialView("_FacilityTable", model);
        }

        private async Task BindNotificationType(Guid id, List<FacilityViewModel> model)
        {
            var details = await mediator.SendAsync(new GetNotificationDetails(id));

            ViewBag.Type = details.NotificationType;

            if (model == null)
            {
                return;
            }

            foreach (var facilityViewModel in model)
            {
                facilityViewModel.NotificationType = details.NotificationType;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, List<FacilityViewModel> model, Guid? deleteId)
        {
            ClearModelState();

            if (model == null)
            {
                model = new List<FacilityViewModel>();
            }

            var facility = model.SingleOrDefault(f => f.Id == deleteId.GetValueOrDefault());

            if (facility != null)
            {
                model.Remove(facility);
            }

            await BindCountries(model);

            await BindNotificationType(id, model);

            return PartialView("_FacilityTable", model);
        }

        private async Task BindCountries(List<FacilityViewModel> models)
        {
            var countries = await mediator.SendAsync(new GetCountries());

            foreach (var model in models)
            {
                model.Address.Countries = countries;
                model.DefaultUkIfUnselected(countries);
            }
        }

        private void ClearModelState()
        {
            var keys = ModelState.Keys.ToArray();
            foreach (var key in keys)
            {
                ModelState.Remove(key);
            }
        }
    }
}