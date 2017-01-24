namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.MissingShipments;

    [AuthorizeActivity(typeof(GetMissingShipmentsReport))]
    public class MissingShipmentsController : Controller
    {
        private readonly IMediator mediator;

        public MissingShipmentsController(IMediator mediator)
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

            var from = model.From.AsDateTime().GetValueOrDefault();
            var to = model.To.AsDateTime().GetValueOrDefault();

            var report = await mediator.SendAsync(new GetMissingShipmentsReport(from, to, model.DateType));

            var fileName = string.Format("missing-shipments-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            return new XlsxActionResult<MissingShipmentData>(report, fileName);
        }
    }
}