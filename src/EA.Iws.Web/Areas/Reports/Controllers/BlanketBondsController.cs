namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.BlanketBonds;

    [AuthorizeActivity(typeof(GetBlanketBondsReport))]
    public class BlanketBondsController : Controller
    {
        private readonly IMediator mediator;

        public BlanketBondsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var report = await mediator.SendAsync(new GetBlanketBondsReport(
                model.FinancialGuaranteeReferenceNumber,
                model.ExporterName,
                model.ImporterName,
                model.ProducerName));

            var filename = string.Format("blanket-bonds-{0}.xlsx", SystemTime.UtcNow.ToShortDateString());

            return new XlsxActionResult<BlanketBondsData>(report, filename);
        }
    }
}