namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using ViewModels.Shared;

    public class NotificationSwitchController : Controller
    {
        private readonly IMediator mediator;

        public NotificationSwitchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Switch(NotificationSwitcherViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Number)
                || string.IsNullOrWhiteSpace(model.OriginalNumber)
                || model.Number.Equals(model.OriginalNumber, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            var id = await mediator.SendAsync(new GetNotificationIdByNumber(model.Number));

            return new EmptyResult();
        } 
    }
}