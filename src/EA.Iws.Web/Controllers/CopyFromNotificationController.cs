namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Requests.Copy;
    using ViewModels.CopyFromNotification;

    [Authorize]
    public class CopyFromNotificationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public CopyFromNotificationController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(), new GetNotificationsToCopyForUser());
                
                return View(new CopyFromNotificationViewModel { Notifications = result });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CopyFromNotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    var resultId = await
                        client.SendAsync(User.GetAccessToken(), new CopyToNotification(model.SelectedNotification.Value, id));

                    if (resultId != Guid.Empty)
                    {
                        return RedirectToAction("Result", "CopyFromNotification", new { id = resultId });
                    }
                }
                catch (ApiException)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred copying this record. Please try again.");
                }
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Result(Guid id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Result(Guid id, FormCollection formCollection)
        {
            return RedirectToAction("Index", "Home", new { id, area = "NotificationApplication" });
        }
    }
}