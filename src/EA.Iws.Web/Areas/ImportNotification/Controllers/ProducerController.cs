namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using ViewModels.Producer;

    [Authorize(Roles = "internal")]
    public class ProducerController : Controller
    {
        private readonly IMediator mediator;

        public ProducerController(IMediator mediator)
        {
            this.mediator = mediator;
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

            var producer = new Producer(id)
            {
                Address = model.Address.AsAddress(),
                AreMultiple = model.AreMultiple,
                BusinessName = model.BusinessName,
                Contact = model.Contact.AsContact()
            };

            await mediator.SendAsync(new SetDraftData<Producer>(id, producer));

            return RedirectToAction("Index", "Facility");
        } 
    }
}