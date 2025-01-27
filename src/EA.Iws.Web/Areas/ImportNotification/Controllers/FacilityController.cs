namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using Core.ImportNotification.Draft;
    using EA.Iws.Api.Client.CompaniesHouseAPI;
    using EA.Iws.Api.Client.Models;
    using EA.Iws.Web.Areas.Common;
    using EA.Iws.Web.Services;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Facility;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class FacilityController : Controller
    {
        private readonly IMediator mediator;
        private readonly ITrimTextService trimTextService;
        private readonly Func<ICompaniesHouseClient> companiesHouseClient;
        private readonly ConfigurationService configurationService;

        public FacilityController(IMediator mediator, ITrimTextService trimTextService,
                                  Func<ICompaniesHouseClient> companiesHouseClient,
                                  ConfigurationService configurationService)
        {
            this.mediator = mediator;
            this.trimTextService = trimTextService;
            this.companiesHouseClient = companiesHouseClient;
            this.configurationService = configurationService;
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
                var details = await mediator.SendAsync(new GetNotificationDetails(id));
                model.NotificationType = details.NotificationType;

                return View(model);
            }

            //Trim address post code
            model.Address.PostalCode = trimTextService.RemoveTextWhiteSpaces(model.Address.PostalCode);

            var facilityCollection = await mediator.SendAsync(new GetDraftData<FacilityCollection>(id));

            var newFacility = new Facility(id)
            {
                Address = model.Address.AsAddress(),
                BusinessName = (string.IsNullOrEmpty(model.Business.OrgTradingName) ? model.Business.Name : (model.Business.Name + " T/A " + model.Business.OrgTradingName)),
                Contact = model.Contact.AsContact(),
                RegistrationNumber = model.Business.RegistrationNumber,
                Type = model.BusinessType,
                Id = Guid.NewGuid(),
                IsActualSite = model.IsActualSite,
                IsAddedToAddressBook = model.IsAddedToAddressBook,
                AdditionalRegistrationNumber = model.Business.AdditionalRegistrationNumber
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

            //Trim address post code
            model.Address.PostalCode = trimTextService.RemoveTextWhiteSpaces(model.Address.PostalCode);

            var facilityCollection = await mediator.SendAsync(new GetDraftData<FacilityCollection>(id));
            var facilityToEdit = facilityCollection.Facilities.SingleOrDefault(f => f.Id == model.FacilityId);

            if (facilityToEdit != null)
            {
                facilityCollection.Facilities.Remove(facilityToEdit);

                var newFacility = new Facility(id)
                {
                    Address = model.Address.AsAddress(),
                    BusinessName = (string.IsNullOrEmpty(model.Business.OrgTradingName) ? model.Business.Name : (model.Business.Name + " T/A " + model.Business.OrgTradingName)),
                    Contact = model.Contact.AsContact(),
                    RegistrationNumber = model.Business.RegistrationNumber,
                    Type = model.BusinessType,
                    Id = model.FacilityId,
                    IsActualSite = model.IsActualSite,
                    IsAddedToAddressBook = model.IsAddedToAddressBook,
                    AdditionalRegistrationNumber = model.Business.AdditionalRegistrationNumber
                };

                facilityCollection.Facilities.Add(newFacility);

                await mediator.SendAsync(new SetDraftData<FacilityCollection>(id, facilityCollection));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetCompanyName(string registrationNumber)
        {
            var result = await GetDefraCompanyDetails(registrationNumber);

            if (result.Error)
            {
                return Json(new { success = false, errorMsg = "Service is unavailable, please contatct system administator." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, companyName = result.Organisation?.Name }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DefraCompaniesHouseApiModel> GetDefraCompanyDetails(string companyRegistrationNumber)
        {
            using (var client = companiesHouseClient())
            {
                return await client.GetCompanyDetailsAsync(configurationService.CurrentConfiguration.CompaniesHouseReferencePath, companyRegistrationNumber);
            }
        }
    }
}