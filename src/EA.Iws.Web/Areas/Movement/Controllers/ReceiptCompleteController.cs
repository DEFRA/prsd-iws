namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Movement;
    using ViewModels;

    public class ReceiptCompleteController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ReceiptCompleteController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var notificationId = await client.SendAsync(User.GetAccessToken(), new GetNotificationIdByMovementId(id));

                return View(new ReceiptCompleteViewModel { NotificationId = notificationId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, ReceiptCompleteViewModel model)
        {
            return RedirectToAction("ApprovedNotification", "Applicant", new { id = model.NotificationId });
        }
    }
}