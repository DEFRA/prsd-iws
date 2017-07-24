namespace EA.Iws.Web.Areas.AdminExportMovement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement.Complete;
    using Requests.Movement.Receive;
    using Requests.Movement.Reject;
    using Requests.Movement.Summary;
    using ViewModels.InternalCapture;

    [AuthorizeActivity(typeof(GetMovementReceiptAndRecoveryData))]
    public class InternalCaptureController : Controller
    {
        private readonly IMediator mediator;

        public InternalCaptureController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetMovementReceiptAndRecoveryData(id));

            if (result.Status == MovementStatus.Cancelled)
            {
                return RedirectToAction("Cancelled", new { id, notificationId = result.NotificationId });
            }

            var model = new CaptureViewModel(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CaptureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Receipt.IsComplete() && !model.IsReceived)
            {
                if (!model.Receipt.WasShipmentAccepted)
                {
                    await mediator.SendAsync(new RecordRejectionInternal(id,
                        model.Receipt.ReceivedDate.AsDateTime().Value,
                        model.Receipt.RejectionReason));
                }
                else
                {
                    await mediator.SendAsync(new RecordReceiptInternal(id, 
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
                await mediator.SendAsync(new RecordOperationCompleteInternal(id,
                    model.Recovery.RecoveryDate.AsDateTime().Value));
            }

            return RedirectToAction("Index", "Home", new { area = "AdminExportNotificationMovements", id = model.NotificationId });
        }

        [HttpGet]
        public ActionResult Cancelled(Guid id, Guid notificationId)
        {
            return View(notificationId);
        }
    }
}