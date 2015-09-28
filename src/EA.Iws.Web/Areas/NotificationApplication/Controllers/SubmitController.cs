namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.NotificationAssessment;
    using Infrastructure;
    using Requests.Notification;
    using Requests.NotificationAssessment;
    using ViewModels.NotificationApplication;

    [Authorize]
    public class SubmitController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public SubmitController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Disclaimer(Guid id)
        {
            using (var client = apiClient())
            {
                var assessmentInfo =
                    await client.SendAsync(User.GetAccessToken(), new GetNotificationAssessmentSummaryInformation(id));

                var status = assessmentInfo.Status;

                if (status == NotificationStatus.NotSubmitted)
                {
                    var model = new DisclaimerViewModel { Id = id };
                    return View(model);
                }
                else
                {
                    ViewBag.NotificationId = id;
                    return View("AlreadySubmitted");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disclaimer(DisclaimerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(),
                    new SubmitNotification(model.Id));
                return RedirectToAction("Index", "WhatToDoNext", new { id = model.Id });
            }
        }
    }
}