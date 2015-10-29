namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin.EntryOrExitPoints;
    using Requests.Shared;
    using ViewModels.EntryOrExitPoint;

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
        public ActionResult Add(AddEntryOrExitPointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            throw new IOException();
        }
    }
}