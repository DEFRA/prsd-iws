namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using Core.Movement;
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Complete;
    using Requests.Movement.Receive;
    using Requests.Movement.Reject;
    using Requests.Movement.Summary;
    using Requests.Notification;
    using Requests.NotificationMovements.Capture;
    using Requests.NotificationMovements.Create;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.CaptureMovement;

    [AuthorizeActivity(typeof(CreateMovementInternal))]
    public class CaptureMovementController : Controller
    {
        private readonly IMediator mediator;

        public CaptureMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Create(Guid id, int? shipmentNumber = null)
        {
            var model = new CaptureViewModel();

            if (shipmentNumber.HasValue)
            {
                model.ShipmentNumber = shipmentNumber;
            }

            model.NotificationType = await mediator.SendAsync(new GetNotificationType(id));
            model.Recovery.NotificationType = model.NotificationType;

            //Set the units based on the notification Id  
            var units = await mediator.SendAsync(new GetShipmentUnits(id));
            model.Receipt.PossibleUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(units).ToArray();
            //floating summary
            await UpdateSummary(model, id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, CaptureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await UpdateSummary(model, id);
                return View(model);
            }

            //Check if shipment number exists
            var movementId =
                await mediator.SendAsync(new GetMovementIdIfExists(id, model.ShipmentNumber.Value));

            if (!movementId.HasValue)
            {
                movementId = await mediator.SendAsync(new CreateMovementInternal(id,
                    model.ShipmentNumber.Value,
                    model.PrenotificationDate.Date,
                    model.ActualShipmentDate.Date.Value,
                    model.HasNoPrenotification));

                if (movementId.HasValue)
                {
                    await SaveMovementData(movementId.Value, model);

                    return RedirectToAction("Edit", new { movementId, saved = true });
                }
            }
            else
            {
                ModelState.AddModelError("Number", CaptureMovementControllerResources.NumberExists);
            }

            ModelState.AddModelError("Number", CaptureMovementControllerResources.SaveUnsuccessful);
            await UpdateSummary(model, id);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid movementId, bool saved = false)
        {
            ViewBag.IsSaved = saved;
            var result = await mediator.SendAsync(new GetMovementReceiptAndRecoveryData(movementId));

            if (result.Status == MovementStatus.Cancelled)
            {
                return RedirectToAction("Cancelled", new { id });
            }

            var model = new CaptureViewModel(result);
            await UpdateSummary(model, id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, Guid movementId, CaptureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IsSaved = false;
                await UpdateSummary(model, id);
                return View(model);
            }

            await SaveMovementData(movementId, model);

            return RedirectToAction("Edit", new { movementId, saved = true });
        }

        private async Task SaveMovementData(Guid movementId, CaptureViewModel model)
        {
            if (model.Receipt.IsComplete() && !model.IsReceived && !model.IsRejected)
            {
                if (!model.Receipt.WasShipmentAccepted)
                {
                    await mediator.SendAsync(new RecordRejectionInternal(movementId,
                        model.Receipt.ReceivedDate.Date.Value,
                        model.Receipt.RejectionReason));
                }
                else
                {
                    await mediator.SendAsync(new RecordReceiptInternal(movementId,
                        model.Receipt.ReceivedDate.Date.Value,
                        model.Receipt.ActualQuantity.Value,
                        model.Receipt.Units.Value));
                }
            }

            if (model.Recovery.IsComplete()
                && (model.Receipt.IsComplete() || model.IsReceived)
                && !model.IsOperationCompleted
                && !model.IsRejected
                && model.Receipt.WasShipmentAccepted)
            {
                await mediator.SendAsync(new RecordOperationCompleteInternal(movementId,
                    model.Recovery.RecoveryDate.Date.Value));
            }

            if (model.HasComments)
            {
                await mediator.SendAsync(new SetMovementComments(movementId)
                {
                    Comments = model.Comments,
                    StatsMarking = model.StatsMarking
                });
            }
        }

        [HttpGet]
        public ActionResult Cancelled(Guid id)
        {
            return View(id);
        }

        private async Task UpdateSummary(CaptureViewModel model, Guid id)
        {
            var summary = await mediator.SendAsync(new GetInternalMovementSummary(id));
            model.SetSummaryData(summary);
        }
    }
}