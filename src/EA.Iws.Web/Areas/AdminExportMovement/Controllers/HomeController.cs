namespace EA.Iws.Web.Areas.AdminExportMovement.Controllers
{
    using System;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Web.ViewModels.Shared;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = mediator.SendAsync(new GetNotificationNumberByMovementId(id)).GetAwaiter().GetResult();

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response));
        }
    }
}