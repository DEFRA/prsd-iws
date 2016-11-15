namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Search;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using Requests.Admin.Search;
    using ViewModels.AdvancedSearch;

    [Authorize(Roles = "internal")]
    public class AdvancedSearchController : Controller
    {
        private readonly IMediator mediator;

        public AdvancedSearchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new IndexViewModel();
            model.Areas = await GetAreas();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Areas = await GetAreas();

                return View(model);
            }

            var criteria = new AdvancedSearchCriteria
            {
                ConsentValidFromStart = model.ConsentValidFromStart.AsDateTime(),
                ConsentValidFromEnd = model.ConsentValidFromEnd.AsDateTime(),
                ConsentValidToStart = model.ConsentValidToStart.AsDateTime(),
                ConsentValidToEnd = model.ConsentValidToEnd.AsDateTime(),
                EwcCode = model.EwcCode,
                ImportCountryName = model.ImportCountryName,
                ImporterName = model.ImporterName,
                LocalAreaId = model.LocalAreaId,
                ProducerName = model.ProducerName,
                BaselOecdCode = model.BaselOecdCode,
                EntryPointName = model.EntryPointName,
                ExitPointName = model.ExitPointName,
                ExporterName = model.ExporterName,
                FacilityName = model.FacilityName,
                NotificationReceivedStart = model.NotificationReceivedStart.AsDateTime(),
                NotificationReceivedEnd = model.NotificationReceivedEnd.AsDateTime(),
                ImportNotificationStatus = model.GetImportNotificationStatus(),
                NotificationStatus = model.GetExportNotificationStatus(),
                TradeDirection = model.SelectedTradeDirection,
                NotificationType = model.SelectedNotificationType,
                OperationCodes = model.SelectedOperationCodes,
                IsInterim = model.IsInterim,
                ExportCountryName = model.ExportCountryName
            };

            return RedirectToAction("Results", criteria.ToRouteValueDictionary());
        }

        [HttpGet]
        public async Task<ActionResult> Results(AdvancedSearchCriteria criteria)
        {
            var results = await mediator.SendAsync(new NotificaitonsAdvancedSearch(criteria));

            var model = new ResultsViewModel
            {
                ExportResults = results.ExportResults.ToArray(),
                ImportResults = results.ImportResults.ToArray()
            };

            return View(model);
        }

        private async Task<SelectList> GetAreas()
        {
            var result = await mediator.SendAsync(new GetLocalAreas());
            return
                new SelectList(
                    result.Select(area => new SelectListItem { Text = area.Name, Value = area.Id.ToString() }),
                    "Value", "Text");
        }
    }
}