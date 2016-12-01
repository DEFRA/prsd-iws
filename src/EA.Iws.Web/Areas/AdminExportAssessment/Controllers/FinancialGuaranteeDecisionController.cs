namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeDecision;

    public class FinancialGuaranteeDecisionController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteeDecisionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, Guid financialGuaranteeId)
        {
            var financialGuarantee = await mediator.SendAsync(
                new GetFinancialGuaranteeDataByNotificationApplicationId(id, financialGuaranteeId));

            if (financialGuarantee.Status != FinancialGuaranteeStatus.ApplicationComplete)
            {
                return RedirectToAction("Index");
            }

            var model = new FinancialGuaranteeDecisionViewModel
            {
                FinancialGuaranteeId = financialGuaranteeId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, FinancialGuaranteeDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await Task.Yield();

            return View(model);
        }
    }
}