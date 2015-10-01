namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels;

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
                return View(model);
            }

            switch (model.SelectedDecision)
            {
                case DecisionType.Consent:
                    await PostConsent(model);
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "Home", new { area = "NotificationAssessment" });
        }

        private async Task PostConsent(NotificationAssessmentDecisionViewModel model)
        {
            var request = new ConsentNotificationApplication(model.NotificationId,
                new DateRange(model.ConsentValidFromDate.AsDateTime().Value, model.ConsentValidToDate.AsDateTime().Value),
                model.ConsentConditions);

            await mediator.SendAsync(request);
        }

        private static SelectList GetDecisionTypes()
        {
            return new SelectList(EnumHelper.GetValues(typeof(DecisionType)), "Key", "Value");
        }
    }
}