namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
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

            var validationSummary = await validator.GetValidationSummary(model.File);
            var failedFileRules = validationSummary.FileRulesResults.Where(r => r.MessageLevel == MessageLevel.Error).Select(r => r.Rule).ToList();
            model.FailedFileRules = failedFileRules;

            return View("Errors", model);
        }

        [HttpGet]
        public ActionResult Errors(Guid notificationId, PrenotificationBulkUploadViewModel model)
        {
            return View(model);
        }
    }
}