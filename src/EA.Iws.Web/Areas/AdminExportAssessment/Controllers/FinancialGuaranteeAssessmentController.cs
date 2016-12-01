namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.FinancialGuarantee;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeAssessment;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeAssessmentController : Controller
    {
        private readonly IMediator mediator;

        public FinancialGuaranteeAssessmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var currentFinancialGuarantee = await mediator.SendAsync(new GetCurrentFinancialGuaranteeDetails(id));

            return View(currentFinancialGuarantee);
        }

        [HttpGet]
        public ActionResult New(Guid id)
        {
            return View(new NewFinancialGuaranteeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New(Guid id, NewFinancialGuaranteeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new CreateFinancialGuarantee(id, model.ReceivedDate.AsDateTime().Value));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Complete(Guid id, Guid financialGuaranteeId)
        {
            var financialGuarantee = await mediator.SendAsync(
                new GetFinancialGuaranteeDataByNotificationApplicationId(id, financialGuaranteeId));

            if (financialGuarantee.Status != FinancialGuaranteeStatus.ApplicationReceived)
            {
                return RedirectToAction("Index");
            }

            var model = new CompleteFinancialGuaranteeViewModel
            {
                FinancialGuaranteeId = financialGuaranteeId,
                ReceivedDate = financialGuarantee.ReceivedDate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(Guid id, CompleteFinancialGuaranteeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(
                new CompleteFinancialGuarantee(id, model.FinancialGuaranteeId, model.CompleteDate.AsDateTime().Value));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Decision(Guid id, Guid financialGuaranteeId)
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
        public async Task<ActionResult> Decision(Guid id, FinancialGuaranteeDecisionViewModel model)
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