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
    using ViewModels.Exporter;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class ExporterController : Controller
    {
        private readonly IMediator mediator;

        public ExporterController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var exporter = await mediator.SendAsync(new GetDraftData<Exporter>(id));

            var countries = await mediator.SendAsync(new GetCountries());

            var model = new ExporterViewModel(exporter);

            model.Address.Countries = countries;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ExporterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var exporter = new Exporter(id)
            {
                Address = model.Address.AsAddress(),
                BusinessName = model.Business.Name,
                Contact = model.Contact.AsContact()
            };

            await mediator.SendAsync(new SetDraftData<Exporter>(id, exporter));

            return RedirectToAction("Index", "Importer");
        }
    }
}