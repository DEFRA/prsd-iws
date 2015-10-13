namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Summary;
    using ViewModels.Movement;

    [Authorize]
    public class NotificationMovementController : Controller
    {
        private readonly IMediator mediator;

        public NotificationMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var model = new MovementsViewModel
            {
                NotificationId = id,
                Movements = await mediator.SendAsync(new GetMovementsForNotificationById(id))
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Operation(Guid id)
        {
            var result =
                await
                    mediator.SendAsync(new GetActiveMovementsWithReceiptCertificateByNotificationId(id));

            return View(new MovementOperationViewModel(id, result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Operation(Guid id, MovementOperationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "DateComplete",
                new
                {
                    id = model.RadioButtons.SelectedValue,
                    area = "Movement"
                });
        }

        [HttpGet]
        public async Task<ActionResult> Receipt(Guid id)
        {
            var result =
                await
                    mediator.SendAsync(
                        new GetActiveMovementsWithoutReceiptCertificateByNotificationId(id));

            return View(new MovementReceiptViewModel(id, result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Receipt(Guid id, MovementReceiptViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "DateReceived",
                new
                {
                    id = model.RadioButtons.SelectedValue,
                    area = "Movement"
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id)
        {
            var movementId = await mediator.SendAsync(new CreateMovementForNotificationById(id));

            return RedirectToAction("Index", "ShipmentDate", new { id = movementId, area = "Movement" });
        }

        [HttpGet]
        public async Task<ActionResult> Summary(Guid id)
        {
            var movementsSummary = await mediator.SendAsync(new GetMovementsSummaryByNotificationId(id));

            var model = new MovementSummaryViewModel
            {
                NotificationId = id,
                NotificationNumber = movementsSummary.NotificationNumber,
                NotificationType = movementsSummary.NotificationType,
                IntendedShipments = movementsSummary.IntendedShipments,
                UsedShipments = movementsSummary.UsedShipments,
                QuantityIntendedTotal = movementsSummary.IntendedQuantityTotal.ToString("G29") + " " + EnumHelper.GetDisplayName(movementsSummary.DisplayUnits),
                QuantityReceivedTotal = movementsSummary.ReceivedQuantityTotal.ToString("G29") + " " + EnumHelper.GetDisplayName(movementsSummary.DisplayUnits),
                ActiveLoadsPermitted = movementsSummary.ActiveLoadsPermitted,
                ActiveLoadsCurrent = movementsSummary.ActiveLoadsCurrent
            };

            return View(model);
        }
    }
}