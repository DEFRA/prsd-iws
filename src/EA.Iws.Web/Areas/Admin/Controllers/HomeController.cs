namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin.Search;
    using ViewModels;
    using ViewModels.Home;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new BasicSearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(BasicSearchViewModel model)
        {
            var searchResults = await mediator.SendAsync(model.ToRequest());
            var importSearchResults = await mediator.SendAsync(new SearchImportNotifications(model.SearchTerm));

            if (searchResults != null)
            {
                model.ExportSearchResults = searchResults;
            }

            if (importSearchResults != null)
            {
                model.ImportSearchResults = importSearchResults;
            }

            model.HasSearched = true;

            return View(model);
        }
    }
}