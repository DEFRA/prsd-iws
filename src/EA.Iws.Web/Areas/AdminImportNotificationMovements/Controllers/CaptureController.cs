namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using Requests.ImportMovement.CompletedReceipt;
    using Requests.ImportMovement.Receipt;
    using Requests.ImportMovement.Reject;
    using Requests.ImportNotification;
    using ViewModels.Capture;
 
    [AuthorizeActivity(typeof(CreateImportMovement))]
    public class CaptureController : Controller
    {
        private const string NumberKey = "Number";
        private readonly IMediator mediator;

        public CaptureController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Create(Guid id)
        {            
            var model = new CreateViewModel();
            model.LatestCurrentMovementNumber = await mediator.SendAsync(new GetLatestMovementNumber(id));
            model.NotificationId = id;

            var result = await mediator.SendAsync(new GetNotificationDetails(id));
            model.Recovery.NotificationType = result.NotificationType;
            //Set the units based on the notification Id  
            var units = await mediator.SendAsync(new GetImportShipmentUnits(id));
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

            var result = await mediator.SendAsync(new GetImportMovementIdIfExists(id, model.Number.Value));

            if (!result.HasValue)
            {
                TempData[NumberKey] = model.Number.Value;
                return RedirectToAction("Create");
            }
            
            return RedirectToAction("Index", "Home", new { area = "AdminImportMovement", id = result.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await mediator.SendAsync(new GetImportMovementIdIfExists(id, model.ShipmentNumber.Value));

            if (!result.HasValue)
            {
                var movementId = await mediator.SendAsync(new CreateImportMovement(id,
                model.ShipmentNumber.Value,
                model.ActualShipmentDate.Date.Value,
                model.PrenotificationDate.Date));

                if (movementId != null)
                {
                    model.IsSaved = true;
                    if (model.Receipt.IsComplete() && !model.IsReceived)
                    {
                        if (!model.Receipt.WasAccepted)
                        {
                            await mediator.SendAsync(new RecordRejection(movementId,
                                model.Receipt.ReceivedDate.Date.Value,
                                model.Receipt.RejectionReason,
                                model.Receipt.RejectionFurtherInformation));
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
                        && model.Receipt.WasAccepted)
                    {
                        await mediator.SendAsync(new RecordCompletedReceipt(movementId,
                            model.Recovery.RecoveryDate.Date.Value));
                    }
                }
            }
            else
            {
                ModelState.AddModelError("Number", CaptureControllerResources.NumberExists);
            }
            return View(model);
        }
    }
}