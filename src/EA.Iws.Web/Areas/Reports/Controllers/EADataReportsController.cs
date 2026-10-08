namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using ClosedXML.Excel;
    using EA.Iws.Core.Reports;
    using EA.Iws.Requests.Admin.Reports;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Helpers;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
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
            var selectedReports = model.SelectedValues.ToList();

            var reportData = await mediator.SendAsync(new GetEADataReport(fromDate, toDate, selectedReports));

            using (var workbook = new XLWorkbook())
            {
                foreach (var report in selectedReports)
                {
                    switch (report)
                    {
                        case EAReportList.ShipmentReport:
                            AddWorksheet(
                                workbook,
                                EnumHelper.GetDescription(EAReportList.ShipmentReport),
                                reportData.ShipmentReportData);
                            break;

                        case EAReportList.FinanceReport:
                            AddWorksheet(
                                workbook,
                                EnumHelper.GetDescription(EAReportList.FinanceReport),
                                reportData.FinanceReportData);
                            break;

                        case EAReportList.ProducerReport:
                            AddWorksheet(
                                workbook,
                                EnumHelper.GetDescription(EAReportList.ProducerReport),
                                reportData.ProducerReportData);
                            break;

                        case EAReportList.FOIReport:
                            AddWorksheet(
                                workbook,
                                EnumHelper.GetDescription(EAReportList.FOIReport),
                                reportData.FreedomOfInformationReportData);
                            break;

                        case EAReportList.DataExportNotification:
                            AddWorksheet(
                                workbook,
                                EnumHelper.GetDescription(EAReportList.DataExportNotification),
                                reportData.DataExportNotificationData);
                            break;

                        case EAReportList.DataImportNotification:
                            AddWorksheet(
                                workbook,
                                EnumHelper.GetDescription(EAReportList.DataImportNotification),
                                reportData.DataImportNotificationData);
                            break;
                    }
                }

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;

                    var fileName =
                        $"EADataReport-{fromDate:yyyyMMdd}-{toDate:yyyyMMdd}.xlsx";

                    return File(
                        memoryStream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }
        }

        private static void AddWorksheet<T>(XLWorkbook workbook, string sheetName, IEnumerable<T> data)
        {
            var worksheet = workbook.Worksheets.Add(sheetName);

            IList<PropertyInfo> properties = typeof(T).GetProperties();
            AddHeaderRow(properties, worksheet);

            worksheet.Cell(2, 1).Value = data.AsEnumerable();

            var headerRange = worksheet.Range(1, 1, 1, worksheet.LastColumnUsed().ColumnNumber());
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LimeGreen;

            worksheet.SheetView.FreezeRows(1);
            worksheet.Columns().AdjustToContents();
        }

        private static void AddHeaderRow(IList<PropertyInfo> properties, IXLWorksheet worksheet)
        {
            for (var i = 0; i < properties.Count(); i++)
            {
                var property = properties[i];

                var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(property, typeof(DisplayNameAttribute));

                var columnName = attr == null ? SplitCamelCase(property.Name) : attr.DisplayName;

                worksheet.Cell(1, i + 1).Value = columnName;
            }
        }

        private static string SplitCamelCase(string input)
        {
            return Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
        }
    }
}