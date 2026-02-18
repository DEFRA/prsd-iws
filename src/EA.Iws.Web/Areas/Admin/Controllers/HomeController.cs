namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.Search;
    using Requests.NotificationAssessment;
  using System.Linq;
  using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Home;

    [AuthorizeActivity(typeof(GetNotificationAttentionSummary))]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new BasicSearchViewModel
            {
                AttentionSummaryTable = await mediator.SendAsync(new GetNotificationAttentionSummary())
            };
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(BasicSearchViewModel model)
        {
            var searchResults = await mediator.SendAsync(model.ToRequest());
            var importSearchResults = await mediator.SendAsync(new SearchImportNotifications(model.SearchTerm));

            bool exportMatch = false;
            bool importMatch = false;

            if (searchResults != null && searchResults.Count() != 0)
            {
                model.ExportSearchResults = searchResults;
                exportMatch = searchResults.First().NotificationNumber.ToLower().Replace(" ", string.Empty)
                == model.SearchTerm.ToLower().Replace(" ", string.Empty);
            }

            if (importSearchResults != null && importSearchResults.Count() != 0)
            {
                model.ImportSearchResults = importSearchResults;
                importMatch = importSearchResults.First().NotificationNumber.ToLower().Replace(" ", string.Empty)
                == model.SearchTerm.ToLower().Replace(" ", string.Empty);
            }

            model.HasSearched = true;

            model.AttentionSummaryTable = await mediator.SendAsync(new GetNotificationAttentionSummary());

            if (model.ExportSearchResults != null && model.ExportSearchResults.Count == 1 && exportMatch)
            {
              return RedirectToAction(
                  actionName: "Index",
                  controllerName: "Home",
                  routeValues: new { id = searchResults.First().Id, area = "AdminExportAssessment" });
            }

            if (model.ImportSearchResults != null && model.ImportSearchResults.Count == 1 && importMatch)
            {
              return RedirectToAction(
                  actionName: "Index",
                  controllerName: "Home",
                  routeValues: new { id = importSearchResults.First().Id, area = "ImportNotification" });
            }

      return View(model);
        }
    }
}