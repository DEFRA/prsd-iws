namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using ViewModels.NotificationSwitch;
    using ViewModels.Shared;

    [Authorize]
    public class NotificationSwitchController : Controller
    {
        private readonly IMediator mediator;
        
        public NotificationSwitchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Switch(NotificationSwitcherViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Number)
                || string.IsNullOrWhiteSpace(model.OriginalNumber)
                || model.Number.Equals(model.OriginalNumber, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            var idAndStatus = await mediator.SendAsync(new GetNotificationIdAndStatusByNumber(model.Number));

            if (idAndStatus.Item1.HasValue && idAndStatus.Item2 != NotificationStatus.NotSubmitted)
            {
                return RedirectToAction("Index", "Home", new { id = idAndStatus.Item1, area = "AdminExportAssessment" });
            }

            return RedirectToAction("NotFound", new { number = model.Number });
        }

        [HttpGet]
        public ActionResult NotFound(string number)
        {
            return View(new NotFoundViewModel { Number = number });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NotFound(string number, FormCollection formCollection)
        {
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}