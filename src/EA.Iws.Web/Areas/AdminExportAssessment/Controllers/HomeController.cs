namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationAssessment;
    using Web.ViewModels.Shared;

    [Authorize(Roles = "internal,readonly")]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var financialGuaranteeDecisionRequired = await mediator.SendAsync(new GetFinancialGuaranteeDecisionRequired(id));

            if (financialGuaranteeDecisionRequired)
            {
                return RedirectToAction("Index", "FinancialGuaranteeAssessment");
            }
            else
            {
                return RedirectToAction("Index", "Overview");
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = Task.Run(() => mediator.SendAsync(new GetNotificationNumber(id))).Result;

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response));
        }
    }
}