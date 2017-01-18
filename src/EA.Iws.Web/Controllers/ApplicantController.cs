namespace EA.Iws.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using ViewModels.Applicant;

    [AuthorizeActivity(typeof(GetExportNotificationsByUser))]
    public class ApplicantController : Controller
    {
        private readonly IMediator mediator;

        public ApplicantController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Home(int? status, int page = 1)
        {
            var notificationStatus = (NotificationStatus?)status;
            var response = (await mediator.SendAsync(new GetExportNotificationsByUser(page, notificationStatus)));

            var model = new UserNotificationsViewModel(response);
            model.SelectedNotificationStatus = notificationStatus;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Home")]
        public ActionResult HomePost(int? selectedNotificationStatus)
        {
            return RedirectToAction("Home", new { page = 1, status = selectedNotificationStatus });
        }
    }
}