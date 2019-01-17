namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
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
        public async Task<ActionResult> Index(Guid id, int page = 1, string filter, DateTime? startDate, DateTime? endDate)
        {
            int screenId = 0;
            int.TryParse(filter, out screenId);
            var response = await mediator.SendAsync(new GetNotificationAuditTable(id, page));
            var screens = await mediator.SendAsync(new GetNotificationAuditScreens());

            var model = new UpdateHistoryViewModel(response, screens);
            model.NotificationId = id;
            model.SelectedScreen = filter;

            model.HasHistoryItems = model.UpdateHistoryItems.Count == 0 ? false : true;

            model.SetDates(startDate, endDate);

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
        public async Task<ActionResult> Index(Guid id, UpdateHistoryViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (!ModelState.IsValid)
            {
                var screens = await mediator.SendAsync(new GetNotificationAuditScreens());
                var response = await mediator.SendAsync(new GetNotificationAudits(id));
                model.Screens = screens.ToList();
                model.UpdateHistoryItems = response.ToList();
                return View(model);
            }

            DateTime startDate = new DateTime(model.StartYear.GetValueOrDefault(), model.StartMonth.GetValueOrDefault(), model.StartDay.GetValueOrDefault());
            DateTime endDate = new DateTime(model.EndYear.GetValueOrDefault(), model.EndMonth.GetValueOrDefault(), model.EndDay.GetValueOrDefault());

            return RedirectToAction("Index", new { filter = model.SelectedScreen, startDate = startDate, endDate = endDate });
        }
    }
}