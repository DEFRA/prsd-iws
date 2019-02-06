namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Rules;
    using Infrastructure;
    using Infrastructure.BulkReceiptRecovery;
    using Prsd.Core.Mediator;
    using ViewModels.ReceiptRecoveryBulkUpload;

    [Authorize]
    public class ReceiptRecoveryBulkUploadController : Controller
    {
        private readonly IMediator mediator;
        private readonly IReceiptRecoveryValidator validator;
        private readonly IFileReader fileReader;

        public ReceiptRecoveryBulkUploadController(IMediator mediator, IReceiptRecoveryValidator validator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.validator = validator;
            this.fileReader = fileReader;
        }

        [HttpGet]
        public ActionResult Index(Guid notificationId)
        {
            ViewBag.NotificationId = notificationId;

            var model = new ReceiptRecoveryBulkUploadViewModel(notificationId);
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, ReceiptRecoveryBulkUploadViewModel model)
        {
            return RedirectToAction("Upload");
        }

        [HttpGet]
        public ActionResult Upload(Guid notificationId)
        {
            var model = new ReceiptRecoveryBulkUploadViewModel(notificationId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Guid notificationId, ReceiptRecoveryBulkUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = notificationId;
                model = new ReceiptRecoveryBulkUploadViewModel(notificationId);
                return View(model);
            }

            var validationSummary = await validator.GetValidationSummary(model.File, notificationId);
            var failedFileRules = validationSummary.FileRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).Select(r => r.Rule).ToList();
            var failedContentRules = validationSummary.ContentRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).ToList();

            model.FailedFileRules = failedFileRules;
            model.FailedContentRules = failedContentRules;

            if (model.ErrorsCount > 0)
            {
                return View("Errors", model);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Success(Guid notificationId)
        {
            var fileName = string.Empty;
            var shipments = new List<int>();
            var shipmentMovementFileName = string.Empty;

            // TODO: Uncomment and update these comments when the DocumentUpload page is added
            /*object fileNameObj;
            object shipmentsObj;
            object shipmentDocumentNameObj;

            
            if (TempData.TryGetValue("PreNotificationFileName", out fileNameObj))
            {
                fileName = fileNameObj as string;
            }
            if (TempData.TryGetValue("PrenotificationShipments", out shipmentsObj))
            {
                shipments = shipmentsObj as List<int>;
            }
            if (TempData.TryGetValue("ShipmentMovementFileName", out shipmentDocumentNameObj))
            {
                shipmentMovementFileName = shipmentDocumentNameObj as string;
            }*/

            var model = new ShipmentMovementDocumentsViewModel(notificationId, shipments, fileName);

            model.ShipmentMovementFileName = shipmentMovementFileName;

            return View(model);
        }

        [HttpGet]
        public ActionResult Warning(Guid notificationId)
        {
            var model = new WarningChoiceViewModel(notificationId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Warning(WarningChoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.GetEnumDisplayValue(WarningChoicesList.Leave).Equals(model.WarningChoices.SelectedValue))
            {
                TempData.Remove("ReceiptRecoveryShipments");
                TempData.Remove("ReceiptRecoveryFileName");
                TempData.Remove("DraftBulkUploadId");

                return RedirectToAction("Index", "Options", new { area = "NotificationApplication", id = model.NotificationId });
            }
            if (model.GetEnumDisplayValue(WarningChoicesList.Return).Equals(model.WarningChoices.SelectedValue))
            {
                return RedirectToAction("Documents");
            }

            return View(model);
        }
    }
}