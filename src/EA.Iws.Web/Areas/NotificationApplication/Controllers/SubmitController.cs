namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationAssessment;
    using ViewModels.NotificationApplication;

    [Authorize]
    public class SubmitController : Controller
    {
        private readonly IMediator mediator;

        public SubmitController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Disclaimer(Guid id)
        {
            var assessmentInfo =
                await mediator.SendAsync(new GetNotificationAssessmentSummaryInformation(id));

            var status = assessmentInfo.Status;

            if (status == NotificationStatus.NotSubmitted)
            {
                var model = new DisclaimerViewModel
                {
                    Id = id,
                    CompetentAuthority = assessmentInfo.CompetentAuthority
                };
                return View(model);
            }

            ViewBag.NotificationId = id;
            return View("AlreadySubmitted");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disclaimer(DisclaimerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SubmitNotification(model.Id));

            if (User.IsInternalUser())
            {
                return RedirectToAction("Index", "KeyDates", new { area = "AdminExportAssessment", id = model.Id });
            }

            return RedirectToAction("Index", "WhatToDoNext", new { id = model.Id });
        }
    }
}