namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.FinancialGuarantee;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuaranteeDecision;

    [AuthorizeActivity(typeof(GetFinancialGuaranteeDataByNotificationApplicationId))]
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
                return RedirectToAction("Index", "FinancialGuaranteeAssessment");
            }

            var model = new FinancialGuaranteeDecisionViewModel
            {
                FinancialGuaranteeId = financialGuaranteeId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, FinancialGuaranteeDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            switch (model.Decision.Value)
            {
                case FinancialGuaranteeDecision.Approved:
                    return RedirectToAction("Approve", new { financialGuaranteeId = model.FinancialGuaranteeId });
                case FinancialGuaranteeDecision.Refused:
                    return RedirectToAction("Refuse", new { financialGuaranteeId = model.FinancialGuaranteeId });
                case FinancialGuaranteeDecision.Released:
                    return RedirectToAction("Release", new { financialGuaranteeId = model.FinancialGuaranteeId });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Approve(Guid id, Guid financialGuaranteeId)
        {
            var financialGuarantee = await mediator.SendAsync(
                new GetFinancialGuaranteeDataByNotificationApplicationId(id, financialGuaranteeId));

            if (financialGuarantee.Status != FinancialGuaranteeStatus.ApplicationComplete)
            {
                return RedirectToAction("Index", "FinancialGuaranteeAssessment");
            }

            var model = new ApproveFinancialGuaranteeViewModel(financialGuarantee)
            {
                NotificationId = id,
                FinancialGuaranteeId = financialGuaranteeId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(Guid id, ApproveFinancialGuaranteeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new ApproveFinancialGuarantee(id, model.FinancialGuaranteeId,
                model.DecisionMadeDate.AsDateTime().Value,
                model.ReferenceNumber, model.ActiveLoadsPermitted.Value, model.IsBlanketBond.Value));

            return RedirectToAction("Index", "FinancialGuaranteeAssessment");
        }

        [HttpGet]
        public async Task<ActionResult> Refuse(Guid id, Guid financialGuaranteeId)
        {
            var financialGuarantee = await mediator.SendAsync(
                new GetFinancialGuaranteeDataByNotificationApplicationId(id, financialGuaranteeId));

            if (financialGuarantee.Status != FinancialGuaranteeStatus.ApplicationComplete)
            {
                return RedirectToAction("Index", "FinancialGuaranteeAssessment");
            }

            var model = new RefuseFinancialGuaranteeViewModel(financialGuarantee)
            {
                NotificationId = id,
                FinancialGuaranteeId = financialGuaranteeId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Refuse(Guid id, RefuseFinancialGuaranteeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new RefuseFinancialGuarantee(id, model.FinancialGuaranteeId,
                model.DecisionMadeDate.AsDateTime().Value, model.ReasonForRefusal));

            return RedirectToAction("Index", "FinancialGuaranteeAssessment");
        }

        [HttpGet]
        public async Task<ActionResult> Release(Guid id, Guid financialGuaranteeId)
        {
            var financialGuarantee = await mediator.SendAsync(
                new GetFinancialGuaranteeDataByNotificationApplicationId(id, financialGuaranteeId));

            if (financialGuarantee.Status != FinancialGuaranteeStatus.Approved)
            {
                return RedirectToAction("Index", "FinancialGuaranteeAssessment");
            }

            var model = new ReleaseFinancialGuaranteeViewModel(financialGuarantee)
            {
                NotificationId = id,
                FinancialGuaranteeId = financialGuaranteeId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Release(Guid id, ReleaseFinancialGuaranteeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new ReleaseFinancialGuarantee(id, model.FinancialGuaranteeId,
                  model.DecisionMadeDate.AsDateTime().Value));

            return RedirectToAction("Released");
        }

        [HttpGet]
        public ActionResult Released()
        {
            return View();
        }
    }
}