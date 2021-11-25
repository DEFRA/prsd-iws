namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Core.Shared;
    using EA.Iws.Requests.ImportMovement.PartialReject;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    using Requests.ImportMovement.Capture;
    using Requests.ImportMovement.CompletedReceipt;
    using Requests.ImportMovement.Receipt;
    using Requests.ImportMovement.Reject;
    using Requests.ImportNotification;
    using Requests.ImportNotificationMovements;
    using ViewModels.Capture;

    [AuthorizeActivity(typeof(CreateImportMovement))]
    public class CaptureController : Controller
    {
        private readonly IMediator mediator;
        private readonly AuthorizationService authorizationService;
        private readonly IAuditService auditService;

        public CaptureController(IMediator mediator, AuthorizationService authorizationService, IAuditService auditService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
            this.auditService = auditService;
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

                await this.auditService.AddImportMovementAudit(this.mediator,
                   id, model.ShipmentNumber.Value,
                   User.GetUserId(),
                   model.HasNoPrenotification == true ? MovementAuditType.NoPrenotificationReceived : MovementAuditType.Prenotified);

                if (movementId.HasValue)
                {
                    await SaveMovementData(movementId.Value, model, id);

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
            model.ShowShipmentDataOverride = await CanShowEditLink();
            model.MovementId = movementId;
            model.NotificationId = id;
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

            await SaveMovementData(movementId, model, id);

            return RedirectToAction("Edit", new { movementId, saved = true });
        }

        private async Task SaveMovementData(Guid movementId, CaptureViewModel model, Guid notificationId)
        {
            if (model.Receipt.ShipmentTypes == ShipmentType.Accepted)
            {
                await mediator.SendAsync(new RecordReceipt(movementId,
                    model.Receipt.ReceivedDate.Date.Value,
                    model.Receipt.ActualUnits.Value,
                    model.Receipt.ActualQuantity.Value));

                await this.auditService.AddImportMovementAudit(this.mediator,
                    notificationId, model.ShipmentNumber.Value,
                    User.GetUserId(),
                    MovementAuditType.Received);
            }
            else if (model.Receipt.ShipmentTypes == ShipmentType.Rejected)
            {
                await mediator.SendAsync(new RecordRejection(movementId,
                    model.Receipt.ReceivedDate.Date.Value,
                    model.Receipt.RejectionReason,
                    model.Receipt.RejectedQuantity.Value,
                    model.Receipt.RejectedUnits.Value));

                await this.auditService.AddImportMovementAudit(this.mediator,
                    notificationId, model.ShipmentNumber.Value,
                    User.GetUserId(),
                    MovementAuditType.Rejected);
            }
            else
            {
                await mediator.SendAsync(new RecordPartialRejection(movementId,
                                                                            model.Receipt.ReceivedDate.Date.Value,
                                                                            model.Receipt.RejectionReason,
                                                                            model.Receipt.ActualQuantity.Value,
                                                                            model.Receipt.ActualUnits.Value,
                                                                            model.Receipt.RejectedQuantity.Value,
                                                                            model.Receipt.RejectedUnits.Value));

                await this.auditService.AddImportMovementAudit(this.mediator,
                                                         notificationId,
                                                         model.ShipmentNumber.Value,
                                                         User.GetUserId(),
                                                         MovementAuditType.PartiallyRejected);
            }

            if (model.Recovery.IsComplete()
                && (model.Receipt.IsComplete() || model.IsReceived)
                && !model.IsOperationCompleted
                && !model.IsRejected
                && model.Receipt.WasAccepted)
            {
                await mediator.SendAsync(new RecordCompletedReceipt(movementId,
                    model.Recovery.RecoveryDate.Date.Value));

                await this.auditService.AddImportMovementAudit(this.mediator,
                    notificationId, model.ShipmentNumber.Value,
                    User.GetUserId(),
                    model.NotificationType == NotificationType.Disposal ? MovementAuditType.Disposed : MovementAuditType.Recovered);
            }

            if (model.HasComments)
            {
                await mediator.SendAsync(new SetMovementComments(movementId)
                {
                    Comments = model.Comments,
                    StatsMarking = model.StatsMarking
                });
            }
            else if (!string.IsNullOrEmpty(model.StatsMarking))
            {
                await mediator.SendAsync(new SetMovementComments(movementId)
                {
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

        private async Task<bool> CanShowEditLink()
        {
            var showUpdateLink = await authorizationService.AuthorizeActivity(UserAdministrationPermissions.CanOverrideShipmentData);

            return showUpdateLink;
        }
    }
}