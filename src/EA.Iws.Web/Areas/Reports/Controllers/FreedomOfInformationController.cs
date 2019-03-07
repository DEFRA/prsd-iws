namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Core.Reports;
    using Core.Reports.FOI;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.FreedomOfInformation;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(GetFreedomOfInformationReport))]
    public class FreedomOfInformationController : Controller
    {
        private readonly IMediator mediator;

        public FreedomOfInformationController(IMediator mediator)
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

            var dateType = ReportEnumParser.TryParse<FOIReportDates>(model.InputParameters.SelectedDate);
            var textFieldType = ReportEnumParser.TryParse<FOIReportTextFields>(model.InputParameters.SelectedTextField);
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
            FOIReportDates dateType,
            DateTime from,
            DateTime to,
            FOIReportTextFields? textFieldType,
            TextFieldOperator? operatorType,
            string textSearch)
        {
            var foiOutputColumns = CheckBoxCollectionViewModel.CreateFromEnum<FOIOutputColumns>();

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

            var columnsToHide = model.FOIReportColumns.GetColumnsToHide();
            var dateType = ReportEnumParser.TryParse<FOIReportDates>(model.FOIReportColumns.DateType);
            var textFieldType = ReportEnumParser.TryParse<FOIReportTextFields>(model.FOIReportColumns.TextFieldType);
            var operatorType = ReportEnumParser.TryParse<TextFieldOperator>(model.FOIReportColumns.OperatorType);

            return RedirectToAction("Download", new
            {
                dateType,
                From = model.FOIReportColumns.FromDate,
                To = model.FOIReportColumns.ToDate,
                textFieldType,
                operatorType,
                model.FOIReportColumns.SearchText,
                columnsToHide
            });
        }

        [HttpGet]
        public async Task<ActionResult> Download(FOIReportDates dateType,
             DateTime from,
             DateTime to,
             FOIReportTextFields? textFieldType,
             TextFieldOperator? operatorType,
             string searchText, string columnsToHide)
        {
            var report = await mediator.SendAsync(new GetFreedomOfInformationReport(from, to, dateType, textFieldType, operatorType, searchText));

            var fileName = string.Format("foi-report-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            return new XlsxActionResult<FreedomOfInformationData>(report, fileName, true, columnsToHide);
        }       
    }
}