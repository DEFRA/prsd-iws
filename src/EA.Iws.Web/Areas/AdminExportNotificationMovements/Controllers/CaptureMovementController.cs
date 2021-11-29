namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Core.Shared;
    using EA.Iws.Requests.Movement.PartialReject;
    using Infrastructure;
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
    using ViewModels.CaptureMovement;

    [AuthorizeActivity(typeof(CreateMovementInternal))]
    public class CaptureMovementController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public CaptureMovementController(IMediator mediator, AuthorizationService authorizationService, IAuditService auditService)
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

                await this.auditService.AddMovementAudit(this.mediator,
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
            model.ShowShipmentDatesOverride = await CanShowEditLink();
            model.NotificationId = id;
            model.MovementId = movementId;
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
                if (model.Receipt.ActualQuantity != null && model.Receipt.ReceivedDate.Date != null && !model.Recovery.IsComplete())
                {
                    await mediator.SendAsync(new RecordReceiptInternal(movementId,
                        model.Receipt.ReceivedDate.Date.Value,
                        model.Receipt.ActualQuantity.Value,
                        model.Receipt.ActualUnits.Value));

                    await this.auditService.AddMovementAudit(this.mediator,
                        notificationId, model.ShipmentNumber.Value,
                        User.GetUserId(),
                        MovementAuditType.Received);
                }
            }
            else if (model.Receipt.ShipmentTypes == ShipmentType.Rejected)
            {
                await mediator.SendAsync(new RecordRejectionInternal(movementId,
                    model.Receipt.ReceivedDate.Date.Value,
                    model.Receipt.RejectionReason,
                    model.Receipt.RejectedQuantity,
                    model.Receipt.RejectedUnits));

                await this.auditService.AddMovementAudit(this.mediator,
                    notificationId, model.ShipmentNumber.Value,
                    User.GetUserId(),
                    MovementAuditType.Rejected);
            }
            else
            {
                await mediator.SendAsync(new RecordPartialRejectionInternal(movementId,
                                                                            model.Receipt.ReceivedDate.Date.Value,
                                                                            model.Receipt.RejectionReason,
                                                                            model.Receipt.ActualQuantity.Value,
                                                                            model.Receipt.ActualUnits.Value,
                                                                            model.Receipt.RejectedQuantity.Value,
                                                                            model.Receipt.RejectedUnits.Value,
                                                                            model.Recovery.RecoveryDate.Date.Value));

                await this.auditService.AddMovementAudit(this.mediator,
                                                         notificationId,
                                                         model.ShipmentNumber.Value,
                                                         User.GetUserId(),
                                                         MovementAuditType.PartiallyRejected);
            }

            if (model.Recovery.IsComplete() && !model.IsOperationCompleted && model.Receipt.ShipmentTypes == ShipmentType.Accepted)
            {
                await mediator.SendAsync(new RecordOperationCompleteInternal(movementId,
                    model.Recovery.RecoveryDate.Date.Value));

                await this.auditService.AddMovementAudit(this.mediator,
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
                var movementId = await mediator.SendAsync(new GetMovementIdIfExists(id, newShipmentNumber.Value));

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
                    var movementId = await mediator.SendAsync(new GetMovementIdIfExists(id, shipmentNumber.Value));
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
            var summary = await mediator.SendAsync(new GetInternalMovementSummary(id));
            model.SetSummaryData(summary);
        }

        private async Task<bool> CanShowEditLink()
        {
            var showUpdateLink = await authorizationService.AuthorizeActivity(UserAdministrationPermissions.CanOverrideShipmentData);

            return showUpdateLink;
        }
    }
}