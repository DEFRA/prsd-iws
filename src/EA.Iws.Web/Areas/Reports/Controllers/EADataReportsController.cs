namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using EA.Iws.Requests.Admin.Reports;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Mediator;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.EADataReports;

    [AuthorizeActivity(typeof(GetEADataReport))]
    public class EADataReportsController : Controller
    {
        private readonly IMediator mediator;

        public EADataReportsController(IMediator mediator)
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

            var fromDate = model.From.AsDateTime().Value;
            var toDate = model.To.AsDateTime().Value;
            var reportData = await mediator.SendAsync(new GetEADataReport(fromDate, toDate, model.SelectedValues.ToList()));

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                foreach (var filename in model.SelectedValues)
                {
                    if (filename == Core.Reports.EAReportList.ShipmentReport)
                    {
                        var sheet = package.Workbook.Worksheets.Add("Shipment Report");
                        sheet.Cells["A1"].LoadFromCollection(reportData.ShipmentReportData, true);
                        sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                    }

                    if (filename == Core.Reports.EAReportList.FinanceReport)
                    {
                        var sheet = package.Workbook.Worksheets.Add("Finance Report");
                        sheet.Cells["A1"].LoadFromCollection(reportData.FinanceReportData, true);
                        sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                    }

                    if (filename == Core.Reports.EAReportList.ProducerReport)
                    {
                        var sheet = package.Workbook.Worksheets.Add("Producer Report");
                        sheet.Cells["A1"].LoadFromCollection(reportData.ProducerReportData, true);
                        sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                    }

                    if (filename == Core.Reports.EAReportList.FOIReport)
                    {
                        var sheet = package.Workbook.Worksheets.Add("FOI Report");
                        sheet.Cells["A1"].LoadFromCollection(reportData.FreedomOfInformationReportData, true);
                        sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                    }
                }
                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EADataReports.xlsx");
            }
        }
    }
}