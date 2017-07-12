namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Delete;
    using Requests.ImportNotification;
    using Requests.ImportNotificationMovements;
    using ViewModels.Home;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(GetImportMovementsSummary))]
    [AuthorizeActivity(typeof(GetImportMovementsSummaryTable))]
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
        public async Task<ActionResult> Index(Guid id, int page = 1)
        {
            var movementData = await mediator.SendAsync(new GetImportMovementsSummary(id));
            var tableData = await mediator.SendAsync(new GetImportMovementsSummaryTable(id, page));
            var canDeleteMovement = await authorizationService.AuthorizeActivity(typeof(DeleteMovement));

            var model = new MovementSummaryViewModel(movementData, tableData);

            model.CanDeleteMovement = canDeleteMovement;

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = Task.Run(() => mediator.SendAsync(new GetNotificationDetails(id))).Result;

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response.NotificationNumber));
        }
    }
}