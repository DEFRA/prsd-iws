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
        public async Task<ActionResult> Index(Guid id, string filter)
        {
            int screenId = 0;
            int.TryParse(filter, out screenId);

            var response = await mediator.SendAsync(new GetNotificationAudits(id));
            var screens = await mediator.SendAsync(new GetNotificationAuditScreens());

            var model = new UpdateHistoryViewModel(response, screens);
            model.NotificationId = id;
            model.SelectedScreen = filter;

            model.HasHistoryItems = model.UpdateHistoryItems.Count == 0 ? false : true;

            return View(model);
        }

        [HttpGet]
        public ActionResult NoChanges(Guid id)
        {
            var model = new NoChangesViewModel { NotificationId = id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UpdateHistoryViewModel model)
        { 
            // convert model date time to date time and send in redirect
            return RedirectToAction("Index", new { filter = model.SelectedScreen });
        }
    }
}