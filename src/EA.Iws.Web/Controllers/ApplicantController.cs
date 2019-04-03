namespace EA.Iws.Web.Controllers
{
    using System.Security;
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
            var response = await mediator.SendAsync(new GetExportNotificationsByUser(page, notificationStatus));

            var model = new UserNotificationsViewModel(response);
            model.SelectedNotificationStatus = notificationStatus;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Home")]
        public async Task<ActionResult> HomePost(UserNotificationsViewModel model, string button, int page = 1)
        {
            if (button == "search")
            {
                if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                {
                    try
                    {
                        var id = await mediator.SendAsync(new GetNotificationIdByNumber(model.SearchTerm));

                        return id.HasValue ? RedirectToAction("Index", "Options", new { area = "NotificationApplication", id }) : RedirectToAction("NotFound");
                    }
                    catch (SecurityException)
                    {
                        return RedirectToAction("NotFound");
                    }
                }

                ModelState.AddModelError("SearchTerm", "Please enter a valid search term");
                return await Home(null, page);
            }

            return RedirectToAction("Home", new { page = 1, status = (int?)model.SelectedNotificationStatus });
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}