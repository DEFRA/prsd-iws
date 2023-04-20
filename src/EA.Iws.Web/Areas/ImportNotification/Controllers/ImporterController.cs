namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using EA.Iws.Web.Areas.Common;
    using EA.Iws.Web.Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using ViewModels.Importer;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class ImporterController : Controller
    {
        private readonly IMediator mediator;
        private readonly ITrimTextMethod trimTextMethod;

        public ImporterController(IMediator mediator, ITrimTextMethod trimTextMethod)
        {
            this.mediator = mediator;
            this.trimTextMethod = trimTextMethod;
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
            model.Address.PostalCode = trimTextMethod.RemoveTextWhiteSpaces(model.Address.PostalCode);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCompanyName(string registrationNumber)
        {
            if (!this.Request.IsAjaxRequest())
            {
                throw new InvalidOperationException();
            }

            try
            {
                string orgName = DefraCompaniesHouseApi.GetOrganisationNameByRegNum(registrationNumber);
                return Json(new { success = true, companyName = orgName });
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    return Json(new { success = false, errorMsg = "Please enter valid company registration number and try again." });
                }
                else if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    return Json(new { success = false, errorMsg = "Service is unavailable, please contatct system administator." });
                }
                else
                {
                    return Json(new { success = false, errorMsg = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMsg = ex.Message });
            }
        }
    }
}