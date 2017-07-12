namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.Admin;
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

            var info = await mediator.SendAsync(new GetNotificationInfo(model.Number));

            if (info.IsExistingNotification)
            {
                if (info.TradeDirection == TradeDirection.Export &&
                    info.ExportNotificationStatus != NotificationStatus.NotSubmitted)
                {
                    return RedirectToAction("Index", "Home", new { id = info.Id, area = "AdminExportAssessment" });
                }

                if (info.TradeDirection == TradeDirection.Import)
                {
                    return RedirectToAction("Index", "Home", new { id = info.Id, area = "ImportNotification" });
                }
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