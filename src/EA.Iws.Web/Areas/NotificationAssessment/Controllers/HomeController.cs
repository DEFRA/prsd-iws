namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using NotificationApplication.ViewModels.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationAssessment;
    using Web.ViewModels.Shared;

    [Authorize(Roles = "internal")]
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
            var result = await mediator.SendAsync(new GetNotificationOverviewInternal(id));
            return View(new NotificationOverviewViewModel(result));
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult AssessmentNavigation(Guid id)
        {
            var response = mediator.SendAsync(new GetNotificationAssessmentSummaryInformation(id)).GetAwaiter().GetResult();
            return PartialView("_AssessmentNavigation", response);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = mediator.SendAsync(new GetNotificationNumber(id)).GetAwaiter().GetResult();

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response));
        }
    }
}