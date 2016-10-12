namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Infrastructure.Authorization;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Facilities;
    using Requests.Notification;
    using Requests.NotificationAssessment;
    using ViewModels.Decision;

    [AuthorizeActivity(ExportNotificationPermissions.CanMakeExportNotificationAssessmentDecision)]
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
            var model = decisionMap.Map(data);
            return View(model);
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
                    if (await ConsentDatesAreValid(model))
                    {
                        await PostConsent(model);
                    }
                    else
                    {
                        return View(model);
                    }
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

        private async Task<bool> ConsentDatesAreValid(NotificationAssessmentDecisionViewModel model)
        {
            bool areValid = true;
            var data = await mediator.SendAsync(new GetNotificationAssessmentDecisionData(model.NotificationId));

            if (model.ConsentedDate.AsDateTime() > DateTime.UtcNow)
            {
                ModelState.AddModelError("ConsentedDate", DecisionControllerResources.ConsentedNotInFuture);
                areValid = false;
            }

            if (model.ConsentedDate.AsDateTime() < data.AcknowledgedOnDate)
            {
                ModelState.AddModelError("ConsentedDate", DecisionControllerResources.ConsentedNotBeforeAcknowledged);
                areValid = false;
            }

            if (model.ConsentValidFromDate.AsDateTime() > DateTime.UtcNow)
            {
                ModelState.AddModelError("ConsentValidFromDate", DecisionControllerResources.ValidFromNotInFuture);
                areValid = false;
            }

            if (model.ConsentValidFromDate.AsDateTime() < data.AcknowledgedOnDate)
            {
                ModelState.AddModelError("ConsentValidFromDate", DecisionControllerResources.ValidFromNotBeforeAcknowledged);
                areValid = false;
            }

            if (model.ConsentValidToDate.AsDateTime() <= DateTime.Today)
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