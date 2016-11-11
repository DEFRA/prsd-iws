namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.Finance;

    [Authorize(Roles = "internal")]
    public class FinanceController : Controller
    {
        private readonly IMediator mediator;

        public FinanceController(IMediator mediator)
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

            var report = await mediator.SendAsync(new GetFinanceReport(to));

            var fileName = "finance-report.csv";

            return new CsvActionResult<FinanceReportData>(report.ToList(), fileName);
        }
    }
}