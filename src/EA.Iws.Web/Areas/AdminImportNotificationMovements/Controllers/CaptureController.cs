namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    using Requests.ImportMovement.Capture;
    using Requests.ImportMovement.CompletedReceipt;
    using Requests.ImportMovement.Receipt;
    using Requests.ImportMovement.Reject;
    using Requests.ImportNotification;
    using Requests.ImportNotificationMovements;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Capture;

    [AuthorizeActivity(typeof(CreateImportMovement))]
    public class CaptureController : Controller
    {
        private readonly IMediator mediator;

        public CaptureController(IMediator mediator)
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
            var result = await mediator.SendAsync(new GetNotificationDetails(id));
            model.Recovery.NotificationType = result.NotificationType;
            model.NotificationType = result.NotificationType;
            //Set the units based on the notification Id  
            var units = await mediator.SendAsync(new GetImportShipmentUnits(id));
            model.Receipt.PossibleUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(units).ToArray();
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

            var movementId = await mediator.SendAsync(new GetImportMovementIdIfExists(id, model.ShipmentNumber.Value));

            if (!movementId.HasValue)
            {
                movementId = await mediator.SendAsync(new CreateImportMovement(id,
                    model.ShipmentNumber.Value,
                    model.ActualShipmentDate.Date.Value,
                    model.PrenotificationDate.Date));

                if (movementId.HasValue)
                {
                    await SaveMovementData(movementId.Value, model);

                    return RedirectToAction("Edit", new { movementId, saved = true });
                }
            }
            else
            {
                ModelState.AddModelError("Number", CaptureControllerResources.NumberExists);
            }
            await UpdateSummary(model, id);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid movementId, bool saved = false)
        {
            ViewBag.IsSaved = saved;
            var result = await mediator.SendAsync(new GetImportMovementReceiptAndRecoveryData(movementId));

            if (result.Data.IsCancelled)
            {
                return RedirectToAction("Cancelled");
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
                if (!model.Receipt.WasAccepted)
                {
                    await mediator.SendAsync(new RecordRejection(movementId,
                        model.Receipt.ReceivedDate.Date.Value,
                        model.Receipt.RejectionReason));
                }
                else
                {
                    await mediator.SendAsync(new RecordReceipt(movementId,
                        model.Receipt.ReceivedDate.Date.Value,
                        model.Receipt.Units.Value,
                        model.Receipt.ActualQuantity.Value));
                }
            }

            if (model.Recovery.IsComplete()
                && (model.Receipt.IsComplete() || model.IsReceived)
                && !model.IsOperationCompleted
                && !model.IsRejected
                && model.Receipt.WasAccepted)
            {
                await mediator.SendAsync(new RecordCompletedReceipt(movementId,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeShipment(Guid id, int? shipmentNumber = null, int? newShipmentNumber = null)
        {
            if (newShipmentNumber.HasValue)
            {
                var movementId = await mediator.SendAsync(new GetImportMovementIdIfExists(id, newShipmentNumber.Value));

                if (movementId.HasValue)
                {
                    return RedirectToAction("Edit", new { movementId });
                }
                else
                {
                    return RedirectToAction("Create", new { id, shipmentNumber = newShipmentNumber.Value });
                }
            }
            else
            {
                if (shipmentNumber.HasValue)
                {
                    var movementId = await mediator.SendAsync(new GetImportMovementIdIfExists(id, shipmentNumber.Value));
                    if (movementId.HasValue)
                    {
                        return RedirectToAction("Edit", new { movementId = movementId.Value });
                    }
                }

                return RedirectToAction("Create", new { id });
            }
        }

        private async Task UpdateSummary(CaptureViewModel model, Guid id)
        {
            var summary = await mediator.SendAsync(new GetImportMovementsSummary(id));
            model.SetSummaryData(summary);
        }
    }
}