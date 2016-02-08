namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Admin.EntryOrExitPoints;
    using Requests.Shared;
    using ViewModels.EntryOrExitPoint;

    [AuthorizeActivity(typeof(AddEntryOrExitPoint))]
    public class EntryOrExitPointController : Controller
    {
        private readonly IMediator mediator;

        public EntryOrExitPointController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await mediator.SendAsync(new GetEntryOrExitPointsGroupedByCountry());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection formCollection)
        {
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            var countries = await mediator.SendAsync(new GetCountries());

            var model = new AddEntryOrExitPointViewModel
            {
                Countries = countries
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddEntryOrExitPointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await mediator.SendAsync(new CheckEntryOrExitPointUnique(model.CountryId.Value, model.Name)))
            {
                ModelState.AddModelError("Name", EntryOrExitPointControllerResources.NameNotUniqueMessage);

                return View(model);
            }

            await mediator.SendAsync(new AddEntryOrExitPoint(model.CountryId.Value, model.Name));

            var countryName = model.Countries.SingleOrDefault(c => c.Id == model.CountryId.Value).Name;

            var successModel = new SuccessViewModel
            {
                PortName = model.Name,
                CountryName = countryName
            };

            return View("Success", successModel);
        }

        [HttpGet]
        public ActionResult Success(SuccessViewModel model)
        {
            return View(model);
        }
    }
}