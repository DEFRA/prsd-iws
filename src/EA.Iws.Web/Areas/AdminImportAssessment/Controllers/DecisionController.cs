namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using ViewModels.Decision;

    [AuthorizeActivity(ImportNotificationPermissions.CanMakeImportNotificationAssessmentDecision)]
    public class DecisionController : Controller
    {
        private readonly IMediator mediator;

        public DecisionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetImportNotificationAssessmentDecisionData(id));
            var model = new DecisionViewModel(data);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, DecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            switch (model.Decision)
            {
                case DecisionType.Consent:
                    await PostConsent(id, model);
                    break;
                case DecisionType.ConsentWithdraw:
                    await PostConsentWithdrawn(id, model);
                    break;
                case DecisionType.Object:
                    await PostObjection(id, model);
                    break;
                case DecisionType.Withdraw:
                    await Postwithdrawn(id, model);
                    break;
                default:
                    break;
            }

            return RedirectToAction("Index", "KeyDates");
        }

        private async Task PostConsent(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new Consent(id,
                model.ConsentValidFromDate.AsDateTime().Value,
                model.ConsentValidToDate.AsDateTime().Value,
                model.ConsentConditions,
                model.ConsentGivenDate.AsDateTime().Value));
        }

        private async Task PostConsentWithdrawn(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new WithdrawConsentForImportNotification(id, 
                model.ReasonsForConsentWithdrawal, 
                model.ConsentWithdrawnDate.AsDateTime().Value));
        }

        private async Task PostObjection(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new ObjectToImportNotification(id, 
                model.ReasonForObjection, 
                model.ObjectionDate.AsDateTime().Value));
        }

        private async Task Postwithdrawn(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new WithdrawImportNotification(id,
                model.ReasonForWithdrawal, 
                model.WithdrawnDate.AsDateTime().Value));
        }
    }
}