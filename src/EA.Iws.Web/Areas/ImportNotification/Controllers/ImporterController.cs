namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using ViewModels.Importer;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class ImporterController : Controller
    {
        private readonly IMediator mediator;

        public ImporterController(IMediator mediator)
        {
            this.mediator = mediator;
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

            var importer = new Importer(id)
            {
                Address = model.Address.AsAddress(),
                BusinessName = model.Business.Name,
                Type = model.BusinessType,
                RegistrationNumber = model.Business.RegistrationNumber,
                Contact = model.Contact.AsContact(),
                IsAddedToAddressBook = model.IsAddedToAddressBook
            };

            await mediator.SendAsync(new SetDraftData<Importer>(id, importer));

            return RedirectToAction("Index", "Producer");
        } 
    }
}