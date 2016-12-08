namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.ImportStats;

    [Authorize(Roles = "internal")]
    public class ImportStatsController : Controller
    {
        private readonly IMediator mediator;

        public ImportStatsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var from = model.From.AsDateTime().Value;
            var to = model.To.AsDateTime().Value;

            var report = await mediator.SendAsync(new GetImportStatsReport(from, to));

            var fileName = string.Format("import-stats-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            return new XlsxActionResult<ImportStatsData>(report, fileName);
        }
    }
}