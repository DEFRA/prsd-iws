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
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Importer;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class ImporterController : Controller
    {
        private readonly IMediator mediator;
        private readonly ITrimTextService trimTextService;
        private readonly Func<ICompaniesHouseClient> companiesHouseClient;
        private readonly ConfigurationService configurationService;

        public ImporterController(IMediator mediator, ITrimTextService trimTextService,
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
            var importer = await mediator.SendAsync(new GetDraftData<Importer>(id));

            var model = new ImporterViewModel(importer);

            var countries = await mediator.SendAsync(new GetCountries());

            model.Address.Countries = countries;
            model.DefaultUkIfUnselected(countries);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ImporterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Trim address post code
            model.Address.PostalCode = trimTextService.RemoveTextWhiteSpaces(model.Address.PostalCode);

            var importer = new Importer(id)
            {
                Address = model.Address.AsAddress(),
                BusinessName = (string.IsNullOrEmpty(model.Business.OrgTradingName) ? model.Business.Name : (model.Business.Name + " T/A " + model.Business.OrgTradingName)),
                Type = model.BusinessType,
                RegistrationNumber = model.Business.RegistrationNumber,
                Contact = model.Contact.AsContact(),
                IsAddedToAddressBook = model.IsAddedToAddressBook
            };

            await mediator.SendAsync(new SetDraftData<Importer>(id, importer));

            return RedirectToAction("Index", "Producer");
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