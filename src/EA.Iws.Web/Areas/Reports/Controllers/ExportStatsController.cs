namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

    [Authorize(Roles = "internal")]
    public class ExportStatsController : Controller
    {
        private readonly IMediator mediator;

        public ExportStatsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Download(int year)
        {
            var report = await mediator.SendAsync(new GetExportStatsReport(year));

            var fileName = string.Format("export-stats-{0}.csv", year);

            return new CsvActionResult<ExportStatsData>(report.ToList(), fileName);
        }
    }
}