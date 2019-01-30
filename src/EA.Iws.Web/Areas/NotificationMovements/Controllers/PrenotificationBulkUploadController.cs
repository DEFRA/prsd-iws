namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Rules;
    using Infrastructure.BulkUpload;
    using Prsd.Core.Mediator;
    using ViewModels.PrenotificationBulkUpload;

    [Authorize]
    public class PrenotificationBulkUploadController : Controller
    {
        private readonly IMediator mediator;
        private readonly IBulkMovementValidator validator;

        public PrenotificationBulkUploadController(IMediator mediator, IBulkMovementValidator validator)
        {
            this.mediator = mediator;
            this.validator = validator;
        }

        [HttpGet]
        public ActionResult Index(Guid notificationId)
        {
            ViewBag.NotificationId = notificationId;
            var model = new PrenotificationBulkUploadViewModel(notificationId);
            return View(model);
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

            var validationSummary = await validator.GetValidationSummary(model.File, notificationId);
            var failedFileRules = validationSummary.FileRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).Select(r => r.Rule).ToList();
            var failedContentRules = validationSummary.ContentRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).ToList();
            model.FailedFileRules = failedFileRules;
            model.FailedContentRules = failedContentRules;

            if (model.ErrorsCount > 0)
            {
                return RedirectToAction("Errors", model);
            }

            var shipments = validationSummary.PrenotificationMovements.Select(p => p.ShipmentNumber).ToList();

            var shipmentsModel = new ShipmentMovementDocumentsViewModel(notificationId, shipments, model.File.FileName);

            TempData["PrenotificationShipments"] = shipments;
            TempData["PreNotificationFileName"] = model.File.FileName;

            return View("Documents", shipmentsModel);
        }

        [HttpGet]
        public ActionResult Documents(Guid notificationId)
        {
            var fileName = string.Empty;
            var shipments = new List<int?>();
            object fileNameObj;
            object shipmentsObj;

            if (TempData.TryGetValue("PreNotificationFileName", out fileNameObj))
            {
                fileName = fileNameObj as string;
            }
            if (TempData.TryGetValue("PrenotificationShipments", out shipmentsObj))
            {
                shipments = shipmentsObj as List<int?>;
            }

            TempData["PrenotificationShipments"] = shipments;
            TempData["PreNotificationFileName"] = fileName;

            var model = new ShipmentMovementDocumentsViewModel(notificationId, shipments, fileName);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Documents(Guid notificationId, ShipmentMovementDocumentsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Warning");
            }

            // TODO: save data...

            return View("Succes", model);
        }

        [HttpGet]
        public ActionResult Errors(Guid notificationId, PrenotificationBulkUploadViewModel model)
        {
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