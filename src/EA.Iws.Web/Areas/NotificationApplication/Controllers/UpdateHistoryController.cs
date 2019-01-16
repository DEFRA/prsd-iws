namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using ViewModels.UpdateHistory;

    [Authorize]
    public class UpdateHistoryController : Controller
    {
        private readonly IMediator mediator;

        public UpdateHistoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var response = await mediator.SendAsync(new GetNotificationAudits(id));

            var model = new UpdateHistoryViewModel(response);
            model.NotificationId = id;

            if (model.UpdateHistoryItems.Count > 0)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("NoChanges", "UpdateHistory", new { id });
            }
        }

        [HttpGet]
        public ActionResult NoChanges(Guid id)
        {
            var model = new NoChangesViewModel { NotificationId = id };

            return View(model);
        }
    }
}