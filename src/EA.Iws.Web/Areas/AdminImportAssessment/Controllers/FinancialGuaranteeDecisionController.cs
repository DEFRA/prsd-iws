namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeDecision;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeDecisionController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest> requestMap;

        public FinancialGuaranteeDecisionController(IMediator mediator, 
            IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest> requestMap)
        {
            this.mediator = mediator;
            this.requestMap = requestMap;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var decisions = await mediator.SendAsync(new GetAvailableFinancialGuaranteeDecisions(id));
            
            return View(new FinancialGuaranteeDecisionViewModel(decisions));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, FinancialGuaranteeDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = requestMap.Map(model, id);

            await mediator.SendAsync(request);

            return RedirectToAction("Index", "KeyDates");
        } 
    }
}