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
                return View("Errors", model);
            }

            var shipments = validationSummary.PrenotificationMovements.Select(p => p.ShipmentNumber).ToList();

            ShipmentMovementDocumentsViewModel shipmentsModel = new ShipmentMovementDocumentsViewModel(notificationId, shipments, model.File.FileName);

            return View("ShipmentMovementDocuments", shipmentsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadShipmentMovements(Guid notificationId, ShipmentMovementDocumentsViewModel model)
        {
            return View("ShipmentMovementDocuments", model);
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
                return RedirectToAction("Index", "Options", new { area = "NotificationApplication", id = model.NotificationId });
            }
            if (model.GetEnumDisplayValue(WarningChoicesList.Return).Equals(model.WarningChoices.SelectedValue))
            {
                // To do: Send user to upload page for the shipment movement document
                throw new NotImplementedException("Redirection to upload page for the shipment movement document not yet implemented");
            }

            return View(model);
        }
    }
}