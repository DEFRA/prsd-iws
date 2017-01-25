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
                    var id = await mediator.SendAsync(new GetNotificationIdByNumber(model.SearchTerm));

                    if (id.HasValue)
                    {
                        return RedirectToAction("Index", "Options", new { area = "NotificationApplication", id });
                    }
                    else
                    {
                        return RedirectToAction("NotFound");
                    }
                }
                else
                {
                    ModelState.AddModelError("SearchTerm", "Please enter a valid search term");
                    return await Home(null, page);
                }
            }
            else
            {
                return RedirectToAction("Home", new { page = 1, status = (int?)model.SelectedNotificationStatus });
            }
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}