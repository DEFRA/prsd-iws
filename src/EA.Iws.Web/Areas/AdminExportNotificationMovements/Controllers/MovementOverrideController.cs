namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Summary;
    using Requests.NotificationMovements.Capture;
    using Requests.NotificationMovements.Create;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.MovementOverride;

    [AuthorizeActivity(UserAdministrationPermissions.CanOverrideShipmentData)]
    public class MovementOverrideController : Controller
    {
        private readonly IMediator mediator;

        public MovementOverrideController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<ActionResult> Index(Guid id, Guid movementId)
        {
            var result = await mediator.SendAsync(new GetMovementReceiptAndRecoveryData(movementId));
            var model = new IndexViewModel(result);
            await UpdateSummary(model, id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, Guid movementId, IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await UpdateSummary(model, id);
                return View(model);
            }

            var data = new MovementReceiptAndRecoveryData
            {
                Id = movementId,
                NotificationId = id,
                ActualDate = model.ActualShipmentDate.Value,
                HasNoPrenotification = model.PrenotificationDate.HasValue ? false : true,
                PrenotificationDate = model.PrenotificationDate.HasValue ? model.PrenotificationDate.Value : (DateTime?)null,
                ReceiptDate = (!model.IsRejected) && model.ReceivedDate.HasValue ? model.ReceivedDate.Value : (DateTime?)null,
                ActualQuantity = model.ActualQuantity,
                ReceiptUnits = model.Units,
                RejectionDate = (model.IsRejected) && model.ReceivedDate.HasValue ? model.ReceivedDate.Value : (DateTime?)null,
                OperationCompleteDate = model.Date.HasValue ? model.Date.Value : (DateTime?)null,
                RejectionReason = model.RejectionReason,
                Comments = model.HasComments ? model.Comments : null,
                StatsMarking = model.HasComments ? model.StatsMarking : null,
                IsRejected = model.IsRejected
            };

            await mediator.SendAsync(new SetMovementReceiptAndRecoveryData(data));

            return RedirectToAction("Edit", "CaptureMovement",  new { movementId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeShipment(Guid id, int? shipmentNumber = null, int? newShipmentNumber = null)
        {
            if (newShipmentNumber.HasValue)
            {
                var movementId = await mediator.SendAsync(new GetMovementIdIfExists(id, newShipmentNumber.Value));

                if (movementId.HasValue)
                {
                    return RedirectToAction("Index", new { id, movementId });
                }
                else
                {
                    return RedirectToAction("Create", "CaptureMovement", new { id, shipmentNumber = newShipmentNumber.Value });
                }
            }
            else
            {
                if (shipmentNumber.HasValue)
                {
                    var movementId = await mediator.SendAsync(new GetMovementIdIfExists(id, shipmentNumber.Value));
                    if (movementId.HasValue)
                    {
                        return RedirectToAction("Index", new { id = id, movementId = movementId.Value });
                    }
                }
                return RedirectToAction("Create", "CaptureMovement", new { id });
            }
        }

        private async Task UpdateSummary(IndexViewModel model, Guid id)
        {
            var summary = await mediator.SendAsync(new GetInternalMovementSummary(id));
            model.SetSummaryData(summary);
        }
    }
}