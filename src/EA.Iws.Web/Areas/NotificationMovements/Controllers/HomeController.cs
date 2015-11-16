namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using ViewModels.Home;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId, int? status)
        {
            var movementsSummary =
                await mediator.SendAsync(new GetSummaryAndTable(notificationId, (MovementStatus?)status));

            var model = new MovementSummaryViewModel(notificationId, movementsSummary);
            model.SelectedMovementStatus = (MovementStatus?)status;

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Summary(Guid notificationId)
        {
            var result = mediator
                .SendAsync(new GetBasicMovementSummary(notificationId))
                .GetAwaiter()
                .GetResult();

            return PartialView("_Summary", result);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateSummary(Guid notificationId, int movementNumber)
        {
            var result = mediator
                .SendAsync(new GetBasicMovementSummary(notificationId))
                .GetAwaiter()
                .GetResult();

            var model = new CreateSummaryViewModel
            {
                SummaryData = result,
                NewShipmentNumber = movementNumber
            };

            return PartialView("_CreateSummary", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(Guid notificationId, int? selectedMovementStatus)
        {
            return RedirectToAction("Index", new { notificationId, status = selectedMovementStatus });
        }
    }
}