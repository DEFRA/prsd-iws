namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;
    using ViewModels.FinancialGuarantee;

    [Authorize(Roles = "internal")]
    public class FinancialGuaranteeController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<FinancialGuaranteeData, FinancialGuaranteeDatesViewModel> dateMap;
        private readonly IMap<FinancialGuaranteeData, FinancialGuaranteeDecisionViewModel> decisionMap;
        private readonly IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest> requestMap;

        public FinancialGuaranteeController(IMediator mediator,
            IMap<FinancialGuaranteeData, FinancialGuaranteeDatesViewModel> dateMap,
            IMap<FinancialGuaranteeData, FinancialGuaranteeDecisionViewModel> decisionMap,
            IMapWithParameter<FinancialGuaranteeDecisionViewModel, Guid, FinancialGuaranteeDecisionRequest> requestMap)
        {
            this.mediator = mediator;
            this.dateMap = dateMap;
            this.decisionMap = decisionMap;
            this.requestMap = requestMap;
        }

        [HttpGet]
        public async Task<ActionResult> Dates(Guid id)
        {
            var result = await mediator.SendAsync(new GetFinancialGuaranteeDataByNotificationApplicationId(id));
            return View(dateMap.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Dates(Guid id, FinancialGuaranteeDatesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SetFinancialGuaranteeDates(id, model.Received.AsDateTime(),
                    model.Completed.AsDateTime()));

            return (model.IsRequiredEntryComplete) ?
                RedirectToAction("Index", "Home", new { id, area = "NotificationAssessment" })
                : RedirectToAction("Dates", "FinancialGuarantee", new { id });
        }

        [HttpGet]
        public async Task<ActionResult> Decision(Guid id)
        {
            var result = await mediator.SendAsync(new GetFinancialGuaranteeDataByNotificationApplicationId(id));
            return View(decisionMap.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnToAssessment(Guid id)
        {
            return RedirectToAction("Index", "Home", new { area = "NotificationAssessment" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Decision(Guid id, FinancialGuaranteeDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!model.IsApplicationCompleted)
            {
                return RedirectToAction("Index", "Home", new { id, area = "NotificationAssessment" });
            }

            await mediator.SendAsync(requestMap.Map(model, id));

            return RedirectToAction("Index", "Home", new { id, area = "NotificationAssessment" });
        }
    }
}