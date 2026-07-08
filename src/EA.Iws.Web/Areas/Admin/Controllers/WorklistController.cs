namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Core.ImportNotificationAssessment;
    using Core.NotificationAssessment;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
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
        public async Task<ActionResult> Index(
            ExportWorklistFilterViewModel exportFilter, 
            ImportWorklistFilterViewModel importFilter, 
            int page = 1, 
            string tab = "export")
        {
            var model = new WorklistViewModel();

            // Define allowed statuses for exports
            model.ExportStatuses = new[]
            {
                NotificationStatus.NotificationReceived,
                NotificationStatus.InAssessment,
                NotificationStatus.ReadyToTransmit,
                NotificationStatus.Transmitted,
                NotificationStatus.DecisionRequiredBy,
                NotificationStatus.Withdrawn,
                NotificationStatus.Objected,
                NotificationStatus.Consented
            };

            // Define allowed statuses for imports
            model.ImportStatuses = new[]
            {
                ImportNotificationStatus.NotificationReceived,
                ImportNotificationStatus.AwaitingAssessment,
                ImportNotificationStatus.InAssessment,
                ImportNotificationStatus.ReadyToAcknowledge,
                ImportNotificationStatus.DecisionRequiredBy,
                ImportNotificationStatus.Consented,
                ImportNotificationStatus.Objected,
                ImportNotificationStatus.ConsentWithdrawn,
                ImportNotificationStatus.Withdrawn
            };

            if (tab == "import")
            {
                model.ImportFilter = importFilter ?? new ImportWorklistFilterViewModel();
                model.ImportResult = await mediator.SendAsync(new GetImportWorklist
                {
                    NotificationNumber = importFilter?.NotificationNumber,
                    Officer = importFilter?.Officer,
                    Statuses = importFilter?.SelectedStatuses,
                    PageNumber = page
                });
            }
            else
            {
                model.ExportFilter = exportFilter ?? new ExportWorklistFilterViewModel();
                model.ExportResult = await mediator.SendAsync(new GetExportWorklist
                {
                    NotificationNumber = exportFilter?.NotificationNumber,
                    Officer = exportFilter?.Officer,
                    Statuses = exportFilter?.SelectedStatuses,
                    PageNumber = page
                });
            }

            ViewBag.CurrentTab = tab;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(WorklistViewModel model, string tab = "export")
        {
            var routeValues = new RouteValueDictionary
            {
                { "tab", tab },
                { "page", 1 }
            };

            if (tab == "import")
            {
                if (!string.IsNullOrWhiteSpace(model.ImportFilter.NotificationNumber))
                {
                    routeValues.Add("importFilter.NotificationNumber", model.ImportFilter.NotificationNumber);
                }

                if (!string.IsNullOrWhiteSpace(model.ImportFilter.Officer))
                {
                    routeValues.Add("importFilter.Officer", model.ImportFilter.Officer);
                }

                if (model.ImportFilter.SelectedStatuses != null && model.ImportFilter.SelectedStatuses.Length > 0)
                {
                    for (int i = 0; i < model.ImportFilter.SelectedStatuses.Length; i++)
                    {
                        routeValues.Add(string.Format("importFilter.SelectedStatuses[{0}]", i), (int)model.ImportFilter.SelectedStatuses[i]);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.ExportFilter.NotificationNumber))
                {
                    routeValues.Add("exportFilter.NotificationNumber", model.ExportFilter.NotificationNumber);
                }

                if (!string.IsNullOrWhiteSpace(model.ExportFilter.Officer))
                {
                    routeValues.Add("exportFilter.Officer", model.ExportFilter.Officer);
                }

                if (model.ExportFilter.SelectedStatuses != null && model.ExportFilter.SelectedStatuses.Length > 0)
                {
                    for (int i = 0; i < model.ExportFilter.SelectedStatuses.Length; i++)
                    {
                        routeValues.Add(string.Format("exportFilter.SelectedStatuses[{0}]", i), (int)model.ExportFilter.SelectedStatuses[i]);
                    }
                }
            }

            return RedirectToAction("Index", routeValues);
        }
    }
}