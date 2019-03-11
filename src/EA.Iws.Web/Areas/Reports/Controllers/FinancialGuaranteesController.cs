namespace EA.Iws.Web.Areas.Reports.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.Reports;
    using Core.Notification;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using Requests.Admin.Reports;
    using ViewModels.FinancialGuarantees;

    [AuthorizeActivity(typeof(GetFinancialGuaranteesReport))]
    public class FinancialGuaranteesController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var report = await mediator.SendAsync(new GetFinancialGuaranteesReport(
                model.FinancialGuaranteeReferenceNumber,
                model.ExporterName,
                model.ImporterName,
                model.ProducerName));

            var filename = string.Format("financial-guarantees-{0}.xlsx", SystemTime.UtcNow.ToShortDateString());

            var columnsToRemove = new List<int>();
            var competentAuthority = await mediator.SendAsync(new GetUserCompetentAuthority());

            if (competentAuthority != UKCompetentAuthority.England 
                && competentAuthority != UKCompetentAuthority.Wales)
            {
                // CoverAmount
                columnsToRemove.Add(10);
                // CalculationContinued
                columnsToRemove.Add(11);
                // OverActiveLoads
                columnsToRemove.Add(12);
            }

            return new XlsxActionResult<FinancialGuaranteesData>(report, filename, columnsToHide: string.Join(",", columnsToRemove));
        }
    }
}