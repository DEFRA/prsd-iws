namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels;
    using ViewModels.Decision;

    [Authorize(Roles = "internal")]
    public class DecisionController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<NotificationAssessmentDecisionData, NotificationAssessmentDecisionViewModel> decisionMap;

        public DecisionController(IMediator mediator,
            IMap<NotificationAssessmentDecisionData, NotificationAssessmentDecisionViewModel> decisionMap)
        {
            this.mediator = mediator;
            this.decisionMap = decisionMap;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetNotificationAssessmentDecisionData(id));
            return View(decisionMap.Map(data));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, NotificationAssessmentDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.DecisionTypes.Count == 0)
                {
                    return RedirectToAction("Index", "Home", new { id, area = "NotificationAssessment" });
                }

                return View(model);
            }

            switch (model.SelectedDecision)
            {
                case DecisionType.Consent:
                    await PostConsent(model);
                    break;
                case DecisionType.Withdrawn:
                    await PostWithdrawn(model);
                    break;
                case DecisionType.Object:
                    await PostObjection(model);
                    break;
                    case DecisionType.ConsentWithdrawn:
                    await PostConsentWithdrawn(model);
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "Home", new { area = "NotificationAssessment" });
        }

        private async Task PostConsentWithdrawn(NotificationAssessmentDecisionViewModel model)
        {
            var request = new WithdrawConsentForNotificationApplication(model.NotificationId, model.ReasonsForConsentWithdrawal);
            await mediator.SendAsync(request);
        }

        private async Task PostWithdrawn(NotificationAssessmentDecisionViewModel model)
        {
            var request = new WithdrawNotificationApplication(model.NotificationId);
            await mediator.SendAsync(request);
        }

        private async Task PostConsent(NotificationAssessmentDecisionViewModel model)
        {
            var request = new ConsentNotificationApplication(model.NotificationId,
                model.ConsentValidFromDate.AsDateTime().GetValueOrDefault(),
                model.ConsentValidToDate.AsDateTime().GetValueOrDefault(),
                model.ConsentConditions);

            await mediator.SendAsync(request);
        }

        private async Task PostObjection(NotificationAssessmentDecisionViewModel model)
        {
            var request = new ObjectNotificationApplication(model.NotificationId);
            await mediator.SendAsync(request);
        }
    }
}