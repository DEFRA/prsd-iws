namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.Shared;
    using ViewModels.Exporter;

    [Authorize(Roles = "internal")]
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
            var exporter = new Exporter
            {
                AddressLine1 = model.Address.AddressLine1,
                AddressLine2 = model.Address.AddressLine2,
                Telephone = model.Contact.Telephone,
                CountryId = model.Address.CountryId,
                BusinessName = model.BusinessName,
                TownOrCity = model.Address.TownOrCity,
                PostalCode = model.Address.PostalCode,
                Email = model.Contact.Email,
                ContactName = model.Contact.Name
            };

            await mediator.SendAsync(new SetDraftData<Exporter>(id, exporter));

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}