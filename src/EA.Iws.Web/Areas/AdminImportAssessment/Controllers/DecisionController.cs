namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;
    using Prsd.Core;
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
                    if (await ConsentDatesAreValid(id, model))
                    {
                        await PostConsent(id, model);
                    }
                    else
                    {
                        return View(model);
                    }
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
                model.ConsentValidFromDate.AsDateTime().GetValueOrDefault(),
                model.ConsentValidToDate.AsDateTime().GetValueOrDefault(),
                model.ConsentConditions,
                model.ConsentGivenDate.AsDateTime().GetValueOrDefault()));
        }

        private async Task PostConsentWithdrawn(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new WithdrawConsentForImportNotification(id, 
                model.ReasonsForConsentWithdrawal, 
                model.ConsentWithdrawnDate.AsDateTime().GetValueOrDefault()));
        }

        private async Task PostObjection(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new ObjectToImportNotification(id, 
                model.ReasonForObjection, 
                model.ObjectionDate.AsDateTime().GetValueOrDefault()));
        }

        private async Task Postwithdrawn(Guid id, DecisionViewModel model)
        {
            await mediator.SendAsync(new WithdrawImportNotification(id,
                model.ReasonForWithdrawal, 
                model.WithdrawnDate.AsDateTime().GetValueOrDefault()));
        }

        private async Task<bool> ConsentDatesAreValid(Guid id, DecisionViewModel model)
        {
            bool areValid = true;
            var data = await mediator.SendAsync(new GetImportNotificationAssessmentDecisionData(id));

            if (model.ConsentGivenDate.AsDateTime() > SystemTime.UtcNow)
            {
                ModelState.AddModelError("ConsentGivenDate", DecisionControllerResources.ConsentedNotInFuture);
                areValid = false;
            }

            if (model.ConsentGivenDate.AsDateTime() < data.AcknowledgedOnDate)
            {
                ModelState.AddModelError("ConsentGivenDate", DecisionControllerResources.ConsentedNotBeforeAcknowledged);
                areValid = false;
            }

            if (model.ConsentValidFromDate.AsDateTime() < data.AcknowledgedOnDate)
            {
                ModelState.AddModelError("ConsentValidFromDate", DecisionControllerResources.ValidFromNotBeforeAcknowledged);
                areValid = false;
            }

            if (model.ConsentValidToDate.AsDateTime() <= SystemTime.UtcNow.Date)
            {
                ModelState.AddModelError("ConsentValidToDate", DecisionControllerResources.ValidToMustBeInFuture);
                areValid = false;
            }

            DateTime validFromDate = model.ConsentValidFromDate.AsDateTime().GetValueOrDefault();

            if (data.IsPreconsented && model.ConsentValidToDate.AsDateTime() > validFromDate.AddYears(3))
            {
                ModelState.AddModelError("ConsentValidToDate", DecisionControllerResources.ValidFromPreconsented);
                areValid = false;
            }

            if ((!data.IsPreconsented) && model.ConsentValidToDate.AsDateTime() > validFromDate.AddYears(1))
            {
                ModelState.AddModelError("ConsentValidToDate", DecisionControllerResources.ValidFromNotPreconsented);
                areValid = false;
            }

            return areValid;
        }
    }
}