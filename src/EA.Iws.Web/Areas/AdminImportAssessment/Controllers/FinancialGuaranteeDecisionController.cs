namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeDecision;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeDecisionController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteeDecisionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var decisions = await mediator.SendAsync(new GetAvailableFinancialGuaranteeDecisions(id));
            
            return View(new FinancialGuaranteeDecisionViewModel(decisions));
        }
    }
}