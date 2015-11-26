namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;
    using Requests.Movement.Reject;
    using Requests.Movement.Summary;
    using ViewModels.InternalCapture;

    [Authorize(Roles = "internal")]
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
                        model.Receipt.RejectionReason,
                        model.Receipt.RejectionFurtherInformation));
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
                && !model.IsOperationCompleted)
            {
                throw new InvalidOperationException();
            }

            return RedirectToAction("Index", "ShipmentSummary", new { area = "NotificationAssessment", id = model.NotificationId });
        }
    }
}