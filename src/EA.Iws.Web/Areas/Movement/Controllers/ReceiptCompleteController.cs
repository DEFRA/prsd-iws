namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.MovementReceipt;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.MovementReceipt;
    using ViewModels;

    public class ReceiptCompleteController : Controller
    {
        private readonly IMediator mediator;
        private const string DateReceivedKey = "DateReceived";
        private const string DecisionKey = "Decision";
        private const string UnitKey = "Unit";
        private const string QuantityKey = "Quantity";
        private const string RejectionReasonKey = "RejectionReason";

        public ReceiptCompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            object dateReceivedResult;
            object decisionResult;
            object unitResult;
            object quantityResult;
            object rejectionReasonResult;

            bool allDataPresent = false;
            var model = new ReceiptCompleteViewModel();

            if (TempData.TryGetValue(DecisionKey, out decisionResult))
            {
                if ((Decision)decisionResult == Decision.Accepted)
                {
                    if (TempData.TryGetValue(DateReceivedKey, out dateReceivedResult)
                        && TempData.TryGetValue(UnitKey, out unitResult)
                        && TempData.TryGetValue(QuantityKey, out quantityResult))
                    {
                        allDataPresent = true;

                        model.DateReceived = DateTime.Parse(dateReceivedResult.ToString());
                        model.Decision = (Decision)decisionResult;
                        model.Unit = (ShipmentQuantityUnits)unitResult;
                        model.Quantity = (decimal)quantityResult;
                    }
                }

                if ((Decision)decisionResult == Decision.Rejected)
                {
                    if (TempData.TryGetValue(DateReceivedKey, out dateReceivedResult)
                        && TempData.TryGetValue(RejectionReasonKey, out rejectionReasonResult))
                    {
                        allDataPresent = true;

                        model.DateReceived = DateTime.Parse(dateReceivedResult.ToString());
                        model.Decision = (Decision)decisionResult;
                        model.RejectionReason = rejectionReasonResult.ToString();
                    }
                }
            }

            if (allDataPresent)
            {
                var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
                model.NotificationId = notificationId;

                return View(model);
            }

            return RedirectToAction("Index", "DateReceived", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ReceiptCompleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

            var fileId = await mediator.SendAsync(new SaveCertificateOfReceiptFile(id, uploadedFile, fileExtension));

            if (model.Decision == Decision.Accepted)
            {
                await mediator.SendAsync(new SetMovementAccepted(id, fileId, model.DateReceived, model.Quantity.GetValueOrDefault()));
            }

            if (model.Decision == Decision.Rejected)
            {
                await mediator.SendAsync(new SetMovementRejected(id, fileId, model.DateReceived, model.RejectionReason));
            }

            return RedirectToAction("Success", "ReceiptComplete", new { id = model.NotificationId, area = "Movement" });
        }

        [HttpGet]
        public ActionResult Success(Guid id)
        {
            var model = new SuccessViewModel
            {
                NotificationId = id
            };
            return View(model);
        }
    }
}