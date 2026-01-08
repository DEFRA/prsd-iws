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

            if (searchResults != null)
            {
                model.ExportSearchResults = searchResults;
            }

            if (importSearchResults != null)
            {
                model.ImportSearchResults = importSearchResults;
            }

            model.HasSearched = true;

            model.AttentionSummaryTable = await mediator.SendAsync(new GetNotificationAttentionSummary());

            var resultsCount = searchResults?.Count ?? 0 + importSearchResults?.Count ?? 0;

            if (resultsCount == 1 && searchResults.First().NotificationNumber == model.SearchTerm)
            {
              return RedirectToAction(
                  actionName: "Index",
                  controllerName: "Home",
                  routeValues: new { id = searchResults.First().Id, area = "AdminExportAssessment" }
              );
            }

            if (resultsCount == 1 && importSearchResults.First().NotificationNumber == model.SearchTerm)
            {
              return RedirectToAction(
                  actionName: "Index",
                  controllerName: "Home",
                  routeValues: new { id = searchResults.First().Id, area = "ImportNotification" }
              );
            }

      return View(model);
        }
    }
}