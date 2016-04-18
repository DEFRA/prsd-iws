namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;

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
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Download(DateTime endDate)
        {
            var report = await mediator.SendAsync(new GetFinanceReport(endDate));

            var fileName = "finance-report.csv";

            return new CsvActionResult<FinanceReportData>(report.ToList(), fileName);
        }
    }
}