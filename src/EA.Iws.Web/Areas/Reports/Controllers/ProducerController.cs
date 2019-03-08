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
    using ViewModels.Producer;

    [AuthorizeActivity(typeof(GetProducerReport))]
    public class ProducerController : Controller
    {
        private readonly IMediator mediator;

        public ProducerController(IMediator mediator)
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

            var dateType = ReportEnumParser.TryParse<ProducerReportDates>(model.InputParameters.SelectedDate);
            var textFieldType = ReportEnumParser.TryParse<ProducerReportTextFields>(model.InputParameters.SelectedTextField);
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
        public async Task<ActionResult> Download(ProducerReportDates dateType,
            DateTime from,
            DateTime to,
            ProducerReportTextFields? textFieldType,
            TextFieldOperator? operatorType,
            string textSearch)
        {
            var report =
                await
                    mediator.SendAsync(new GetProducerReport(dateType, from, to, textFieldType, operatorType, textSearch));

            var fileName = string.Format("producer-report-{0}-{1}.xlsx", from.ToShortDateString(), to.ToShortDateString());

            return new XlsxActionResult<ProducerData>(report, fileName, true);
        }
    }
}