namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeDecisionHistory;

    [AuthorizeActivity(typeof(GetFinancialGuaranteeHistory))]
    public class FinancialGuaranteeDecisionHistoryController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteeDecisionHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var currentFinancialGuarantee = await mediator.SendAsync(new GetCurrentFinancialGuaranteeDetails(id));
            var history = await mediator.SendAsync(new GetFinancialGuaranteeHistory(id));

            var model = new FinancialGuaranteeDecisionHistoryViewModel
            {
                CurrentFinancialGuarantee = currentFinancialGuarantee,
                FinancialGuaranteeHistory = history
            };

            return View(model);
        }
    }
}