namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.Worklist;

    [AuthorizeActivity(typeof(GetExportWorklist))]
    public class WorklistController : Controller
    {
        private readonly IMediator mediator;

        public WorklistController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(ExportWorklistFilterViewModel filter, int page = 1)
        {
            var model = new WorklistViewModel
            {
                ExportsFilter = filter ?? new ExportWorklistFilterViewModel(),
                ExportsResult = await mediator.SendAsync(new GetExportWorklist
                {
                    NotificationNumber = filter?.NotificationNumber,
                    Officer = filter?.Officer,
                    Statuses = filter?.SelectedStatuses,
                    PageNumber = page
                })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(WorklistViewModel model)
        {
            var routeValues = new RouteValueDictionary
            {
                { "page", 1 }
            };

            if (!string.IsNullOrWhiteSpace(model.ExportsFilter.NotificationNumber))
            {
                routeValues.Add("filter.NotificationNumber", model.ExportsFilter.NotificationNumber);
            }

            if (!string.IsNullOrWhiteSpace(model.ExportsFilter.Officer))
            {
                routeValues.Add("filter.Officer", model.ExportsFilter.Officer);
            }

            if (model.ExportsFilter.SelectedStatuses != null && model.ExportsFilter.SelectedStatuses.Length > 0)
            {
                for (int i = 0; i < model.ExportsFilter.SelectedStatuses.Length; i++)
                {
                    routeValues.Add(string.Format("filter.SelectedStatuses[{0}]", i), (int)model.ExportsFilter.SelectedStatuses[i]);
                }
            }

            return RedirectToAction("Index", routeValues);
        }
    }
}