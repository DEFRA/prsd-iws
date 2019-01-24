namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification.Audit;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Requests.Copy;
    using ViewModels.CopyFromNotification;

    [Authorize]
    public class CopyFromNotificationController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public CopyFromNotificationController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetNotificationsToCopyForUser(id));

            return View(new CopyFromNotificationViewModel { Notifications = result });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CopyFromNotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            try
            {
                var resultId = await
                    mediator.SendAsync(new CopyToNotification(model.SelectedNotification.GetValueOrDefault(), id));

                if (resultId != Guid.Empty)
                {
                    await this.auditService.AddAuditEntry(this.mediator,
                        resultId,
                        User.GetUserId(),
                        NotificationAuditType.Added,
                        NotificationAuditScreenType.CopiedFromNotification);

                    return RedirectToAction("Result", "CopyFromNotification", new { id = resultId });
                }
            }
            catch (ApiException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred copying this record. Please try again.");
            }

            return View(model);
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
            return RedirectToAction("Index", "Shipment", new { id, area = "NotificationApplication", backToOverview = true });
        }
    }
}