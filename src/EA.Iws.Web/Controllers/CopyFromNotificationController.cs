namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Requests.Copy;
    using ViewModels.CopyFromNotification;

    [Authorize]
    public class CopyFromNotificationController : Controller
    {
        private readonly IMediator mediator;

        public CopyFromNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
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
                    mediator.SendAsync(new CopyToNotification(model.SelectedNotification.Value, id));

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