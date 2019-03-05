namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Core.Reports;
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

            var from = model.InputParameters.FromDate.AsDateTime().Value;
            var to = model.InputParameters.ToDate.AsDateTime().Value;

            var dateType = model.InputParameters.TryParse<ShipmentsReportDates>(model.InputParameters.SelectedDate);
            var textFieldType = model.InputParameters.TryParse<ShipmentReportTextFields>(model.InputParameters.SelectedTextField);
            var operatorType = model.InputParameters.TryParse<TextFieldOperator>(model.InputParameters.SelectedOperator);

            var report =
                await
                    mediator.SendAsync(new GetShipmentsReport(from, to,
                        dateType.Value, textFieldType, operatorType, model.InputParameters.TextSearch));

            var fileName = string.Format("shipments-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            return new XlsxActionResult<ShipmentData>(report, fileName, true);
        }
    }
}