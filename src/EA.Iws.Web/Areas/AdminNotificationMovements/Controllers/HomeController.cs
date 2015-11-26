namespace EA.Iws.Web.Areas.AdminNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using ViewModels.Home;
    using Web.ViewModels.Shared;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int? status)
        {
            var movementsSummary =
                await mediator.SendAsync(new GetSummaryAndTable(id, (MovementStatus?)status));

            var model = new MovementSummaryViewModel(id, movementsSummary);
            model.SelectedMovementStatus = (MovementStatus?)status;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(Guid id, int? selectedMovementStatus)
        {
            return RedirectToAction("Index", new { id, status = selectedMovementStatus });
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = mediator.SendAsync(new GetNotificationNumber(id)).GetAwaiter().GetResult();

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response));
        }
    }
}