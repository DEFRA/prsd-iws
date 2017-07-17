namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Complete;
    using Requests.Movement.Receive;
    using Requests.Movement.Reject;
    using Requests.Notification;
    using Requests.NotificationMovements.Capture;
    using Requests.NotificationMovements.Create;
    using ViewModels.CaptureMovement;

    [AuthorizeActivity(typeof(CreateMovementInternal))]
    public class CaptureMovementController : Controller
    {
        private const string MovementNumberKey = "MovementNumberKey";
        private readonly IMediator mediator;

        public CaptureMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Create(Guid id)
        {
            var model = new CreateViewModel();            
            model.LatestCurrentMovementNumber = await mediator.SendAsync(new GetLatestMovementNumber(id));
            model.NotificationType = await mediator.SendAsync(new GetNotificationType(id));
            model.NotificationId = id;

            //Set the units based on the notification Id  
           var units = await mediator.SendAsync(new GetShipmentUnits(id));
           model.Receipt.PossibleUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(units).ToArray();
            
            return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var movementId =
                await mediator.SendAsync(new GetMovementIdIfExists(id, model.Number.Value));

            if (!movementId.HasValue)
            {
                TempData[MovementNumberKey] = model.Number;
                return RedirectToAction("Create");
            }

            return RedirectToAction("Index", "InternalCapture", new { area = "AdminExportMovement", id = movementId.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Check if shipment number exists
            var movementId =
                await mediator.SendAsync(new GetMovementIdIfExists(id, model.Number.Value));

            if (!movementId.HasValue)
            {
                var success = await mediator.SendAsync(new CreateMovementInternal(id,
                    model.Number.Value,
                    model.PrenotificationDate.AsDateTime(),
                    model.ActualShipmentDate.AsDateTime().Value,
                    model.HasNoPrenotification));

                if (success)
                {
                     movementId = await mediator.SendAsync(new GetMovementIdByNumber(id, model.Number.Value));

                    model.IsSaved = true;
                    if (model.Receipt.IsComplete() && !model.IsReceived)
                    {
                        if (!model.Receipt.WasShipmentAccepted)
                        {
                            await mediator.SendAsync(new RecordRejectionInternal(movementId.Value,
                                model.Receipt.ReceivedDate.AsDateTime().Value,
                                model.Receipt.RejectionReason,
                                model.Receipt.RejectionFurtherInformation));
                        }
                        else
                        {
                            await mediator.SendAsync(new RecordReceiptInternal(movementId.Value,
                                model.Receipt.ReceivedDate.AsDateTime().Value,
                                model.Receipt.ActualQuantity.Value,
                                model.Receipt.Units.Value));
                        }
                    }

                    if (model.Recovery.IsComplete()
                        && (model.Receipt.IsComplete() || model.IsReceived)
                        && !model.IsOperationCompleted
                        && model.Receipt.WasShipmentAccepted)
                    {
                        await mediator.SendAsync(new RecordOperationCompleteInternal(movementId.Value,
                            model.Recovery.RecoveryDate.AsDateTime().Value));
                    }

                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("Number", CaptureMovementControllerResources.NumberExists);
            }
            ModelState.AddModelError("Number", CaptureMovementControllerResources.SaveUnsuccessful);

            return View(model);
        }
    }
}