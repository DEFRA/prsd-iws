namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Movement;
    using Requests.Notification;
    using ViewModels;

    public class OperationCompleteController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public OperationCompleteController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }
 
        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var notificationId = await client.SendAsync(User.GetAccessToken(), new GetNotificationIdByMovementId(id));
                var notification = await client.SendAsync(User.GetAccessToken(), new GetNotificationBasicInfo(notificationId));

                var model = new OperationCompleteViewModel
                {
                    NotificationId = notificationId,
                    NotificationType = notification.NotificationType
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, OperationCompleteViewModel model)
        {
            return RedirectToAction("ApprovedNotification", "Applicant", new { id = model.NotificationId });
        }
    }
}