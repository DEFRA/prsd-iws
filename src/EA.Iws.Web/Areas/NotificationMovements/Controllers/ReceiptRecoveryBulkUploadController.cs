namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Rules;
    using Infrastructure;
    using Infrastructure.BulkReceiptRecovery;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationMovements.BulkUpload;
    using ViewModels.ReceiptRecoveryBulkUpload;

    [Authorize]
    public class ReceiptRecoveryBulkUploadController : Controller
    {
        private readonly IMediator mediator;
        private readonly IReceiptRecoveryValidator validator;
        private readonly IFileReader fileReader;

        public ReceiptRecoveryBulkUploadController(IMediator mediator, 
            IReceiptRecoveryValidator validator, 
            IFileReader fileReader)
        {
            this.mediator = mediator;
            this.validator = validator;
            this.fileReader = fileReader;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            ViewBag.NotificationId = notificationId;
            var data = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));

            var model = new ReceiptRecoveryBulkUploadViewModel(notificationId, data.NotificationType);
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, ReceiptRecoveryBulkUploadViewModel model)
        {
            return RedirectToAction("Upload");
        }

        [HttpGet]
        public async Task<ActionResult> Upload(Guid notificationId)
        {
            var data = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));

            var model = new ReceiptRecoveryBulkUploadViewModel(notificationId, data.NotificationType);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Guid notificationId, ReceiptRecoveryBulkUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = notificationId;
                model = new ReceiptRecoveryBulkUploadViewModel(notificationId, model.NotificationType);
                return View(model);
            }

            var validationSummary = await validator.GetValidationSummary(model.File, notificationId);
            var failedFileRules = validationSummary.FileRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).Select(r => r.Rule).ToList();
            var failedContentRules = validationSummary.ContentRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).ToList();

            model.FailedFileRules = failedFileRules;
            model.FailedContentRules = failedContentRules;

            var shipments = validationSummary.ShipmentNumbers != null ? validationSummary.ShipmentNumbers.ToList() : null;

            var shipmentsModel = new ShipmentMovementDocumentsViewModel(notificationId, shipments, model.File.FileName);

            if (model.ErrorsCount > 0)
            {
                return View("Errors", model);
            }

            TempData["PrenotificationShipments"] = shipments;
            TempData["PreNotificationFileName"] = model.File.FileName;

            TempData["DraftBulkUploadId"] = validationSummary.DraftBulkUploadId;

            return View("Documents", shipmentsModel);
        }

        [HttpGet]
        public ActionResult Documents(Guid notificationId)
        {
            var fileName = string.Empty;
            var shipments = new List<int>();
            object fileNameObj;
            object shipmentsObj;

            if (TempData.TryGetValue("PreNotificationFileName", out fileNameObj))
            {
                fileName = fileNameObj as string;
            }
            if (TempData.TryGetValue("PrenotificationShipments", out shipmentsObj))
            {
                shipments = shipmentsObj as List<int>;
            }

            TempData["PrenotificationShipments"] = shipments;
            TempData["PreNotificationFileName"] = fileName;

            var model = new ShipmentMovementDocumentsViewModel(notificationId, shipments, fileName);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Documents(Guid notificationId, ShipmentMovementDocumentsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Warning");
            }

            var validationSummary = await validator.GetShipmentMovementValidationSummary(model.File, notificationId);
            var failedFileRules = validationSummary.FileRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).Select(r => r.Rule).ToList();

            if (failedFileRules.Count > 0)
            {
                foreach (var rule in failedFileRules)
                {
                    ModelState.AddModelError(string.Empty, Prsd.Core.Helpers.EnumHelper.GetDisplayName(rule));
                }

                var fileName = string.Empty;
                var shipments = new List<int>();
                object fileNameObj;
                object shipmentsObj;

                if (TempData.TryGetValue("PreNotificationFileName", out fileNameObj))
                {
                    fileName = fileNameObj as string;
                }
                if (TempData.TryGetValue("PrenotificationShipments", out shipmentsObj))
                {
                    shipments = shipmentsObj as List<int>;
                }

                TempData["PrenotificationShipments"] = shipments;
                TempData["PreNotificationFileName"] = fileName;

                model.ReceiptRecoveryFileName = fileName;
                model.Shipments = shipments;

                return View(model);
            }

            object draftBulkUploadIdObj;

            if (TempData.TryGetValue("DraftBulkUploadId", out draftBulkUploadIdObj))
            {
                var draftBulkUploadId = draftBulkUploadIdObj as Guid?;

                if (draftBulkUploadId != null && draftBulkUploadId != Guid.Empty)
                {
                    var fileExtension = Path.GetExtension(model.File.FileName);
                    var uploadedFile = await fileReader.GetFileBytes(model.File);

                    await
                        mediator.SendAsync(new CreateReceiptRecovery(notificationId, draftBulkUploadId.Value,
                            uploadedFile, fileExtension));

                    TempData["ShipmentMovementFileName"] = model.File.FileName;
                    return RedirectToAction("Success");
                }
            }

            return RedirectToAction("Success");
        }

        [HttpGet]
        public ActionResult Success(Guid notificationId)
        {
            var fileName = string.Empty;
            var shipments = new List<int>();
            var shipmentMovementFileName = string.Empty;

            object fileNameObj;
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
            }

            var model = new ShipmentMovementDocumentsViewModel(notificationId, shipments, fileName);

            model.ShipmentMovementFileName = shipmentMovementFileName;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Warning(Guid notificationId)
        {
            var data = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));
            var model = new WarningChoiceViewModel(notificationId, data.NotificationType);

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