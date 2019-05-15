namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Core.Reports;
    using Core.Reports.Compliance;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.Reports;
    using ViewModels.Compliance;

    [AuthorizeActivity(typeof(GetComplianceReport))]
    public class ComplianceController : Controller
    {
        private readonly IMediator mediator;

        public ComplianceController(IMediator mediator)
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

            var dateType = ReportEnumParser.TryParse<ComplianceReportDates>(model.InputParameters.SelectedDate);
            var textFieldType = ReportEnumParser.TryParse<ComplianceTextFields>(model.InputParameters.SelectedTextField);
            var operatorType = ReportEnumParser.TryParse<TextFieldOperator>(model.InputParameters.SelectedOperator);

            return RedirectToAction("Download", new
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
        public async Task<ActionResult> Download(ComplianceReportDates dateType,
           DateTime from,
           DateTime to,
           ComplianceTextFields? textFieldType,
           TextFieldOperator? operatorType,
           string textSearch)
        {
            var report =
                await
                    mediator.SendAsync(new GetComplianceReport(dateType, from, to, textFieldType, operatorType, textSearch));

            var fileName = string.Format("compliance-report-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            var guidance = new ComplianceDataGuidance
            {
                NotificationNumber = "This shows the relevant notification number",
                NoPrenotificationCount = "This shows the total number of shipments recorded in the service that haven't met prenotifcation rules",
                PreNotificationColour = "The colour indicates the level of compliance based on data shown in the previous cell. 0 = GREEN, 1 to 10 = AMBER and 11 or more = RED",
                MissingShipments = "This shows whether there are gaps in the sequential list of shipments for the notification",
                MissingShipmentsColour = "The colour indicates the level of compliance based on data shown in the previous cell. 0 = GREEN, 1 to 10 = AMBER and 11 or more = RED",
                OverLimitShipments = "For export notifications: This shows the number of shipments that are 'active' which exceed the number of permitted active loads approved against the financial provision. For import notifications: not monitiored by the service N/A",
                OverActiveLoads = "The colour indicates the level of compliance based on data shown in the previous cell. 0 = GREEN, 1 to 5 = AMBER and 6 or more = RED",
                OverTonnage = "This shows whether the total intended quantity consented on the notification has been exceeded (Y/N)",
                OverTonnageColour = "The colour indicates compliance based on data shown in the previous cell. N = GREEN, and Y = RED",
                OverShipments = "This shows whether the total intended number of shipments consented on the notification has been exceeded (Y/N)",
                OverShipmentsColour = "The colour indicates compliance based on data shown in the previous cell. N = GREEN, and Y = RED",
                Notifier = "This shows the name of the notifier/exporter for the notification",
                Consignee = "This shows the name of the consignee/importer for the notification",
                FileExpired = "This shows whether the notification has expired by having gone passed the 'Consent valid to' date when this report was run"
            };

            var colourGuidance = new ComplianceDataColourGuidance
            {
                HeaderText = "Colour/Symbol key",
                GreenText = "GREEN with a black tick overlay indicates compliance",
                AmberText = "AMBER with a black exclamation indicates non-compliance to a lesser degree",
                RedText = "RED with a white cross indicates non-compliance to a greater degree warranting action"
            };

            return new ComplianceXlsxActionResult(report, new List<ComplianceDataGuidance> { guidance }, colourGuidance,
                fileName, true);
        }
    }
}