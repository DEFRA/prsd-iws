namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.BulkUpload;
    using Prsd.Core.Mediator;
    using ViewModels.PrenotificationBulkUpload;

    [Authorize]
    public class ReceiptRecoveryBulkUploadController : Controller
    {
        private readonly IMediator mediator;
        private readonly IBulkMovementValidator validator;
        private readonly IFileReader fileReader;

        public ReceiptRecoveryBulkUploadController(IMediator mediator, IBulkMovementValidator validator, IFileReader fileReader)
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
    }
}