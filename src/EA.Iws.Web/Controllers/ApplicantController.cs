namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationAssessment;
    using ViewModels.Applicant;

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

        [HttpGet]
        public async Task<ActionResult> ApprovedNotification(Guid id)
        {
            ApprovedNotificationViewModel model;

            var notificationInfo = await mediator.SendAsync(new GetNotificationBasicInfo(id));
            model = new ApprovedNotificationViewModel(notificationInfo.NotificationType);
            
            model.NotificationId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApprovedNotification(ApprovedNotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.UserChoices.SelectedValue == 1)
            {
                var notificationAssessmentInfo = await mediator.SendAsync(new GetNotificationAssessmentSummaryInformation(model.NotificationId));

                if (notificationAssessmentInfo.Status == NotificationStatus.NotSubmitted)
                {
                    return RedirectToAction("Index", "Exporter", new { id = model.NotificationId, area = "NotificationApplication" });
                }

                return RedirectToAction("Index", "Home", new { id = model.NotificationId, area = "NotificationApplication" });
            }
            if (model.UserChoices.SelectedValue == 2)
            {
                return RedirectToAction("GenerateNotificationDocument", "Home", new { id = model.NotificationId, area = "NotificationApplication" });
            }
            if (model.UserChoices.SelectedValue == 3)
            {
                return RedirectToAction("Index", "NotificationMovement", new { id = model.NotificationId });
            }
            if (model.UserChoices.SelectedValue == 4)
            {
                return RedirectToAction("Receipt", "NotificationMovement", new { id = model.NotificationId });
            }
            if (model.UserChoices.SelectedValue == 5)
            {
                return RedirectToAction("Operation", "NotificationMovement", new { id = model.NotificationId });
            }
            if (model.UserChoices.SelectedValue == 6)
            {
                return RedirectToAction("Operation", "NotificationMovement", new { id = model.NotificationId });
            }

            throw new InvalidOperationException("Invalid user choice to view applicant's notification");
        }
    }
}