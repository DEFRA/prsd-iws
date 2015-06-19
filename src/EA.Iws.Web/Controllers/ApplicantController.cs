namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Notification;

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
    }
}