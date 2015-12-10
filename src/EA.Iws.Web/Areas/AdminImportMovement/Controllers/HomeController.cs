namespace EA.Iws.Web.Areas.AdminImportMovement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using Requests.ImportMovement.CompletedReceipt;
    using Requests.ImportMovement.Receipt;
    using Requests.ImportMovement.Reject;
    using ViewModels.Home;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetImportMovementReceiptAndRecoveryData(id));

            return View(new HomeViewModel(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, HomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Receipt.IsComplete() && !model.IsReceived)
            {
                if (!model.Receipt.WasAccepted)
                {
                    await mediator.SendAsync(new RecordRejection(id,
                        model.Receipt.ReceivedDate.AsDateTime().Value,
                        model.Receipt.RejectionReason,
                        model.Receipt.RejectionFurtherInformation));
                }
                else
                {
                    await mediator.SendAsync(new RecordReceipt(id,
                        model.Receipt.ReceivedDate.AsDateTime().Value,
                        model.Receipt.Units.Value,
                        model.Receipt.ActualQuantity.Value));
                }
            }

            if (model.Recovery.IsComplete()
                && (model.Receipt.IsComplete() || model.IsReceived)
                && !model.IsOperationCompleted
                && model.Receipt.WasAccepted)
            {
                await mediator.SendAsync(new RecordCompletedReceipt(id,
                    model.Recovery.RecoveryDate.AsDateTime().Value));
            }

            return RedirectToAction("Index", "Home", new { area = "AdminImportNotificationMovements", id = model.NotificationId });
        }
    } 
}