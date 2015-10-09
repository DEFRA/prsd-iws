namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.MovementReceipt;
    using ViewModels;

    public class ReceiptCompleteController : Controller
    {
        private readonly IMediator mediator;

        public ReceiptCompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            return View(new ReceiptCompleteViewModel { NotificationId = notificationId });
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

            await mediator.SendAsync(new SetCertificateOfReceipt(id, uploadedFile, fileExtension));

            return RedirectToAction("ApprovedNotification", "Applicant", new { id = model.NotificationId, area = string.Empty });
        }
    }
}