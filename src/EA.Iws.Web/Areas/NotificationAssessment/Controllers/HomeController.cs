namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.NotificationAssessment;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public HomeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult AssessmentNavigation(Guid id)
        {
            using (var client = apiClient())
            {
                var response =
                    client.SendAsync(User.GetAccessToken(), new GetNotificationAssessmentSummaryInformation(id)).GetAwaiter().GetResult();

                return PartialView("_AssessmentNavigation", response);
            }
        }
    }
}