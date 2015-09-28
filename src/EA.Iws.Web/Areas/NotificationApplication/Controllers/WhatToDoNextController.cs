namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Notification;
    using Infrastructure;
    using Requests.Notification;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class WhatToDoNextController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public WhatToDoNextController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetWhatToDoNextDataForNotification(id));

                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, FormCollection model)
        {
            return RedirectToAction("Home", "Applicant", new { area = string.Empty });
        }

        [HttpGet]
        public ActionResult CardPaymentInformation(CompetentAuthority competentAuthority)
        {
            switch (competentAuthority)
            {
                case CompetentAuthority.England:
                    return PartialView("_EaCard");
                case CompetentAuthority.Scotland:
                    return PartialView("_SepaCard");
                case CompetentAuthority.Wales:
                    return PartialView("_NrwCard");
                default:
                    return new EmptyResult();
            }
        }
    }
}