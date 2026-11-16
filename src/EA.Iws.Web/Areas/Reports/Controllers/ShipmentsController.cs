namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Core.Reports;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.Shipments;
    using Web.ViewModels.Shared;

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
        public ActionResult Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var from = model.InputParameters.FromDate.AsDateTime().Value;
            var to = model.InputParameters.ToDate.AsDateTime().Value;

            var dateType = ReportEnumParser.TryParse<ShipmentsReportDates>(model.InputParameters.SelectedDate);
            var textFieldType = ReportEnumParser.TryParse<ShipmentReportTextFields>(model.InputParameters.SelectedTextField);
            var operatorType = ReportEnumParser.TryParse<TextFieldOperator>(model.InputParameters.SelectedOperator);

            return RedirectToAction("ColumnSelection", new
            {
                dateType,
                From = model.InputParameters.FromDate.AsDateTime().Value,
                To = model.InputParameters.ToDate.AsDateTime().Value,
                textFieldType,
                operatorType,
                model.InputParameters.TextSearch
            });
        }

        [HttpGet]
        public ActionResult ColumnSelection(
          ShipmentsReportDates dateType,
          DateTime from,
          DateTime to,
          ShipmentReportTextFields? textFieldType,
          TextFieldOperator? operatorType,
          string textSearch)
        {
            var foiOutputColumns = CheckBoxCollectionViewModel.CreateFromEnum<ShipmentReportOutputColumns>();

            var model = new ColumnSelectionViewModel(dateType, from, to, textFieldType, operatorType, textSearch, foiOutputColumns);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ColumnSelection(ColumnSelectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var columnsToHide = model.ShipmentReportColumns.GetColumnsToHide();
            var dateType = ReportEnumParser.TryParse<ShipmentsReportDates>(model.ShipmentReportColumns.DateType);
            var textFieldType = ReportEnumParser.TryParse<ShipmentReportTextFields>(model.ShipmentReportColumns.TextFieldType);
            var operatorType = ReportEnumParser.TryParse<TextFieldOperator>(model.ShipmentReportColumns.OperatorType);

            return RedirectToAction("Download", new
            {
                dateType,
                From = model.ShipmentReportColumns.FromDate,
                To = model.ShipmentReportColumns.ToDate,
                textFieldType,
                operatorType,
                model.ShipmentReportColumns.SearchText,
                columnsToHide
            });
        }

        [HttpGet]
        public async Task<ActionResult> Download(ShipmentsReportDates dateType,
             DateTime from,
             DateTime to,
             ShipmentReportTextFields? textFieldType,
             TextFieldOperator? operatorType,
             string searchText, string columnsToHide)
        {
            var report =
               await
                   mediator.SendAsync(new GetShipmentsReport(from, to,
                      dateType, textFieldType, operatorType, searchText));

            var fileName = string.Format("shipments-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            return new XlsxActionResult<ShipmentData>(report, fileName, true, columnsToHide);
        }
    }
}