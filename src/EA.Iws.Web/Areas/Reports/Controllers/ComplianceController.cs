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
    using Resources = ComplianceGuidanceResources;

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
                NotificationNumber = Resources.NotificationNumber,
                NoPrenotificationCount = Resources.NoPrenotificationCount,
                PreNotificationColour = Resources.PreNotificationColour,
                MissingShipments = Resources.MissingShipments,
                MissingShipmentsColour = Resources.MissingShipmentsColour,
                OverLimitShipments = Resources.OverLimitShipments,
                OverActiveLoads = Resources.OverActiveLoads,
                OverTonnage = Resources.OverTonnage,
                OverTonnageColour = Resources.OverTonnageColour,
                OverShipments = Resources.OverShipments,
                OverShipmentsColour = Resources.OverShipmentsColour,
                Notifier = Resources.Notifier,
                Consignee = Resources.Consignee,
                FileExpired = Resources.FileExpired
            };

            var colourGuidance = new ComplianceDataColourGuidance
            {
                HeaderText = Resources.ColourHeaderText,
                GreenText = Resources.GreenText,
                AmberText = Resources.AmberText,
                RedText = Resources.RedText
            };

            return new ComplianceXlsxActionResult(report, new List<ComplianceDataGuidance> { guidance }, colourGuidance,
                fileName, true);
        }
    }
}