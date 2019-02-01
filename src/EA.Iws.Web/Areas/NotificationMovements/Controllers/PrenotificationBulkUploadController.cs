namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.Rules;
    using Infrastructure;
    using Infrastructure.BulkUpload;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.BulkUpload;
    using ViewModels.PrenotificationBulkUpload;

    [Authorize]
    public class PrenotificationBulkUploadController : Controller
    {
        private readonly IMediator mediator;
        private readonly IBulkMovementValidator validator;
        private readonly IFileReader fileReader;

        public PrenotificationBulkUploadController(IMediator mediator, IBulkMovementValidator validator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.validator = validator;
            this.fileReader = fileReader;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            ViewBag.NotificationId = notificationId;

            var ruleSummary = await mediator.SendAsync(new GetMovementRulesSummary(notificationId));

            var updatedRules = ruleSummary.RuleResults.Where(p => p.Rule != MovementRules.ConsentExpiresInThreeOrLessWorkingDays
            && p.Rule != MovementRules.ConsentExpiresInFourWorkingDays
            && p.Rule != MovementRules.HasApprovedFinancialGuarantee
            && p.Rule != MovementRules.FileClosed
            && p.Rule != MovementRules.ActiveLoadsReached);

            MovementRulesSummary v = new MovementRulesSummary(updatedRules);

            if (!v.IsSuccess)
            {
                return GetRuleErrorView(ruleSummary);
            }

            var model = new PrenotificationBulkUploadViewModel(notificationId);
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult ConsentWithdrawn(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentPeriodExpired(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TotalIntendedQuantityReached(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TotalMovementsReached(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TotalIntendedQuantityExceeded(Guid notificationId)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, PrenotificationBulkUploadViewModel model)
        {
            return RedirectToAction("UploadPrenotifications");
        }

        [HttpGet]
        public ActionResult UploadPrenotifications(Guid notificationId)
        {
            var model = new PrenotificationBulkUploadViewModel(notificationId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadPrenotifications(Guid notificationId, PrenotificationBulkUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = notificationId;
                model = new PrenotificationBulkUploadViewModel(notificationId);
                return View(model);
            }

            var validationSummary = await validator.GetPrenotificationValidationSummary(model.File, notificationId);
            var failedFileRules = validationSummary.FileRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).Select(r => r.Rule).ToList();
            var failedContentRules = validationSummary.ContentRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).ToList();
            var warningContentRule = validationSummary.ContentRulesResults.Where(r => r.MessageLevel == MessageLevel.Warning).ToList();
            model.FailedFileRules = failedFileRules;
            model.FailedContentRules = failedContentRules;
            model.WarningContentRules = warningContentRule;
            var shipments = validationSummary.ShipmentNumbers != null ? validationSummary.ShipmentNumbers.ToList() : null;

            var shipmentsModel = new ShipmentMovementDocumentsViewModel(notificationId, shipments, model.File.FileName);

            if (model.ErrorsCount > 0 || model.WarningsCount > 0)
            {
                TempData["PrenotificationShipments"] = shipments;
                TempData["PreNotificationFileName"] = model.File.FileName;
                return View("Errors", model);
            }

            TempData["PrenotificationShipments"] = shipments;
            TempData["PreNotificationFileName"] = model.File.FileName;

            return View("Documents", shipmentsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Errors(Guid notificationId, PrenotificationBulkUploadViewModel model)
        {
            return RedirectToAction("Documents");
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

            // TODO: save data...

            object draftBulkUploadIdObj;

            if (TempData.TryGetValue("DraftBulkUploadId", out draftBulkUploadIdObj))
            {
                var draftBulkUploadId = draftBulkUploadIdObj as Guid?;

                if (draftBulkUploadId != null && draftBulkUploadId != Guid.Empty)
                {
                    var fileExtension = Path.GetExtension(model.File.FileName);
                    var uploadedFile = await fileReader.GetFileBytes(model.File);

                    await
                        mediator.SendAsync(new CreateBulkPrenotification(notificationId, draftBulkUploadId.Value,
                            uploadedFile, fileExtension));

                    TempData["ShipmentMovementFileName"] = model.File.FileName;
                    return RedirectToAction("Success");
                }
            }

            return View(model);
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
        public ActionResult Warning(Guid notificationId)
        {
            var model = new WarningChoiceViewModel(notificationId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Warning(WarningChoiceViewModel model, string cfp)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.GetEnumDisplayValue(WarningChoicesList.Leave).Equals(model.WarningChoices.SelectedValue))
            {
                TempData.Remove("PrenotificationShipments");
                TempData.Remove("PreNotificationFileName");
                TempData.Remove("DraftBulkUploadId");

                return RedirectToAction("Index", "Options", new { area = "NotificationApplication", id = model.NotificationId });
            }
            if (model.GetEnumDisplayValue(WarningChoicesList.Return).Equals(model.WarningChoices.SelectedValue))
            {
                return RedirectToAction("Documents");
            }

            return View(model);
        }

        private ActionResult GetRuleErrorView(MovementRulesSummary ruleSummary)
        {
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalShipmentsReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalMovementsReached");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalIntendedQuantityReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalIntendedQuantityReached");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalIntendedQuantityExceeded && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalIntendedQuantityExceeded");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.HasApprovedFinancialGuarantee && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("NoApprovedFinancialGuarantee");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ActiveLoadsReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalActiveLoadsReached");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentPeriodExpired && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentPeriodExpired");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentExpiresInFourWorkingDays && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentExpiresInFourWorkingDays");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentExpiresInThreeOrLessWorkingDays && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentExpiresInThreeOrLessWorkingDays");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentWithdrawn && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentWithdrawn");
            }
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.FileClosed && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("FileClosed");
            }

            throw new InvalidOperationException("Unknown rule view");
        }
    }
}