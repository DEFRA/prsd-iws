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
            var searchNotificationNumber = string.IsNullOrEmpty(model.SearchTerm) ? string.Empty : model.SearchTerm.ToLower().Replace(" ", string.Empty);

            if (searchResults != null && searchResults.Count() != 0)
            {
                model.ExportSearchResults = searchResults;

                if (searchResults.FirstOrDefault().NotificationNumber.ToLower().Replace(" ", string.Empty).Equals(searchNotificationNumber))
                {
                    return RedirectToAction(
                            actionName: "Index",
                            controllerName: "Home",
                            routeValues: new { id = searchResults.First().Id, area = "AdminExportAssessment" });
                }
            }

            if (importSearchResults != null && importSearchResults.Count() != 0)
            {
                model.ImportSearchResults = importSearchResults;

                if (importSearchResults.FirstOrDefault().NotificationNumber.ToLower().Replace(" ", string.Empty).Equals(searchNotificationNumber))
                {
                    return RedirectToAction(
                            actionName: "Index",
                            controllerName: "Home",
                            routeValues: new { id = importSearchResults.First().Id, area = "ImportNotification" });
                }
            }

            model.HasSearched = true;

            model.AttentionSummaryTable = await mediator.SendAsync(new GetNotificationAttentionSummary());

            return View(model);
        }
    }
}