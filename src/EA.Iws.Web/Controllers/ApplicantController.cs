namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Notification;
    using ViewModels.Applicant;

    [Authorize]
    public class ApplicantController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ApplicantController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult _GetUserNotifications()
        {
            using (var client = apiClient())
            {
                // Child actions (partial views) cannot be async and we must therefore get the result of the task.
                // The called code must use ConfigureAwait(false) on async tasks to prevent deadlock.
                var response =
                    client.SendAsync(User.GetAccessToken(), new GetNotificationsByUser()).Result;

                return PartialView(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ApprovedNotification(Guid id)
        {
            ApprovedNotificationViewModel model;
            using (var client = apiClient())
            {
                var notificationInfo = await client.SendAsync(User.GetAccessToken(), new GetNotificationBasicInfo(id));
                model = new ApprovedNotificationViewModel(notificationInfo.NotificationType);
            }
            model.NotificationId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovedNotification(ApprovedNotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.UserChoices.SelectedValue == 1)
            {
                return RedirectToAction("Index", "Home", new { id = model.NotificationId, area = "NotificationApplication" });
            }
            if (model.UserChoices.SelectedValue == 2)
            {
                return RedirectToAction("GenerateNotificationDocument", "Home", new { id = model.NotificationId, area = "NotificationApplication" });
            }
            if (model.UserChoices.SelectedValue == 3)
            {
                return RedirectToAction("Index", "ShipmentDate", new { id = model.NotificationId, area = "MovementDocument" });
            }
            if (model.UserChoices.SelectedValue == 4)
            {
                return RedirectToAction("Home", "Applicant", new { id = model.NotificationId });
            }
            if (model.UserChoices.SelectedValue == 5)
            {
                return RedirectToAction("Home", "Applicant", new { id = model.NotificationId });
            }
            if (model.UserChoices.SelectedValue == 6)
            {
                return RedirectToAction("Home", "Applicant", new { id = model.NotificationId });
            }

            throw new InvalidOperationException("Invalid user choice to view applicant's notification");
        }
    }
}