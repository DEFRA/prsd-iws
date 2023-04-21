namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using EA.Iws.Web.Areas.Common;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using ViewModels.Producer;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class ProducerController : Controller
    {
        private readonly IMediator mediator;
        private readonly ITrimTextService trimTextService;

        public ProducerController(IMediator mediator, ITrimTextService trimTextService)
        {
            this.mediator = mediator;
            this.trimTextService = trimTextService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var producer = await mediator.SendAsync(new GetDraftData<Producer>(id));

            var model = new ProducerViewModel(producer);

            var countries = await mediator.SendAsync(new GetCountries());

            model.Address.Countries = countries;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ProducerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Trim address post code
            model.Address.PostalCode = trimTextService.RemoveTextWhiteSpaces(model.Address.PostalCode);

            var producer = new Producer(id)
            {
                Address = model.Address.AsAddress(),
                AreMultiple = model.AreMultiple,
                BusinessName = (string.IsNullOrEmpty(model.Business.OrgTradingName) ? model.Business.Name : (model.Business.Name + " T/A " + model.Business.OrgTradingName)),
                Contact = model.Contact.AsContact(),
                IsAddedToAddressBook = model.IsAddedToAddressBook,
                Type = model.BusinessType,
                RegistrationNumber = model.Business.RegistrationNumber
            };

            await mediator.SendAsync(new SetDraftData<Producer>(id, producer));

            return RedirectToAction("Index", "Facility");
        }
    }
}