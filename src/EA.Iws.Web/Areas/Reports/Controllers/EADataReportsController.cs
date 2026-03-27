namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Requests.Admin.Reports;
    using EA.Iws.Web.Infrastructure;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.EADataReports;

    //[AuthorizeActivity(typeof(GetEADataReport))]
    public class EADataReportsController : Controller
    {
        private readonly IMediator mediator;

        public EADataReportsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: Reports/EADataReports
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

            //var report = await mediator.SendAsync(new GetEADataReport(from, to));

            //var fileName = string.Format("export-stats-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            //return new XlsxActionResult<ExportStatsData>(report, fileName);

            return null;
        }
    }
}