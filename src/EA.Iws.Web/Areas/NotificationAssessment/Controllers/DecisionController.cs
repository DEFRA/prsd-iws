namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.NotificationAssessment;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;
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
        public async Task<ActionResult> NewIndex(Guid id)
        {
            var data = await mediator.SendAsync(new GetNotificationAssessmentDecisionData(id));

            return View(decisionMap.Map(data));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewIndex(Guid id, NotificationAssessmentDecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new DecisionViewModel
            {
                NotificationId = id, 
                DecisionTypes = GetDecisionTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(DecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DecisionTypes = GetDecisionTypes();
                return View(model);
            }

            var setDates = new SetDecision
            {
                NotificationApplicationId = model.NotificationId,
                ConditionsOfConsent = model.ConditionsOfConsent,
                DecisionMade = model.DecisionMadeDate.AsDateTime(),
                ConsentedFrom = model.ConsentValidFromDate.AsDateTime(),
                ConsentedTo = model.ConsentValidToDate.AsDateTime(),
                DecisionType = Convert.ToInt32(model.DecisionType)
            };

            await mediator.SendAsync(setDates);
            
            model.DecisionTypes = GetDecisionTypes();

            return RedirectToAction("Index", "Home", new { area = "NotificationAssessment" });
        }

        private static SelectList GetDecisionTypes()
        {
            return new SelectList(EnumHelper.GetValues(typeof(DecisionType)), "Key", "Value");
        }
    }
}