namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
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
                    return RedirectToAction("Index", "KeyDates");
                }

                return View(model);
            }

            switch (model.SelectedDecision)
            {
                case DecisionType.Consent:
                    await PostConsent(model);
                    break;
                case DecisionType.Withdraw:
                    await PostWithdrawn(model);
                    break;
                case DecisionType.Object:
                    await PostObjection(model);
                    break;
                    case DecisionType.ConsentWithdraw:
                    await PostConsentWithdrawn(model);
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "KeyDates");
        }

        private async Task PostConsentWithdrawn(NotificationAssessmentDecisionViewModel model)
        {
            var date = new DateTime(model.ConsentWithdrawnDate.Year.GetValueOrDefault(), model.ConsentWithdrawnDate.Month.GetValueOrDefault(), model.ConsentWithdrawnDate.Day.GetValueOrDefault());
            var request = new WithdrawConsentForNotificationApplication(model.NotificationId, model.ReasonsForConsentWithdrawal, date);
            await mediator.SendAsync(request);
        }

        private async Task PostWithdrawn(NotificationAssessmentDecisionViewModel model)
        {
            var date = new DateTime(model.WithdrawnDate.Year.GetValueOrDefault(), model.WithdrawnDate.Month.GetValueOrDefault(), model.WithdrawnDate.Day.GetValueOrDefault());

            var request = new WithdrawNotificationApplication(model.NotificationId, date, model.ReasonForWithdrawal);
            await mediator.SendAsync(request);
        }

        private async Task PostConsent(NotificationAssessmentDecisionViewModel model)
        {
            var request = new ConsentNotificationApplication(model.NotificationId,
                model.ConsentValidFromDate.AsDateTime().GetValueOrDefault(),
                model.ConsentValidToDate.AsDateTime().GetValueOrDefault(),
                model.ConsentedDate.AsDateTime().GetValueOrDefault(),
                model.ConsentConditions);

            await mediator.SendAsync(request);
        }

        private async Task PostObjection(NotificationAssessmentDecisionViewModel model)
        {
            var date = new DateTime(model.ObjectionDate.Year.GetValueOrDefault(), model.ObjectionDate.Month.GetValueOrDefault(), model.ObjectionDate.Day.GetValueOrDefault());

            var request = new ObjectNotificationApplication(model.NotificationId, date, model.ReasonForObjection);
            await mediator.SendAsync(request);
        }
    }
}