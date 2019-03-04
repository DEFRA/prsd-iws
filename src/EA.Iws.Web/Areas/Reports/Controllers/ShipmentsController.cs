namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.Shipments;

    [AuthorizeActivity(typeof(GetShipmentsReport))]
    public class ShipmentsController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentsController(IMediator mediator)
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

            var report =
                await
                    mediator.SendAsync(new GetShipmentsReport(model.From, model.To, model.DateType.Value,
                        model.TextFieldType, model.OperatorType, model.TextSearch));

            var fileName = string.Format("shipments-{0}-{1}.xlsx", model.From.ToShortDateString(), model.To.ToShortDateString());

            return new XlsxActionResult<ShipmentData>(report, fileName);
        }
    }
}