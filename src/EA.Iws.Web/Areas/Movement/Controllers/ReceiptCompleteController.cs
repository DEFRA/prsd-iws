namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels;

    [Authorize]
    public class ReceiptCompleteController : Controller
    {
        private readonly IMediator mediator;
        private const string DateReceivedKey = "DateReceived";
        private const string UnitKey = "Unit";
        private const string QuantityKey = "Quantity";

        public ReceiptCompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            object dateReceivedResult;
            object unitResult;
            object quantityResult;

            bool allDataPresent = false;
            var model = new ReceiptCompleteViewModel();
            
            if (TempData.TryGetValue(DateReceivedKey, out dateReceivedResult)
                && TempData.TryGetValue(UnitKey, out unitResult)
                && TempData.TryGetValue(QuantityKey, out quantityResult))
            {
                allDataPresent = true;

                model.DateReceived = DateTime.Parse(dateReceivedResult.ToString());
                model.Unit = (ShipmentQuantityUnits)unitResult;
                model.Quantity = (decimal)quantityResult;
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
            
            await mediator.SendAsync(new SetMovementAccepted(id, fileId, model.DateReceived, model.Quantity.GetValueOrDefault()));

            return RedirectToAction("Success", "ReceiptComplete");
        }

        [HttpGet]
        public async Task<ActionResult> Success(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            var model = new SuccessViewModel
            {
                NotificationId = notificationId
            };

            return View(model);
        }
    }
}