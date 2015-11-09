namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using ViewModels.ShipmentSummary;

    [Authorize]
    public class ShipmentSummaryController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentSummaryController(IMediator mediator)
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
    }
}