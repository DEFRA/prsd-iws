namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;
    using Requests.NotificationMovements;
    using ViewModels.Home;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(GetSummaryAndTable))]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;
        private readonly AuthorizationService authorizationService;

        public HomeController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int? status, int page = 1)
        {
            var movementsSummary = await mediator.SendAsync(new GetSummaryAndTable(id, (MovementStatus?)status, page));
            var canDeleteMovement = await authorizationService.AuthorizeActivity(typeof(DeleteMovement));

            var model = new MovementSummaryViewModel(id, movementsSummary);
            model.SelectedMovementStatus = (MovementStatus?)status;
            model.CanDeleteMovement = canDeleteMovement;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(Guid id, int? selectedMovementStatus)
        {
            return RedirectToAction("Index", new { id, status = selectedMovementStatus, page = 1 });
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = Task.Run(() => mediator.SendAsync(new GetNotificationNumber(id))).Result;

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response));
        }
    }
}