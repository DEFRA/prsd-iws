namespace EA.Iws.Web.Controllers
{
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    [Authorize]
    public class ApplicantController : Controller
    {
        private readonly IMediator mediator;

        public ApplicantController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult _GetUserNotifications()
        {
            // Child actions (partial views) cannot be async and we must therefore get the result of the task.
            // The called code must use ConfigureAwait(false) on async tasks to prevent deadlock.
            var response =
                mediator.SendAsync(new GetNotificationsByUser()).Result;

            return PartialView(response);
        }
    }
}