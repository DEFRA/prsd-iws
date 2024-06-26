﻿namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using Areas.AdminImportNotificationMovements.ViewModels.MovementOverride;
    using Core.ImportMovement;
    using Core.Movement;
    using EA.Iws.Core.Authorization.Permissions;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    using Requests.ImportMovement.Capture;
    using Requests.ImportNotificationMovements;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    [AuthorizeActivity(UserAdministrationPermissions.CanOverrideShipmentData)]
    public class MovementOverrideController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        public MovementOverrideController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }
        public async Task<ActionResult> Index(Guid id, Guid movementId)
        {
            var result = await mediator.SendAsync(new GetImportMovementReceiptAndRecoveryData(movementId));
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

            var data = new ImportMovementSummaryData
            {
                Data = new ImportMovementData
                {
                    NotificationId = id,
                    ActualDate = model.ActualShipmentDate.Value,
                    PreNotificationDate = model.PrenotificationDate.HasValue ? model.PrenotificationDate.Value : (DateTime?)null
                },
              ReceiptData = new ImportMovementReceiptData
              {
                  ReceiptDate = (!model.IsRejected) && model.ReceivedDate.HasValue ? model.ReceivedDate.Value : (DateTime?)null,
                  ActualQuantity = model.ActualQuantity,
                  ReceiptUnits = model.Units,
                  RejectionReason = model.RejectionReason,
              },                
                HasNoPrenotification = model.PrenotificationDate.HasValue ? false : true,
                RejectionDate = (model.IsRejected) && model.ReceivedDate.HasValue ? model.ReceivedDate.Value : (DateTime?)null,
                RecoveryData = new ImportMovementRecoveryData
                {
                    OperationCompleteDate = model.Date.HasValue ? model.Date.Value : (DateTime?)null
                },
                Comments = model.Comments,
                StatsMarking = model.StatsMarking,
                IsRejected = model.IsRejected,
                MovementId = movementId,
                RejectedQuantity = model.RejectedQuantity,
                RejectedUnit = model.RejectedUnits
            };

            await mediator.SendAsync(new SetImportMovementReceiptAndRecoveryData(data));

            await this.auditService.AddImportMovementAudit(this.mediator,
                id, model.ShipmentNumber,
                User.GetUserId(),
                MovementAuditType.Edited);

            return RedirectToAction("Edit", "Capture", new { movementId });
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
                    return RedirectToAction("Index", new { id, movementId });
                }
                else
                {
                    return RedirectToAction("Create", "Capture", new { id, shipmentNumber = newShipmentNumber.Value });
                }
            }
            else
            {
                if (shipmentNumber.HasValue)
                {
                    var movementId = await mediator.SendAsync(new GetImportMovementIdIfExists(id, shipmentNumber.Value));
                    if (movementId.HasValue)
                    {
                        return RedirectToAction("Index", new { movementId = movementId.Value });
                    }
                }

                return RedirectToAction("Create", "Capture", new { id });
            }
        }

        private async Task UpdateSummary(IndexViewModel model, Guid id)
        {
            var summary = await mediator.SendAsync(new GetImportMovementsSummary(id));
            model.SetSummaryData(summary);
        }
    }
}