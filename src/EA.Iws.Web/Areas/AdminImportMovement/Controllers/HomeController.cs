namespace EA.Iws.Web.Areas.AdminImportMovement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;
    using Requests.ImportMovement.Capture;
    using Requests.ImportMovement.CompletedReceipt;
    using Requests.ImportMovement.Receipt;
    using Requests.ImportMovement.Reject;
    using Requests.ImportNotification;
    using ViewModels.Home;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(GetImportMovementReceiptAndRecoveryData))]
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

            if (result.Data.IsCancelled)
            {
                return RedirectToAction("Cancelled", new { id, notificationId = result.Data.NotificationId });
            }

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

        [HttpGet]
        public ActionResult Cancelled(Guid id, Guid notificationId)
        {
            return View(notificationId);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var notificationId = Task.Run(() => mediator.SendAsync(new GetNotificationIdByMovementId(id))).Result;
            var response = Task.Run(() => mediator.SendAsync(new GetNotificationDetails(notificationId))).Result;

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response.NotificationNumber));
        }
    } 
}